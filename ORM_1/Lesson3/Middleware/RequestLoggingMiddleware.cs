using Microsoft.AspNetCore.Http;

namespace Lesson3.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path;

            if (!path.StartsWith("/lib") &&
                !path.StartsWith("/css") &&
                !path.StartsWith("/js") &&
                !path.Contains("."))
            {
                Console.WriteLine(
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Method: {context.Request.Method} - Path: {path}"
                );
            }

            if (path.StartsWith("/Book/Detail/"))
            {
                string idText = path.Replace("/Book/Detail/", "");

                if (int.TryParse(idText, out int id))
                {
                    if (id <= 0)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "text/plain;charset=utf-8";
                        Console.WriteLine($"Status Code: {context.Response.StatusCode}");

                        await context.Response.WriteAsync("Book id không hợp lệ");

                        return;
                    }
                }
            }

            await _next(context);

            if (!path.StartsWith("/lib") &&
                !path.StartsWith("/css") &&
                !path.StartsWith("/js") &&
                !path.Contains("."))
            {
                Console.WriteLine($"Status Code: {context.Response.StatusCode}");
            }
        }
        public class CheckDatabaseMiddleware
        {
            private readonly RequestDelegate _next;

            public CheckDatabaseMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                bool dbConnected = true; // giả lập

                context.Items["DbStatus"] = dbConnected;

                await _next(context);
            }
        }
        public class DatabaseLogMiddleware
        {
            private readonly RequestDelegate _next;

            public DatabaseLogMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                bool dbStatus = (bool)context.Items["DbStatus"];

                Console.WriteLine($"Database Connected: {dbStatus}");

                await _next(context);
            }
        }
    }
}