using Analysis.Application;
using Analysis.Application.Interface;
using Analysis.Common;
using Analysis.Data.Pom;
using Analysis.Infrastructure;
using Analysis.Infrastructure.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Analysis.Presentation
{
    public class Startup
    {
        private readonly AppSettings appSettings;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.appSettings = Configuration.GetSection(typeof(AppSettings).Name).Get<AppSettings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = appSettings.PomCovid.ResponseCacheDuration
                });
            });

            this.ConfigureModules(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Analysis", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Analysis API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureModules(IServiceCollection services)
        {
            services.AddSingleton(this.appSettings);

            var pomRawStorage = new PomRawStorage(this.appSettings);
            services.AddSingleton(pomRawStorage);
            services.AddSingleton<IExternalStorage, PomCovidExternalStorage>();
            services.AddScoped<PomTotalStorage>();
            services.AddScoped<PomDailyStorage>();

            services.AddScoped<IRawAppService, PomRawAppService>();
            services.AddScoped<PomTotalAppService>();
            services.AddScoped<PomDailyAppService>();
            services.AddScoped<PomTableAppService>();
        }
    }
}
