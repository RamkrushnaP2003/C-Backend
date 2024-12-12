// using Ocelot.DependencyInjection;
// using Ocelot.Middleware;
// using Microsoft.Extensions.Configuration;

// // Environment.SetEnvironmentVariable("ENVIRONMENT_ASPNETCORE", "Development");
// Environment.SetEnvironmentVariable("ENVIRONMENT_ASPNETCORE", "Production");

// var builder = WebApplication.CreateBuilder(args);

// // Debugging - Output paths and file names
// Console.WriteLine(builder.Environment.ContentRootPath);
// Console.WriteLine(builder.Environment.EnvironmentName.ToLower());
// Console.WriteLine($@"{builder.Environment.ContentRootPath}/{builder.Environment.EnvironmentName}");
// Console.WriteLine($@"{builder.Environment.ContentRootPath}/ocelot.{builder.Environment.EnvironmentName.ToLower()}", builder.Environment);
// Console.WriteLine($"appsettings.{builder.Environment.EnvironmentName}.json");

// // Set the base path for configuration files and load them
// builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//                      .AddOcelot($@"{builder.Environment.ContentRootPath}/ocelot.{builder.Environment.EnvironmentName.ToLower()}", builder.Environment)
//                      .AddEnvironmentVariables();

// // Register Ocelot services only once
// builder.Services.AddOcelot(builder.Configuration);

// var app = builder.Build();

// // Use Ocelot middleware
// await app.UseOcelot();

// app.Run();


using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Configuration;

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine(builder.Environment.ContentRootPath);
Console.WriteLine(builder.Environment.EnvironmentName.ToLower());
Console.WriteLine($@"{builder.Environment.ContentRootPath}/{builder.Environment.EnvironmentName}");
Console.WriteLine($@"{builder.Environment.ContentRootPath}/ocelot.{builder.Environment.EnvironmentName.ToLower()}.json");
Console.WriteLine($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddOcelot($@"{builder.Environment.ContentRootPath}/ocelot.{builder.Environment.EnvironmentName.ToLower()}", builder.Environment)
                     .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
