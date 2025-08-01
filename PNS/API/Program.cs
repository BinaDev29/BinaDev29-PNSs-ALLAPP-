// File Path: API/Program.cs
using Application;
using Infrastructure;
using Persistence;
using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Application.Profiles; // ይህንን ጨምረሃል!

// *** ለ AutoMapper አዲስ using directive ***
using AutoMapper;
using System.Reflection; // ለ Assembly.GetExecutingAssembly()

// *** እዚህ ጋር ነው ማስተካከያው - ይህ መስመር አንድ ጊዜ ብቻ ነው መሆን ያለበት! ***
var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

// AutoMapper profiles በአብዛኛው Application project ውስጥ ስለሚገኙ፣ የ Application assembly ን መመዝገብ አለብህ።
// MappingProfile አሁን ስለሚታወቅ typeof(MappingProfile).Assembly የሚለውን እንጠቀማለን።
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// *** የተደጋገሙትን AddAutoMapper() calls አስወግድ ***
// ሌሎች AutoMapper profile ዎችህ የሚገኙበትን assembly(ies) መጨመር ሊያስፈልግህ ይችላል።
// ለምሳሌ፣ Application layer ውስጥ profile ዎችህ ካሉ፣ እንዲህ ማከል ትችላለህ:
// builder.Services.AddAutoMapper(typeof(Application.MappingProfile).Assembly); // ይህ ከላይ ባለው ተሸፍኗል
// ወይም የሁሉም Application assemblies ውስጥ ያሉ profile ዎችን ለመመዝገብ:
// builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ይህ ብዙ ጊዜ አያስፈልግም እና ቀርፋፋ ሊሆን ይችላል

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();

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