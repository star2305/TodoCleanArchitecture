using System.Text.Json;
using TodoCleanArchitecture.Application.Interfaces;

namespace TodoCleanArchitecture.WebApi.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuditLogWriter audit)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var username = context.User?.Identity?.Name;
                var traceId = context.TraceIdentifier;

                var data = new
                {
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString.Value
                };

                await audit.WriteAsync(
                    level: "Error",
                    category: "Exception",
                    action: "Unhandled",
                    message: "Unhandled exception occurred",
                    username: username,
                    traceId: traceId,
                    dataJson: JsonSerializer.Serialize(data),
                    exception: ex.ToString()
                );

                // 사용자에게는 깔끔한 에러 응답
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message = "An unexpected error occurred.",
                    traceId
                }));
            }
        }
    }
}
