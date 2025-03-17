using TaskManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Tasks API";
        return Task.CompletedTask;
    });
});

builder.Services.AddProblemDetails();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));

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
