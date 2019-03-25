using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTaskSchedule.DB;
using ApiTaskSchedule.Hubs;
using ApiTaskSchedule.Jobs;
using ApiTaskSchedule.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiTaskSchedule
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200", "http://127.0.0.1:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<TaskSchedulerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IScheduler, Scheduler>(); 
            services.AddTransient<IJobPersister, JobPersister>(); 
            services.AddTransient<StartEngineJob>();
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            services.AddSignalR().AddJsonProtocol(options => {
                options.PayloadSerializerSettings = settings;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            //app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<JobHub>("/jobs");
            });

            app.UseMvc();
        }
    }
}
