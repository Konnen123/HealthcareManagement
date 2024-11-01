using Application;
using DotNetEnv;
using Infrastructure;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Configuration.AddEnvironmentVariables();

builder.Configuration["ConnectionStrings:Host"] = Environment.GetEnvironmentVariable("DB_HOST");
builder.Configuration["ConnectionStrings:Port"] = Environment.GetEnvironmentVariable("DB_PORT");
builder.Configuration["ConnectionStrings:Username"] = Environment.GetEnvironmentVariable("DB_USER");
builder.Configuration["ConnectionStrings:Password"] = Environment.GetEnvironmentVariable("DB_PASSWORD");
builder.Configuration["ConnectionStrings:Database"] = Environment.GetEnvironmentVariable("DB_NAME");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
