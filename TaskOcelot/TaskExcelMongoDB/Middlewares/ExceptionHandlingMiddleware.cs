using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskExcelMongoDB.Exceptions; // Ensure this is added

namespace TaskExcelMongoDB.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            // Add case for InvalidUserDataException
            var response = exception switch
            {
                InvalidUserDataException => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
                ApplicationException => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred"),
                KeyNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, "The requested key was not found."),
                UnauthorizedAccessException => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized access"),
                ArgumentException => new ExceptionResponse(HttpStatusCode.BadRequest, "User data contains empty or null fields."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please try again")
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)response.StatusCode;
            var jsonResponse = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }

    public class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public ExceptionResponse(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
