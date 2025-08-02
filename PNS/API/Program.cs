// File Path: API/Program.cs
using Application;
using Infrastructure;
using Persistence;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Application.Profiles;
using AutoMapper;
using System.Reflection;
using MediatR;
using Application.CQRS.Notifications.Handlers;
using Application.Interfaces; // <<<< ይህንን ጨምር! >>>>

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

// ********************************************************************************
// PnsDbContext ን መመዝገብ
// ********************************************************************************
// PnsDbContext ሲጠየቅ IApplicationDbContext ን እንዲሰጥ መመዝገብ <<<< ይህንን ክፍል ጨምር >>>>
builder.Services.AddScoped<IApplicationDbContext, PnsDbContext>();
// ********************************************************************************

builder.Services.AddControllers();

// ********************************************************************************
// MediatR Configuration - ይህንን ክፍል አክዬበታለሁ!
// ********************************************************************************
// MediatR ን ይመዘግባል እና handlers ያሉበትን assembly እንዲያገኝ ያግዘዋል።
// CreateNotificationCommandHandler ያለው በ "Application" project ውስጥ ስለሆነ፣
// የዚያን project's assembly እንሰጠዋለን።
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateNotificationCommandHandler).Assembly));
// ********************************************************************************

// Configure API Versioning
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

// Configure Swagger/OpenAPI for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification API", Version = "v1" });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authentication using the 'X-API-KEY' header",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { key, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsBuilder => corsBuilder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
});

// --- Build the Application ---
var app = builder.Build();

// --- Configure the HTTP Request Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API v1"));
}
else
{
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();