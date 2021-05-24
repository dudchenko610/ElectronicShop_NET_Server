using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Middlewares
{
    public class SignalRAuthMiddleware
    {
        private RequestDelegate _next;

        public SignalRAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var qs = context.Request.QueryString;

            if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]) && qs.HasValue)
            {
                var token = (from pair in qs.Value.TrimStart('?').Split('&')
                             where pair.StartsWith("token=")
                             select pair.Substring(6)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }

            await _next.Invoke(context);
        }

    }
}
