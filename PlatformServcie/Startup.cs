using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlatformServcie.AsyncDataServices;
using PlatformServcie.PlatformData;
using PlatformServcie.SyncDataClient.Http;

namespace PlatformServcie
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if(_env.IsProduction()){
                Console.WriteLine("--> using SqlServer DB");
                services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));

            }
            else{
                 Console.WriteLine("--> using InMem DB");
                  services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
             }
           
            services.AddScoped<IPlatformRepo, PlatformRepo>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IMessageBusClient,MessageBusClient>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformServcie", Version = "v1" });
            });

            Console.WriteLine($"Command Service end point {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformServcie v1"));
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
