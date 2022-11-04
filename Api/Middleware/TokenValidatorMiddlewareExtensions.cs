namespace Api.Middleware
{
    public static class TokenValidatorMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidator(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidatorMiddleware>();
        }
    }
}
