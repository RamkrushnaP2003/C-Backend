using Microsoft.Extensions.DependencyInjection;
using TaskExcelMongoDB.Data;
using TaskExcelMongoDB.Repositories.Implementations;
using TaskExcelMongoDB.Repositories.Interfaces;
using TaskExcelMongoDB.Services.Implementations;
using TaskExcelMongoDB.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
builder.Services.AddScoped<IStoreExcelService, StoreExcelService>();
builder.Services.AddScoped<IStoreExcelRepository, StoreExcelRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
