using Application;
using Domain.Entities;
using DotNetEnv;
using HealthcareManagement.JsonConverters;
using Infrastructure;
using Identity;
using Identity.Middleware;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

string defaultConnectionStringPrefix = "ConnectionStrings:DefaultConnection:";
string identityConnectionStringPrefix = "ConnectionStrings:IdentityConnection:";

builder.Configuration[$"{defaultConnectionStringPrefix}Host"] = Environment.GetEnvironmentVariable("DB_HOST");
builder.Configuration[$"{defaultConnectionStringPrefix}Port"] = Environment.GetEnvironmentVariable("DB_PORT");
builder.Configuration[$"{defaultConnectionStringPrefix}Username"] = Environment.GetEnvironmentVariable("DB_USER");
builder.Configuration[$"{defaultConnectionStringPrefix}Password"] = Environment.GetEnvironmentVariable("DB_PASSWORD");
builder.Configuration[$"{defaultConnectionStringPrefix}Database"] = Environment.GetEnvironmentVariable("DB_NAME");

builder.Configuration[$"{identityConnectionStringPrefix}Host"] = Environment.GetEnvironmentVariable("IDENTITY_DB_HOST");
builder.Configuration[$"{identityConnectionStringPrefix}Port"] = Environment.GetEnvironmentVariable("IDENTITY_DB_PORT");
builder.Configuration[$"{identityConnectionStringPrefix}Username"] = Environment.GetEnvironmentVariable("IDENTITY_DB_USER");
builder.Configuration[$"{identityConnectionStringPrefix}Password"] = Environment.GetEnvironmentVariable("IDENTITY_DB_PASSWORD");
builder.Configuration[$"{identityConnectionStringPrefix}Database"] = Environment.GetEnvironmentVariable("IDENTITY_DB_NAME");

builder.Configuration["CORS:ClientUrl"] = Environment.GetEnvironmentVariable("CLIENT_URL");
builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_SECRET");
builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
builder.Configuration["Jwt:AccessExpiry"] = Environment.GetEnvironmentVariable("JWT_ACCESS_EXPIRY_TIME_SECONDS");
builder.Configuration["Jwt:RefreshExpiry"] = Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRY_TIME_DAYS");

builder.Configuration["ML:DiagnosisModelPath"] = Environment.GetEnvironmentVariable("DIAGNOSIS_MODEL_PATH");
builder.Configuration["ML:OutputFilePath"] = Environment.GetEnvironmentVariable("OUTPUT_DIAGNOSIS_PATH");

builder.Configuration["AWS:AccessKey"] = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
builder.Configuration["AWS:SecretKey"] = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
builder.Configuration["AWS:Region"] = Environment.GetEnvironmentVariable("AWS_REGION");

var MyAllowSpecificOrigin = "MyAllowSpecificOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigin,
                    policy =>
                    {
                        policy.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:4200");
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, defaultConnectionStringPrefix);
builder.Services.AddIdentity(builder.Configuration, identityConnectionStringPrefix);


builder.Services.AddControllers().AddOData(opt => opt.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count().AddRouteComponents("odata", GetEdmModel()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UserRoleConverter());
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigin);

app.UseHttpsRedirection();

app.UseAuthentication();


app.UseMiddleware<ResponseMiddleware>();

app.UseAuthorization();

app.MapControllers();   
app.Run();

IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<Appointment>("Appointments");
    return odataBuilder.GetEdmModel();
}

public partial class Program
{
}
