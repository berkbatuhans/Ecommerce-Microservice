using System;
using AutoMapper;
using Consul;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Helpers;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Providers;
using Infrastructure.ServiceDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Api.Customers
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


            ConfigureConsul(services);

            services.AddScoped<ICustomersProvider, CustomersProvider>();
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<CustomersDbContext>(options => {
                options.UseInMemoryDatabase("Customers");
            });
            services.AddControllers();

            //services.AddConsulConfig(Configuration);
            
            
            
        }

        private void ConfigureConsul(IServiceCollection services)
        {
            var serviceConfig = Configuration.GetServiceConfig();
            services.RegisterConsulServices(serviceConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseConsul();
        }
    }
}
