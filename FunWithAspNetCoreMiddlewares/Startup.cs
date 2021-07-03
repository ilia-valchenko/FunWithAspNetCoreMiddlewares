using FunWithAspNetCoreMiddlewares.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunWithAspNetCoreMiddlewares
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        // IServiceCollection is a collection of services which have been already registered.
        // Here we can register additional services (e.g. MVC architecture).
        public void ConfigureServices(IServiceCollection services)
        {
            // The method below brings MVC architecture to our ASP .NET Core application.
            //services.AddMvc();
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        // The IApplicationBuilder is responsible for handling incomming HTTP requests.
        // IWebHostEnvironment and ILoggerFactory are optional parameters.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // The code below works for ASP .NET Core 2.2.
            // For ASP .NET Core 3.1 we configure logging in the Program.
            //loggerFactory.AddCondole();

            var logger = loggerFactory.CreateLogger("fakeCategoryName");
            logger.LogInformation("Some info");

            // IMPORTANT!
            // HTTP request comes to the Configure method.
            // It will be handled by different middlewares.
            // As you see the request will be handled by the Authentication middliware first of all.
            // Then it will be handled by the UseStaticFiles middleware.

            // Use cookie authentication.
            // TODO: Why it's cookie authentication?
            app.UseAuthentication();

            app.UseStaticFiles();

            // NOTE: We have a list of predefined middlewares
            // * app.UseAuthentication() - Provides an authentication support.
            // * app.UseResponseCaching() - Allows us to caches responses.
            // * app.UseResponseCompression() - Allows us to compress responses before sending them back to clients.
            // * app.UseRouting() - Defines routes.
            // * app.UseStaticFiles() - Allow us to store static files on a server (e.g. JS, CSS, etc).

            app.UseMiddleware<FooMiddleware>();

            int counter = 0;

            // IMPORTANT!
            // Middleware is created only once on the application startup.
            // It means that the counter value will be incremented each time when a request comes.

            // The Run method accepts the instance of the RequestDelegate type.
            app.Run(async (HttpContext context) =>
            {
                counter++;
                await context.Response.WriteAsync($"Hello World! Counter: {counter}");
            });

            // ****************** HOW IT WORKS UNDER THE HOOD ******************
            // public delegate Task RequestDelegate(HttpContext context)

            //public static void Run(this IApplicationBuilder app, RequestDelegate handler)
            //{
            //    if (app == null)
            //    {
            //        throw new ArgumentNullException(...);
            //    }

            //    if (handler == null)
            //    {
            //        throw new ArgumentNullException(...);
            //    }

            //    app.Use(_ => handler);
            //}

            //public static IApplicationBuilder Use(this IApplicationBuilder app, Func<RequestDelegate, RequestDelegate> middleware)
            //{
            //    // What is the _components?
            //    // private readonly IList<Func<RequestDelegate, RequestDelegate>> _components = new List(...);

            //    _components.Add(middleware);
            //    return this;
            //}

            // *****************************************************************

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}