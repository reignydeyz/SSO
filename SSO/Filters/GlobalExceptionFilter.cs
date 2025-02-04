using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace SSO.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Handle the exception using the custom method
            HandleExceptionAsync(context.HttpContext, context.Exception);

            context.ExceptionHandled = true; // Mark the exception as handled
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case ArgumentNullException:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Resource not found.";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                case ArgumentException:
                    statusCode = ((ArgumentException)exception).ParamName == "Forbidden"
                        ? HttpStatusCode.Forbidden
                        : HttpStatusCode.BadRequest;

                    message = exception.Message;
                    break;
                case DbUpdateException:
                    statusCode = HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;
                case InvalidOperationException:
                    statusCode = exception.Message.Contains("OTP") ? HttpStatusCode.Accepted : HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            var response = new { error = message };
            var responseJson = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            context.Response.WriteAsync(responseJson);
        }
    }
}
