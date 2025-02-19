using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Application.Use_Cases.Commands;
using Application.Use_Cases.Commands.AppointmentCommands;
using Application.Utils;
using Domain.Entities;
using Domain.Entities.User;
using Domain.Utils;
using DotNetEnv;
using FluentAssertions;
using Identity.Persistence;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HealthcareManagement.IntegrationTests
{
    public class AppointmentsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> factory;
        private readonly ApplicationDbContext dbContext;
        private readonly UsersDbContext usersDbContext;

        private string BaseUrl = "/api/v1/Appointments";

        public AppointmentsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                Env.Load();
                var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL");

                builder.ConfigureServices(services =>
                {
                    var descriptors = services.Where(
                        d => d.ServiceType.FullName.Contains("Microsoft.EntityFrameworkCore")).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                    services.AddDbContext<UsersDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = TestAuthenticationSchemeProvider.Name;
                        options.DefaultChallengeScheme = TestAuthenticationSchemeProvider.Name;
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                        TestAuthenticationSchemeProvider.Name,
                        _ => { });
                });
            });

            var scope = this.factory.Services.CreateScope();
            dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            dbContext.Database.EnsureCreated();
        }
        #region CreateAppointment
        [Fact]
        public async Task CreateAppointment_GivenCreateAppointmentCommandValid_ShouldReturnCreatedResponse()
        {
            // Arrange
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(doctor);
            usersDbContext.Users.Add(patient);
            await usersDbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);
            var appointment = CreateAppointmentCommandSUT(10, doctor.UserId);
            var serialize = JsonConvert.SerializeObject(appointment);

            // Act
            var response = await client.PostAsync(BaseUrl, new StringContent(
                serialize, Encoding.UTF8, "application/json"));

            Console.WriteLine(client.DefaultRequestHeaders.Authorization);
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
            usersDbContext.Doctors.Add(doctor);
            usersDbContext.Patients.Add(patient);
            await usersDbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

            // Act
            var response = await client.PostAsync(BaseUrl, new StringContent(
                JsonConvert.SerializeObject(CreateAppointmentCommandSUT(-10, doctor.UserId)), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
        #endregion

        #region GetAppointmentById
        [Fact]
        public async Task GetAppointmentById_GivenEntityIsInTheDatabase_ShouldReturnAppointmentDto()
        {
            // Arrange
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

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
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

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
            User doctor = DoctorSUT();
            User doctor2 = DoctorSUT();
            User patient = PatientSUT();
            User patient2 = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(patient2);
            usersDbContext.Users.Add(doctor);
            usersDbContext.Users.Add(doctor2);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment1 = AppointmentSUT(10, patient.UserId, doctor.UserId);
            Appointment appointment2 = AppointmentSUT(15, patient2.UserId, doctor2.UserId);
            dbContext.Appointments.Add(appointment1);
            dbContext.Appointments.Add(appointment2);
            await dbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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

            foreach (var returnedAppointment in returnedAppointments)
            {
                returnedAppointment.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetAllAppointments_WhenAppointmentsTableIsEmpty_ShouldReturnNoContentResponse()
        {
            // Arrange
            User doctor = DoctorSUT();
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

            // Act
            var response = await client.GetAsync(BaseUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        #endregion

        #region CancelAppointment
        [Fact]
        public async Task CancelAppointment_GivenCancelAppointmentCommandValid_ShouldReturnNoContentResponse()
        {
            // Arrange
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            CancelAppointmentCommand cancelAppointmentCommand = CancelAppointmentCommandSUT(appointment.Id, appointment.PatientId);
            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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
            Patient patient = PatientSUT();
            usersDbContext.Patients.Add(patient);
            await usersDbContext.SaveChangesAsync();

            CancelAppointmentCommand cancelAppointmentCommand = CancelAppointmentCommandSUT(Guid.NewGuid(), Guid.NewGuid());
            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

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
            Doctor doctor = DoctorSUT();
            Patient patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointment.Id, appointment.DoctorId, 20);

            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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
            // Arrange
            Doctor doctor = DoctorSUT();
            usersDbContext.Doctors.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Guid appointmentId = Guid.NewGuid();

            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointmentId, doctor.UserId, 20);
            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

            // Act
            var response = await client.PutAsync($"{BaseUrl}/{appointmentId}", new StringContent(
                JsonConvert.SerializeObject(updateAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAppointment_GivenUpdateAppointmentCommand_WhenAppointmentIdFromUrlDoesNotMatchAppointmentIdFromRequestBody_ShouldReturnBadRequestResponse()
        {
            // Arrange
            Doctor doctor = DoctorSUT();
            usersDbContext.Doctors.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Guid appointmentId = Guid.NewGuid();
            UpdateAppointmentCommand updateAppointmentCommand = UpdateAppointmentCommandSUT(appointmentId, doctor.UserId, 20);
            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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
            Doctor doctor = DoctorSUT();
            usersDbContext.Doctors.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            HttpClient client = CreateAuthenticatedClient(Roles.DOCTOR, doctor.UserId);

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
            User doctor = DoctorSUT();
            User patient = PatientSUT();
            usersDbContext.Users.Add(patient);
            usersDbContext.Users.Add(doctor);
            await usersDbContext.SaveChangesAsync();

            Appointment appointment = AppointmentSUT(10, patient.UserId, doctor.UserId);
            dbContext.Appointments.Add(appointment);
            await dbContext.SaveChangesAsync();

            RescheduleAppointmentCommand rescheduleAppointmentCommand = RescheduleAppointmentCommandSUT(appointment.PatientId, appointment.Id);

            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

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
            Patient patient = PatientSUT();
            usersDbContext.Patients.Add(patient);
            await usersDbContext.SaveChangesAsync();

            RescheduleAppointmentCommand rescheduleAppointmentCommand = RescheduleAppointmentCommandSUT(Guid.NewGuid(), Guid.NewGuid());
            HttpClient client = CreateAuthenticatedClient(Roles.PATIENT, patient.UserId);

            // Act
            var response = await client.PatchAsync($"{BaseUrl}/Reschedule/{Guid.NewGuid()}", new StringContent(
                JsonConvert.SerializeObject(rescheduleAppointmentCommand), Encoding.UTF8, "application/json"));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        #region SUTs

        private static CreateAppointmentCommand CreateAppointmentCommandSUT(int appointmentDurationInMinutes, Guid doctorId)
        {
            return new CreateAppointmentCommand
            {
                Date = DateSingleton.GetCurrentDateOnly().AddDays(1),
                StartTime = TimeSingleton.GetCurrentTimeOnly(),
                EndTime = TimeSingleton.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                UserNotes = "Test User Notes",
                DoctorId = doctorId
            };
        }

        private static Appointment AppointmentSUT(int appointmentDurationInMinutes, Guid patientId, Guid doctorId)
        {
            return new Appointment
            {
                Date = DateSingleton.GetCurrentDateOnly(),
                StartTime = TimeSingleton.GetCurrentTimeOnly(),
                EndTime = TimeSingleton.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                PatientId = patientId,
                DoctorId = doctorId,
                UserNotes = "Test User Notes"
            };
        }

        private static CancelAppointmentCommand CancelAppointmentCommandSUT(Guid appointmentId, Guid pacientId)
        {
            return new CancelAppointmentCommand
            {
                AppointmentId = appointmentId,
                CancellationReason = "Test Cancellation Reason",
                PatientId = pacientId
            };
        }

        private static UpdateAppointmentCommand UpdateAppointmentCommandSUT(Guid appointmentId, Guid doctorId, int appointmentDurationInMinutes)
        {
            return new UpdateAppointmentCommand
            {
                Id = appointmentId,
                Date = DateSingleton.GetCurrentDateOnly(),
                StartTime = TimeSingleton.GetCurrentTimeOnly(),
                EndTime = TimeSingleton.GetCurrentTimeOnly().AddMinutes(appointmentDurationInMinutes),
                UserNotes = "Test User Notes",
                DoctorId = doctorId,
            };
        }

        private static RescheduleAppointmentCommand RescheduleAppointmentCommandSUT(Guid patientId, Guid appointmentId)
        {
            return new RescheduleAppointmentCommand
            {
                PatientId = patientId,
                AppointmentId = appointmentId,
                NewDate = DateSingleton.GetCurrentDateOnly(),
                NewStartTime = TimeSingleton.GetCurrentTimeOnly(),
            };
        }

        private static Doctor DoctorSUT()
        {
            return new Doctor
            {
                FirstName = "mock",
                LastName = "mock",
                Email = "mockemail@example.com",
                Password = "mockPassword",
                PhoneNumber = "0752122923",
                DateOfBirth = new DateOnly(2000, 2, 2),
                CreatedAt = DateSingleton.GetCurrentDateOnly(),
                IsEnabled = true,
                MedicalRank = "mock"
            };
        }

        private static Patient PatientSUT()
        {
            return new Patient
            {
                FirstName = "mock",
                LastName = "mock",
                Email = "mockemail@example.com",
                Password = "mockPassword",
                PhoneNumber = "0752122923",
                DateOfBirth = new DateOnly(2000, 2, 2),
                CreatedAt = DateSingleton.GetCurrentDateOnly(),
                IsEnabled = true
            };
        }

        private HttpClient CreateAuthenticatedClient(Roles role, Guid userId)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationSchemeProvider.Name);
            client.DefaultRequestHeaders.Add("role", role.ToString());
            client.DefaultRequestHeaders.Add("nameidentifier", userId.ToString());
            return client;
        }
        #endregion
    }
}