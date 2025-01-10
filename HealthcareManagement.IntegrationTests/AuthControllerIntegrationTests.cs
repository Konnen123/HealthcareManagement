using System.Net;
using System.Text;
using Application.Use_Cases.Commands.AuthCommands;
using Application.Utils;
using Domain.Entities.User;
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

    private string BaseUrl = "/api/v1/Auth/reset-password";

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
        const string newPassword = "NewPassword";
        const string mockToken = "MockToken";
        User user = UserSUT();
        var resetPasswordToken = resetPasswordTokenSut(mockToken, user);
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = resetPasswordCommandSut(newPassword, mockToken);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);

        HttpClient client = this.factory.CreateClient();
        var response = await client.PutAsync(BaseUrl, new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        var token = dbContext.ResetPasswordTokens.FirstOrDefault(t => t.UserId == user.UserId);
        Assert.Null(token);
        var updatedUser = dbContext.Users.FirstOrDefault(u => u.UserId == user.UserId);
        Assert.NotNull(updatedUser);
    }

    [Fact]
    public async Task SendForgotPasswordMail_GivenResetPasswordCommandInvalid_ShouldReturnUnauthorizedResponse()
    {
        const string newPassword = "NewPassword";
        const string mockToken = "invalidToken";
        User user = UserSUT();
        var resetPasswordToken = resetPasswordTokenSut(mockToken, user);
        resetPasswordToken.ExpiresAt = DateTime.UtcNow;
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = resetPasswordCommandSut(newPassword, mockToken);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);

        HttpClient client = this.factory.CreateClient();
        var response = await client.PutAsync(BaseUrl, new StringContent(
            serialize, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task SendForgotPasswordMail_GivenResetPasswordCommandInvalid_ShouldReturnBadRequestResponse()
    {
        const string newPassword = "NewPassword";
        const string mockToken = "invalidToken";
        User user = UserSUT();
        var resetPasswordToken = resetPasswordTokenSut(mockToken, user);
        resetPasswordToken.ExpiresAt = DateTime.UtcNow;
        dbContext.Users.Add(user);
        dbContext.ResetPasswordTokens.Add(resetPasswordToken);
        await dbContext.SaveChangesAsync();
        var resetPasswordCommand = invalidResetPasswordCommandSut(newPassword);

        var serialize = JsonConvert.SerializeObject(resetPasswordCommand);

        HttpClient client = this.factory.CreateClient();
        var response = await client.PutAsync(BaseUrl, new StringContent(
            serialize, Encoding.UTF8, "application/json"));

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
            MedicalRank = "mock"
        };
    }

    private static ResetPasswordCommand resetPasswordCommandSut(string password, string token)
    {
        return new ResetPasswordCommand()
        {
            Password = password,
            Token = token
        };
    }
    private static ResetPasswordCommand invalidResetPasswordCommandSut(string password)
    {
        return new ResetPasswordCommand()
        {
            Password = password
        };
    }

    private static ResetPasswordToken resetPasswordTokenSut(string token, User user)
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

    #endregion

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}