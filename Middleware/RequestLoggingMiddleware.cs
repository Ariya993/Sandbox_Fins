using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;
        var userIP = context.Connection.RemoteIpAddress?.ToString();
        var timestamp = DateTime.UtcNow;

        _logger.LogInformation($"Request: {method} {path} from {userIP}");

        // Simpan log ke database
        using (var scope = context.RequestServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var logEntry = new RequestLog
            {
                Method = method,
                Path = path,
                UserIP = userIP,
                Timestamp = timestamp
            };

            //dbContext.RequestLogs.Add(logEntry);
            //await dbContext.SaveChangesAsync();
        }

        await _next(context);
    }
}
