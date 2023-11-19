using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using RESTServer;
using System.Net.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<ICustomerRepository, FileCustomerRepository>();
builder.Services.AddSingleton<ICutomerValidator, CustomerValidator>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//await app.PrepareData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context =>
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var problem = new ProblemDetails
        {
            Status = 500,
            Title = "An exception was thrown.",
            Detail = ""
        };
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        problem.Detail = exceptionHandlerPathFeature?.Error.Message;
        if (exceptionHandlerPathFeature?.Error is CustoemrValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            problem.Detail = exceptionHandlerPathFeature.Error.Message;
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Title = "Validation error";

        }
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }));


app.MapGet("/customers", ([FromServices] ICustomerService customerService) =>
{
    var customers = customerService.GetCustomers();
    return customers;
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
.Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
.WithOpenApi();

app.MapPost("/customers", ([FromBody] List<Customer> customers, [FromServices] ICustomerService customerService) =>
{
    customerService.AddCustomers(customers);
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
.Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
.WithOpenApi();

app.Run("http://localhost:5000");
