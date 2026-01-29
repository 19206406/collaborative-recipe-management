using BuildingBlocks.Exceptions;
using BuildingBlocks.Exceptions.Common;
using BuildingBlocks.Models;
using System.Text.Json;

namespace User.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            } 
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); 
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var problemDetails = exception switch
            {
                NotFoundException notFoundException => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Resource not found",
                    Status = StatusCodes.Status404NotFound,
                    Errors = new List<string> { notFoundException.Message }
                },

                BadRequestException badRequestException => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Bad request", 
                    Status = StatusCodes.Status400BadRequest, 
                    Instance = context.Request.Path, 
                    Errors = new List<string> { badRequestException.Message }
                }, 

                UnauthorizedAccessException unauthorizedException => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Unauthorized access.",
                    Status = StatusCodes.Status401Unauthorized,
                    Instance = context.Request.Path,
                    Errors = new List<string> { unauthorizedException.Message }
                },

                ForbiddenException forbiddenException => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Title = "Forbidden.",
                    Status = StatusCodes.Status403Forbidden,
                    Instance = context.Request.Path,
                    Errors = new List<string> { forbiddenException.Message }
                },

                ConflictException conflictException => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8", 
                    Title = "Conflict", 
                    Status = StatusCodes.Status409Conflict, 
                    Instance = context.Request.Path,
                    Errors = new List<string> { conflictException.Message }
                },

                BaseException baseException => new ProblemDetailsResponse
                {
                    Type = GetTypeForStatusCode(MapExceptionToStatusCode(baseException)),
                    Title = baseException.Code,
                    Status = MapExceptionToStatusCode(baseException),
                    Instance = context.Request.Path,
                    Errors = new List<string> { baseException.Message }
                },

                _ => new ProblemDetailsResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "An error occurred while processing your request.",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                    Errors = new List<string>
                    {
                        "Ha ocurrido un error interno en el servidor"
                    }
                }
            };

            context.Response.StatusCode = problemDetails.Status;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(problemDetails, options);
            await context.Response.WriteAsync(json);
        }

        private static int MapExceptionToStatusCode(BaseException exception)
        {
            return exception.Code switch
            {
                "NotFound" => StatusCodes.Status404NotFound,
                "BadRequest" => StatusCodes.Status400BadRequest,
                "Unauthorized" => StatusCodes.Status401Unauthorized,
                "Forbidden" => StatusCodes.Status403Forbidden,
                "Conflict" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            }; 
        }

        private static string GetTypeForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                401 => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                403 => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                404 => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                409 => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        }
    }
}
