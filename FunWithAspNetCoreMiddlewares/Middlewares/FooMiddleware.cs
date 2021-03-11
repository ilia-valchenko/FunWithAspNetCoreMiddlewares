using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FunWithAspNetCoreMiddlewares.Middlewares
{
    // Custom middleware has to have the following thins:
    // * private delegate DelegateRequest which will contain a reference to the next middleware.
    // * public constructor with DelegateRequest parameter.
    // * The handle method name has to start with Invoke or InvokeAsync phrase.

    public class FooMiddleware
    {
        // This delegate contains a reference to the next middleware.
        private readonly RequestDelegate next;

        public FooMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string method = context.Request.Method;

            if (method == "GET")
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("That is GET request!");
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}