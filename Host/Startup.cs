using CustomerApi.Contracts.Interfaces;
using CustomerApi.Middlewares;
using CustomerApi.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using CustomerApi.Services.CustomerService;

namespace CustomerApi
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
            // Register in memory data store
            services.AddDbContext<CustomerDbContext>(opt => opt.UseInMemoryDatabase("CustomerDb"));

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(
                options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

            services.AddTransient<ErrorHandlingMiddleware, ErrorHandlingMiddleware>();

            AddSwaggerSupport(services);
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

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }

        private void AddSwaggerSupport(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Customer API",
                        Version = "v1",
                        Description = "A simple customer API that performs CRUD operations on Customer",
                        Contact = new Contact
                        {
                            Name = "Parag Naikade",
                            Email = "paragnaikade@gmail.com",
                            Url = "https://www.linkedin.com/in/parag-naikade/"
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
