using System.Net;
using System.Text;
using Application.Use_Cases.Commands.MailCommands;
using Application.Utils;
using Domain.Entities.User;
using Domain.Services;
using Domain.Utils;
using FluentAssertions;
using Identity.Persistence;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace HealthcareManagement.IntegrationTests;


public class MailControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly UsersDbContext dbContext;
    private readonly Mock<IMailService> mockEmailService;

    private string BaseUrl = "/api/v1/Mail/forgot-password";

    public MailControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        mockEmailService = new Mock<IMailService>();
        mockEmailService
            .Setup(service => service.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<string>.Success("MockEmailSent"));
        
        this.factory = factory.WithWebHostBuilder(builder =>
        {
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

                services.AddSingleton(mockEmailService.Object);
            });
        });
        

        var scope = this.factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Database.EnsureCreated();
    }
    
    #region SendForgotPasswordMail

    [Fact]
    public async Task SendForgotPasswordMail_GivenForgotPasswordCommandValid_ShouldReturnOkResponse()
    {
        //Arrange
        User user = UserSUT();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        var forgotPasswordCommand = ForgotPasswordCommandSUT(user.Email);
        var serialize = JsonConvert.SerializeObject(forgotPasswordCommand);
        HttpClient client = this.factory.CreateClient();
        
        //Act
        var response = await client.PostAsync(BaseUrl, new StringContent(
            serialize, Encoding.UTF8, "application/json"));
        
        //Assert
        response.EnsureSuccessStatusCode();
        
        mockEmailService.Verify(service => 
                service.SendEmailAsync(
                    It.Is<string>(to => to == user.Email),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SendForgotPasswordMail_GivenForgotPasswordCommandInvalid_ShouldReturnBadRequestResponse()
    {
        //Arrange
        const string INVALID_MAIL = "invalid-email";
        User user = UserSUT();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        var forgotPasswordCommand = ForgotPasswordCommandSUT(INVALID_MAIL);
        var serialize = JsonConvert.SerializeObject(forgotPasswordCommand);
        HttpClient client = this.factory.CreateClient();
        
        //Act
        var response = await client.PostAsync(BaseUrl, new StringContent(
            serialize, Encoding.UTF8, "application/json"));
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        mockEmailService.Verify(service => 
                service.SendEmailAsync(
                    It.Is<string>(to => to == INVALID_MAIL),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);
        
    }
    #endregion
    
    #region SUTs
    private static User UserSUT()
    {
        return new Doctor()
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

    private static ForgotPasswordCommand ForgotPasswordCommandSUT(string email)
    {
        return new ForgotPasswordCommand()
        {
            Email = email
        };
    }
    
    #endregion
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}