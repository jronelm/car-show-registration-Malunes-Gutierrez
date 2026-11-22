using System.Text.Json.Serialization;
using Api.Features.Registrations;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Registrations.Common;
using Application.Features.Registrations.CreateRegistration;
using Domain.Common;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Application.Features.Registrations.GetRegistrations;
using Application.Features.Registrations.GetRegistrationById;
using Application.Features.Registrations.GetRegistrationSummary;

var builder = WebApplication.CreateBuilder(args);

// Database (SQLite file next to the Api project)
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "carshow.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// Validators
builder.Services.AddValidatorsFromAssembly(typeof(IAppDbContext).Assembly);

// Handlers
builder.Services.AddScoped<
    ICommandHandler<CreateRegistrationCommand, Result<RegistrationResponse>>,
    CreateRegistrationCommandHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>>,
    GetRegistrationsQueryHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetRegistrationByIdQuery, Result<RegistrationResponse>>,
    GetRegistrationByIdQueryHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>>,
    GetRegistrationSummaryQueryHandler>();

// JSON: enums are sent and received as strings ("Competitor", not 1)
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// CORS: allow the React dev server
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

// API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Car Show Registration API";
        document.Info.Description = "Car Show 2027 registration backend (Vertical Slice Architecture)";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Apply migrations (seed data will be added later)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Root endpoint
app.MapGet("/", () => "Welcome to the Car Show Registration API! Go to /scalar/v1 for API documentation.");

// Map endpoints
app.MapRegistrationEndpoints();

app.Run();