using Application;
using Domain.Entities;
using DotNetEnv;
using Infrastructure;
using Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

builder.Configuration["ConnectionStrings:Host"] = Environment.GetEnvironmentVariable("DB_HOST");
builder.Configuration["ConnectionStrings:Port"] = Environment.GetEnvironmentVariable("DB_PORT");
builder.Configuration["ConnectionStrings:Username"] = Environment.GetEnvironmentVariable("DB_USER");
builder.Configuration["ConnectionStrings:Password"] = Environment.GetEnvironmentVariable("DB_PASSWORD");
builder.Configuration["ConnectionStrings:Database"] = Environment.GetEnvironmentVariable("DB_NAME");
builder.Configuration["CORS:ClientUrl"] = Environment.GetEnvironmentVariable("CLIENT_URL");
builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_SECRET");

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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);

builder.Services.AddControllers().AddOData(opt => opt.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count().AddRouteComponents("odata", GetEdmModel()));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Healthcare Management", Version = "v1" });

    // Add JWT Bearer Authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: Bearer abcdef12345"
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
