using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Middlewares
{
    public class LogMiddleware
    {
        private RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine(context.Request.Path);


            Console.WriteLine("__________________ Request __________________");
            foreach (var e in context.Request.Headers)
            {
                Console.WriteLine("[] " + e.Key + "   :   " + e.Value);
            }

            


            Console.WriteLine("__________________ Response __________________");
            foreach (var e in context.Response.Headers)
            {
                Console.WriteLine("[] " + e.Key + "   :   " + e.Value);
            }

            await _next.Invoke(context);
        }

    }
}
