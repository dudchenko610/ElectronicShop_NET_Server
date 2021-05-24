using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next.Invoke(context);
            }
            catch (ServerException ex)
            {
                await HandleServerExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleServerExceptionAsync(HttpContext context, ServerException exception)
        {


            Console.WriteLine("---------------------- " + exception.Message + " ----------------------");

            foreach (string message in exception.ErrorMessages)
            { 
                Console.WriteLine("_____ " + message + " _____");
            }

            Console.WriteLine(exception.StackTrace);

            var result = JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.Code;

            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            Console.WriteLine("---------------------- " + exception.Message);
            Console.WriteLine(exception.StackTrace);


            var result = JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            return context.Response.WriteAsync(result);
        }
    }
}
