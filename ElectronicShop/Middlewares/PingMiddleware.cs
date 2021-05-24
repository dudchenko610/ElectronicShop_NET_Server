using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Middlewares
{
    public class PingMiddleware
    {
        private RequestDelegate _next;

        public PingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
          //  Thread.Sleep(300);

            await _next.Invoke(context);
        }

    }
}
