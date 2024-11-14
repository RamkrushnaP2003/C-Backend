using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Middleware;
using TaskExcelMongoDB.Repositories.Implementations;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddScoped<IStoreExcelService, StoreExcelService>();
builder.Services.AddScoped<IStoreExcelRepository, StoreExcelRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

var app = builder.Build();

// Use custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Register custom middleware

// Configure other middlewares
app.UseAuthorization();
app.MapControllers();

app.Run();
