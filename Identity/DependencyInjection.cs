using System.Text;
using Application.Utils;
using Domain.Repositories;
using Domain.Services;
using Identity.Persistence;
using Identity.Repositories;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;

namespace Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services, 
            IConfiguration configuration, string connectionStringPrefix)
        {
            DbContextSingleton.AddDbContext<UsersDbContext>(services, configuration, connectionStringPrefix);

            string jwtSecret = configuration["Jwt:Key"] ?? "testJwt";
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            services.AddMemoryCache();
            services.AddTransient(sp =>
            {
                return new RequestDelegate(context =>
                {
                    return Task.CompletedTask;
                });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DOCTOR_PATIENT", policy =>
                    policy.RequireRole("DOCTOR", "PATIENT"));

                options.AddPolicy("DOCTOR", policy =>
                    policy.RequireRole("DOCTOR"));

                options.AddPolicy("PATIENT", policy =>
                    policy.RequireRole("PATIENT"));
            });
            
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IPatientsRepository, PatientsRepository>();
            services.AddScoped<IDoctorsRepository, DoctorsRepository>();
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFailedLoginAttemptsRepository, FailedLoginAttemptsRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();
            services.AddScoped<IMailService, SmtpEmailService>();
          
            return services;
        }
    }
}
