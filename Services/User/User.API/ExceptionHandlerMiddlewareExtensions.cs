namespace User.API
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middlewares.ExceptionHandlerMiddleware>();
        }
    }
}
