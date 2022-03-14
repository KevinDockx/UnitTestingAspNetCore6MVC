using EmployeeManagement.Middleware;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.Test
{
    public class EmployeeManagementSecurityHeadersMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            RequestDelegate next = (HttpContext httpContext) => Task.CompletedTask;

            var middleware = new EmployeeManagementSecurityHeadersMiddleware(next);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            var cspHeader = httpContext.Response.Headers["Content-Security-Policy"].ToString();
            var xContentTypeOptionsHeader = httpContext.Response.Headers["X-Content-Type-Options"].ToString();
            
            Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
            Assert.Equal("nosniff", xContentTypeOptionsHeader); 
         }
    }
}
