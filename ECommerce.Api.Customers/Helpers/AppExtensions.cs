        using System;
        using System.Linq;
        using System.Net;
        using System.Net.Sockets;
        using Consul;
        using Microsoft.AspNetCore.Builder;
        using Microsoft.AspNetCore.Hosting.Server.Features;
        using Microsoft.AspNetCore.Http.Features;
        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.DependencyInjection;
        using Microsoft.Extensions.Hosting;
        using Microsoft.Extensions.Logging;

        namespace ECommerce.Api.Customers.Helpers
        {
            public static class AppExtensions
            {
                //public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
                //{
                //    services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
                //    {
                //        //var address = configuration.GetValue<string>("http://localhost:8500");
                //        var address = configuration["Consul:Host"];
                //        consulConfig.Address = new Uri(address);
                //    }));
                //    return services;
                //}

                public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
                {
                    var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
                    ILogger logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
                    var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();


                    if (!(app.Properties["server.Features"] is FeatureCollection features)) return app;

                    var addresses = features.Get<IServerAddressesFeature>();

                    var address = addresses.Addresses.First();

                    Console.WriteLine($"address={address}");
                    var name = Dns.GetHostName(); // get container id
            
                    var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    //var uri = new Uri(address);
                    var uri = new UriBuilder()
                    {
                        Host = ip.ToString(),
                        Port = 80,
                
                    };
                    var registration = new AgentServiceRegistration()
                    {
                        ID = $"CustomersService-{uri.Port}",
                        Name = "Customers",

                        //Address = "localhost",
                        //Port = 5001,
                        Address = $"{uri.Host}",
                        Port = uri.Port,
                        Check = new AgentServiceCheck()
                        {
                            //HTTP = $"{uri.AbsoluteUri}HealthCheck",
                            HTTP = $"http://localhost/HealthCheck",
                            Interval = TimeSpan.FromSeconds(10),
                        }
                    };

                    logger.LogInformation("Registering with Consul");

                    consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                    consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

                    lifetime.ApplicationStopping.Register(() =>
                    {
                        logger.LogInformation("Unregistering from Consul");
                        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                    });

                    return app;
                }

            
        }

        
           }
