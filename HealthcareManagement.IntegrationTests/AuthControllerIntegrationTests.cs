using System.Net;
using System.Text;
using Application.Use_Cases.Commands.AuthCommands;
using Application.Utils;
using Domain.Entities.Tokens;
using Domain.Entities.User;
using Domain.Utils;
using FluentAssertions;
using Identity.Persistence;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HealthcareManagement.IntegrationTests;

public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> factory;
    private readonly UsersDbContext dbContext;
    private string BaseUrl = "/api/v1/Auth/";

    public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
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
            });
        });


        var scope = this.factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        dbContext.Database.EnsureCreated();
    }


    #region ResetUserPassword

    [Fact]
    public async Task ResetUserPassword_GivenResetPasswordCommandValid_ShouldReturnOkResponse()
    {
        //Arrange
        const string newPassword = "NewPassword";
        const string mockToken = "mocked-reset-password-token";
        User user = UserSUT();
        var resetPasswordToken = ResetPasswordTokenSUT(mockToken, user);
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = ResetPasswordCommandSUT(newPassword, mockToken);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PutAsync($"{BaseUrl}reset-password", new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        //Assert
        response.EnsureSuccessStatusCode();
        var token = dbContext.ResetPasswordTokens.FirstOrDefault(t => t.UserId == user.UserId);
        Assert.Null(token);
        var updatedUser = dbContext.Users.FirstOrDefault(u => u.UserId == user.UserId);
        Assert.NotNull(updatedUser);
    }

    [Fact]
    public async Task SendForgotPasswordMail_GivenResetPasswordCommandInvalid_ShouldReturnUnauthorizedResponse()
    {
        //Arrange
        const string newPassword = "NewPassword";
        const string mockToken = "invalidToken";
        User user = UserSUT();
        var resetPasswordToken = ResetPasswordTokenSUT(mockToken, user);
        resetPasswordToken.ExpiresAt = DateTime.UtcNow;
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = ResetPasswordCommandSUT(newPassword, mockToken);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PutAsync($"{BaseUrl}reset-password", new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SendForgotPasswordMail_GivenResetPasswordCommandInvalid_ShouldReturnBadRequestResponse()
    {
        //Arrange
        const string newPassword = "NewPassword";
        const string mockToken = "invalidToken";
        User user = UserSUT();
        var resetPasswordToken = ResetPasswordTokenSUT(mockToken, user);
        resetPasswordToken.ExpiresAt = DateTime.UtcNow;
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = ResetPasswordCommandInvalidSUT(newPassword);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PutAsync($"{BaseUrl}reset-password", new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Login

    [Fact]
    public async Task Login_GivenLoginCommandBadCredentials_ShouldReturnUnauthorizedResponse()
    {
        //Arrange
        User user = UserSUT();
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        const string invalidPassword = "InvalidPassword";
        var loginUserCommand = LoginUserCommandSUT(user.Email, invalidPassword);
        user.Password = hashPassword;
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var serialize = JsonConvert.SerializeObject(loginUserCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PostAsync($"{BaseUrl}Login", new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    
    [Fact]
    public async Task Login_GivenLoginCommandInvalid_ShouldReturnBadRequestResponse()
    {
        //Arrange
        var registerUserCommand = ResetPasswordCommandSUT("mock", "mock");
        var serialize = JsonConvert.SerializeObject(registerUserCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PostAsync($"{BaseUrl}Login", new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Register

    [Fact]
    public async Task Register_GivenRegisterCommandInvalid_ShouldReturnBadRequestResponse()
    {
        //Arrange
        var user = PatientSUT();
        var registerUserCommand = RegisterUserCommandSUT(user);
        registerUserCommand.Email = "invalid-email";
        
        var serialize = JsonConvert.SerializeObject(registerUserCommand);
        HttpClient client = this.factory.CreateClient();

        //Act
        var response = await client.PostAsync($"{BaseUrl}Register", new StringContent(
            serialize, Encoding.UTF8, "application/json"));
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
            MedicalRank = "Mock",
            UserId = Guid.Empty,
        };
    }
    
    private static User PatientSUT()
    {
        return new Patient()
        {
            FirstName = "mock",
            LastName = "mock",
            Email = "mockemail@example.com",
            Password = "mockPassword",
            PhoneNumber = "0752122923",
            DateOfBirth = new DateOnly(2000, 2, 2),
            CreatedAt = DateSingleton.GetCurrentDateOnly(),
            IsEnabled = true,
            UserId = Guid.Empty,
            Role = Roles.PATIENT
        };
    }

    private static ResetPasswordCommand ResetPasswordCommandSUT(string password, string token)
    {
        return new ResetPasswordCommand()
        {
            Password = password,
            Token = token
        };
    }

    private static ResetPasswordCommand ResetPasswordCommandInvalidSUT(string password)
    {
        return new ResetPasswordCommand()
        {
            Password = password
        };
    }

    private static ResetPasswordToken ResetPasswordTokenSUT(string token, User user)
    {
        return new ResetPasswordToken()
        {
            Token = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            UserId = user.UserId,
            UserAuthentication = user
        };
    }

    private static LoginUserCommand LoginUserCommandSUT(string email, string password)
    {
        return new LoginUserCommand()
        {
            Email = email,
            Password = password
        };
    }

    private static RegisterUserCommand RegisterUserCommandSUT(User user)
    {
        return new RegisterUserCommand()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Role = user.Role
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