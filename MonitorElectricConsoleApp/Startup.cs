using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorElectricConsoleApp
{
    public class Startup
    {
        public Startup()
        {
            using (var client = new AppContext())
            {
                client.Database.EnsureCreated();
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Примеры api запросов:");
            sb.AppendLine("https://localhost:8080/api/math/sum?first=4&second=7");
            sb.AppendLine("https://localhost:8080/api/math/div?first=4&second=7");
            sb.AppendLine("https://localhost:8080/api/math/parallelnosafely");
            sb.AppendLine("https://localhost:8080/api/math/parallelsafely");
            Console.WriteLine(sb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHsts();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
