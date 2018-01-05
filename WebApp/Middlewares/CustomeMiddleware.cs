using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Middlewares
{
    public class CustomeMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($"---------------------> Requested ask for {httpContext.Request.Path}");

            await _next.Invoke(httpContext);
        }
    }
}
