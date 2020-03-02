using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JsonOptionsInjection
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MyServiceWithOwnOptions>();
            services.AddTransient<MyServiceOptionsInjected>();

            services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var svc1 = context.RequestServices.GetService<MyServiceWithOwnOptions>();

                    var svc2 = context.RequestServices.GetService<MyServiceOptionsInjected>();

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }

    public class MyServiceWithOwnOptions
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            AllowTrailingCommas = false, 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false
        };
    }

    public class MyServiceOptionsInjected
    {
        private readonly JsonSerializerOptions _options;

        public MyServiceOptionsInjected(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
