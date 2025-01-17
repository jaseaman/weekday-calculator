using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WeekdayCalculator.Core.Services.Dates;
using WeekdayCalculator.Infrastructure.Repository;
using WeekdayCalculator.Infrastructure.Repository.EntityFramework;
using WeekdayCalculator.Infrastructure.Services.Dates;

namespace WeekdayCalculator.Api
{
    public class Startup
    {
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath($"{environment.ContentRootPath}/Config/")
                .AddJsonFile($"Secrets/Secrets.{environment.EnvironmentName.ToLower()}.json", false);

            _environment = environment;
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString =
                $@"User ID={_configuration["Database:Username"]}; Password={_configuration["Database:Password"]}; 
                Host={_configuration["Database:Endpoint"]}; Port={_configuration["Database:Port"]};
                Database={_configuration["Database:Name"]}; Pooling=true;";
            services.AddTransient<IDateService, DateService>();
            services.AddDbContext<WeekdayCalculatorDbContext>(o => { o.UseNpgsql(connectionString); });
            services.AddScoped<DbContext>(s => s.GetService<WeekdayCalculatorDbContext>());
            services.AddTransient(typeof(IReadRepository<,>), typeof(EntityFrameworkRepository<,>));
            
            services.AddMvcCore(options => options.EnableEndpointRouting = false)
                .AddApiExplorer();

            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1",
                    new OpenApiInfo {Title = Assembly.GetExecutingAssembly().GetName().Name, Version = "v1"});
                var docFilePath = Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                o.IncludeXmlComments(docFilePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger(o => { o.RouteTemplate = $"/swagger/{{documentName}}/swagger.json"; });
            app.UseSwaggerUI(o => { o.SwaggerEndpoint($"/swagger/v1/swagger.json", "WeekdayCalculator"); });
        }
    }
}