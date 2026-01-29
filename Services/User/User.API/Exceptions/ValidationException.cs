using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Exceptions
{
    public class ValidationException : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            if (exception is not FluentValidation.ValidationException validationException)
                return false;

            var problemDetails = new ProblemDetails
            {
                Title = "One or more validation errors occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            var errors = validationException.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            problemDetails.Extensions.Add("errors", errors);

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken); 
            
            return true; 
        }
    }
}
