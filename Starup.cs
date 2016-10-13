using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection service)
        {
            service.AddMvc();
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            //configure pipeline
            app.UseMiddleware<MyLoggingMiddleWare>();
            app.UseStatusCodePagesWithRedirects("/{0}.html");
            app.Map("/mvc", mvcApp => { mvcApp.UseMvcWithDefaultRoute(); });

            //afEnd of the pipeline
            app.Run(c => c.Response.WriteAsync("LivingIT web End of Pipeline"));
            

        }
    }

    public class MyLoggingMiddleWare
    {
        private RequestDelegate _next;

        public MyLoggingMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Begin Reqeust");
            //REQUEST
            await _next.Invoke(context);
            //RESPONSE
            Console.WriteLine("End Request");
        }
    }

}