using Scalar.AspNetCore;
using TaskManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options.Title = "Task Management API");

    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHsts();

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseExceptionHandler(options => { });

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();
