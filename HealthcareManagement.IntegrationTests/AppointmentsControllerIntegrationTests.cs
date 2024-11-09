using Application.Use_Cases.Commands;
using Application.Utils;
using System.Net;
using System.Text;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using FluentAssertions;
using Domain.Entities;
using Application.Use_Cases.Commands.AppointmentCommands;

namespace HealthcareManagement.IntegrationTests
{
    public class AppointmentsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;

        private string BaseUrl = "/api/v1/Appointments";

        public AppointmentsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                });
            });

            var scope = this.factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }
        #region CreateAppointment
        [Fact]
        public async Task CreateAppointment_GivenCreateAppointmentCommandValid_ShouldReturnCreatedResponse()
        {
            // Arrange
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            dbContext.Users.Add(doctor);
            dbContext.Users.Add(patient);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();
            var appointment = CreateAppointmentCommandSUT(10, patient.Id, doctor.Id);
            var serialize = JsonConvert.SerializeObject(appointment);

            // Act
            var response = await client.PostAsync(BaseUrl, new StringContent(
                serialize, Encoding.UTF8, "application/json"));


            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Trim().Should().Be("application/json; charset=utf-8");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateAppointment_GivenCreateAppointmentCommand_WhenEndTimeIsBeforeStartTime_ShouldReturnBadRequest()
        {
            // Arrange
            Doctor doctor = DoctorSUT();
            Patient patient = PatientSUT();
            dbContext.Doctors.Add(doctor);
            dbContext.Patients.Add(patient);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PostAsync(BaseUrl, new StringContent(
                JsonConvert.SerializeObject(CreateAppointmentCommandSUT(-10, patient.Id, doctor.Id)), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        #endregion

        #region GetAppointmentById
        [Fact]
        public async Task GetAppointmentById_GivenEntityIsInTheDatabase_ShouldReturnAppointmentDto()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{BaseUrl}/{appointment.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Trim().Should().Be("application/json; charset=utf-8");

            var responseString = await response.Content.ReadAsStringAsync();
            var returnedAppointment = JsonConvert.DeserializeObject<Appointment>(responseString);
            returnedAppointment.Should().NotBeNull();
            returnedAppointment.Id.Should().Be(appointment.Id);
        }

        [Fact]
        public async Task GetAppointmentById_GivenEntityIsNotInTheDatabase_ShouldReturnNotFoundResponse()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{BaseUrl}/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Content.Headers.ContentType.ToString().Trim().Should().Be("application/json; charset=utf-8");
        }

        #endregion

        #region GetAllAppointments

        [Fact]
        public async Task GetAllAppointments_GivenEntitiesAreInTheDatabase_ShouldReturnListOfAppointmentDtos()
        {
            // Arrange
            Appointment appointment1 = AppointmentSUT(10);
            Appointment appointment2 = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment1);
            dbContext.Appointments.Add(appointment2);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Trim().Should().Be("application/json; charset=utf-8");

            var responseString = await response.Content.ReadAsStringAsync();
            var returnedAppointments = JsonConvert.DeserializeObject<List<Appointment>>(responseString);
            returnedAppointments.Should().NotBeNull();
            returnedAppointments.Count.Should().Be(2);

            foreach(var returnedAppointment in returnedAppointments)
            {
                returnedAppointment.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetAllAppointments_WhenAppointmentsTableIsEmpty_ShouldReturnNoContentResponse()
        {
            // Arrange

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        #endregion

        #region CancelAppointment
        [Fact]
        public async Task CancelAppointment_GivenCancelAppointmentCommandValid_ShouldReturnNoContentResponse()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            CancelAppointmentCommand cancelAppointmentCommand = CancelAppointmentCommandSUT(appointment.Id, appointment.PatientId);
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PatchAsync($"{BaseUrl}/Cancel", new StringContent(
                JsonConvert.SerializeObject(cancelAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task CancelAppointment_GivenCancelAppointmentCommand_WhenAppointmentIdDoesNotExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            CancelAppointmentCommand cancelAppointmentCommand = CancelAppointmentCommandSUT(Guid.NewGuid(), Guid.NewGuid());
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PatchAsync($"{BaseUrl}/Cancel", new StringContent(
                JsonConvert.SerializeObject(cancelAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region UpdateAppointment
        [Fact]
        public async Task UpdateAppointment_GivenUpdateAppointmentCommandValid_ShouldReturnNoContentResponse()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointment.Id, appointment.PatientId, 20);

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PutAsync($"{BaseUrl}/{appointment.Id}", new StringContent(
                JsonConvert.SerializeObject(updateAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateAppointment_GivenUpdateAppointmentCommand_WhenAppointmentIdDoesNotExist_ShouldReturnBadRequestResponse()
        {
            Guid appointmentId = Guid.NewGuid();
            // Arrange
            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointmentId, Guid.NewGuid(), 20);
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PutAsync($"{BaseUrl}/{appointmentId}", new StringContent(
                JsonConvert.SerializeObject(updateAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAppointment_GivenUpdateAppointmentCommand_WhenAppointmentIdFromUrlDoesNotMatchAppointmentIdFromRequestBody_ShouldReturnBadRequestResponse()
        {
            Guid appointmentId = Guid.NewGuid();
            // Arrange
            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointmentId, Guid.NewGuid(), 20);
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PutAsync($"{BaseUrl}/{Guid.NewGuid}", new StringContent(
                JsonConvert.SerializeObject(updateAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region DeleteAppointment

        [Fact]
        public async Task DeleteAppointment_GivenAppointmentIdExists_ShouldReturnNoContentResponse()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"{BaseUrl}/{appointment.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAppointment_GivenAppointmentIdDoesNotExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.DeleteAsync($"{BaseUrl}/{Guid.NewGuid()}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region RescheduleAppointment

        [Fact]
        public async Task RescheduleAppointment_GivenRescheduleAppointmentCommandValid_ShouldReturnNoContentResponse()
        {
            // Arrange
            Appointment appointment = AppointmentSUT(10);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            RescheduleAppointmentCommand rescheduleAppointmentCommand = RescheduleAppointmentCommandSUT(appointment.PatientId, appointment.Id);

            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PatchAsync($"{BaseUrl}/Reschedule/{appointment.Id}", new StringContent(
                JsonConvert.SerializeObject(rescheduleAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RescheduleAppointment_GivenRescheduleAppointmentCommand_WhenAppointmentIdDoesNotExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            RescheduleAppointmentCommand rescheduleAppointmentCommand = RescheduleAppointmentCommandSUT(Guid.NewGuid(), Guid.NewGuid());
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PatchAsync($"{BaseUrl}/Reschedule/{Guid.NewGuid()}", new StringContent(
                JsonConvert.SerializeObject(rescheduleAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }
        #region SUTs

        private CreateAppointmentCommand CreateAppointmentCommandSUT(int appointmentDurationInMinutes, Guid patientId, Guid doctorId)
        {
            return new CreateAppointmentCommand
            {
                PatientId = patientId,
                Date = DateManager.Instance.GetCurrentDateOnly().AddDays(1),
                StartTime = TimeManager.Instance.GetCurrentTimeOnly(),
                EndTime = TimeManager.Instance.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                UserNotes = "Test User Notes",
                DoctorId = doctorId
            };
        }

        private Appointment AppointmentSUT(int appointmentDurationInMinutes)
        {
            return new Appointment
            {
                Id = Guid.NewGuid(),
                Date = DateManager.Instance.GetCurrentDateOnly(),
                StartTime = TimeManager.Instance.GetCurrentTimeOnly(),
                EndTime = TimeManager.Instance.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                UserNotes = "Test User Notes"
            };
        }

        private CancelAppointmentCommand CancelAppointmentCommandSUT(Guid appointmentId, Guid pacientId)
        {
            return new CancelAppointmentCommand
            {
                AppointmentId = appointmentId,
                CancellationReason = "Test Cancellation Reason",
                PatientId = pacientId
            };
        }

        private UpdateAppointmentCommand UpdateAppointmentCommandSUT(Guid appointmentId, Guid patientId , int appointmentDurationInMinutes)
        {
            return new UpdateAppointmentCommand
            {
                Id = appointmentId,
                PatientId = patientId,
                Date = DateManager.Instance.GetCurrentDateOnly(),
                StartTime = TimeManager.Instance.GetCurrentTimeOnly(),
                EndTime = TimeManager.Instance.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                UserNotes = "Test User Notes",
                DoctorId = Guid.NewGuid()
            };
        }

        private RescheduleAppointmentCommand RescheduleAppointmentCommandSUT(Guid patientId, Guid appointmentId)
        {
            return new RescheduleAppointmentCommand
            {
                PatientId = patientId,
                AppointmentId = appointmentId,
                NewDate = DateManager.Instance.GetCurrentDateOnly(),
                NewStartTime = TimeManager.Instance.GetCurrentTimeOnly(),
            };
        }

        private Doctor DoctorSUT()
        {
            return new Doctor
            {
                FirstName = "mock",
                LastName = "mock",
                Email = "mockemail@example.com",
                Password = "mockPassword",
                PhoneNumber = "0752122923",
                DateOfBirth = new DateOnly(2000, 2, 2),
                CreatedAt = DateManager.Instance.GetCurrentDateOnly(),
                IsEnabled = true,
                MedicalRank = "mock"
            };
        }

        private Patient PatientSUT()
        {
            return new Patient
            {
                FirstName = "mock",
                LastName = "mock",
                Email = "mockemail@example.com",
                Password = "mockPassword",
                PhoneNumber = "0752122923",
                DateOfBirth = new DateOnly(2000, 2, 2),
                CreatedAt = DateManager.Instance.GetCurrentDateOnly(),
                IsEnabled = true
            };
        }

        #endregion
    }
}