using System.Reflection;
using Application;
using Application.Utils;
using DotNetEnv;
using Infrastructure;
using Microsoft.AspNetCore.OData;

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

builder.Services.AddControllers()
    .AddOData(options =>
        options.Select()   
               .Filter()  
               .OrderBy() 
               .Expand()   
               .SetMaxTop(100))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    });


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
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

app.MapControllers();   
app.Run();

public partial class Program
{
}