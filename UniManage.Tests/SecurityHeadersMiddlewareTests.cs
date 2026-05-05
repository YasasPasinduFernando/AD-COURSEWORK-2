using AD_COURSEWORK_2.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace UniManage.Tests;

public class SecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_sets_security_headers_on_response()
    {
        var nextInvoked = false;
        Task Next(HttpContext ctx)
        {
            nextInvoked = true;
            return Task.CompletedTask;
        }

        var middleware = new SecurityHeadersMiddleware(Next);
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(httpContext);

        nextInvoked.Should().BeTrue();
        httpContext.Response.Headers["X-Frame-Options"].ToString().Should().Be("DENY");
        httpContext.Response.Headers["X-Content-Type-Options"].ToString().Should().Be("nosniff");
        httpContext.Response.Headers["Referrer-Policy"].ToString().Should().Be("strict-origin-when-cross-origin");
        httpContext.Response.Headers["Content-Security-Policy"].ToString().Should().Contain("default-src 'self'");
    }
}
