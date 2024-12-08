using Domain.Repositories;
using Identity.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Text;
using Domain.Services;
using Identity.Persistence;
using Identity.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

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
                options.AddPolicy("DOCTOR_PACIENT", policy =>
                    policy.RequireRole("DOCTOR", "PACIENT"));

                options.AddPolicy("DOCTOR", policy =>
                    policy.RequireRole("DOCTOR"));

                options.AddPolicy("PACIENT", policy =>
                    policy.RequireRole("PACIENT"));
            });

            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFailedLoginAttemptsRepository, FailedLoginAttemptsRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
          
            return services;
        }
    }
}
