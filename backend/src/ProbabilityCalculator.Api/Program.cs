using FluentValidation;
using ProbabilityCalculator.Api.Logging;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;
using ProbabilityCalculator.Api.Services;
using ProbabilityCalculator.Api.Validation;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// All operations injected as IEnumerable<ICalculationOperation>
builder.Services.AddSingleton<ICalculationOperation, CombinedWithOperation>();
builder.Services.AddSingleton<ICalculationOperation, EitherOperation>();

// Validation
builder.Services.AddScoped<IValidator<CalculationRequest>, CalculationRequestValidator>();

// File based audit log
builder.Services.AddSingleton<ICalculationLogger, FileCalculationLogger>();

// Calculator service
builder.Services.AddScoped<ICalculatorService, CalculatorService>();

// CORS — allowed origins read from configuration
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();

app.MapPost("/api/calculate", (
    CalculationRequest request,
    IValidator<CalculationRequest> validator,
    ICalculatorService service) =>
{
    var validation = validator.Validate(request);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var result = service.Calculate(request);
    return Results.Ok(result);
})
.WithName("Calculate");


app.Run();

public partial class Program { }
