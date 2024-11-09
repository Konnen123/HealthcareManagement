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

        [Fact]
        public async Task CreateAppointment_GivenCreateAppointmentCommandValid_ShouldReturnCreatedResponse()
        {

            // Arrange
            HttpClient client = factory.CreateClient();

            // Act
            var response = await client.PostAsync(BaseUrl, new StringContent(
                JsonConvert.SerializeObject(CreateAppointmentCommandMock(10)), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Trim().Should().Be("application/json; charset=utf-8");
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        public void Dispose()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
        }

        private CreateAppointmentCommand CreateAppointmentCommandMock(int appointmentDurationInMinutes)
        {

            return new CreateAppointmentCommand
            {
                PatientId = Guid.NewGuid(),
                Date = DateManager.Instance.GetCurrentDateTime(),
                StartTime = DateManager.Instance.GetCurrentDateTime(),
                EndTime = DateManager.Instance.GetCurrentDateTime().AddMinutes(appointmentDurationInMinutes),
                UserNotes = "Test User Notes",
                DoctorId = Guid.NewGuid()
            };
        }
    }
}