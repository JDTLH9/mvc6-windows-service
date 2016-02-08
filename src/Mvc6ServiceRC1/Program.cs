using System;
using System.ServiceProcess;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Mvc6ServiceRC1
{
    public class Program : ServiceBase
    {
        private IHostingEngine _hostingEngine;
        private IApplication _shutdownServerDisposable;
        private readonly IApplicationEnvironment _applicationEnvironment;

        public Program(IApplicationEnvironment applicationEnvironment)
        {
            _applicationEnvironment = applicationEnvironment;
        }

        public void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                OnStart(null);
                Console.ReadLine();
                OnStop();
                return;
            }

            Run(this); 
        }

        protected override void OnStart(string[] args)
        {
            var configSource = new JsonConfigurationProvider($@"{_applicationEnvironment.ApplicationBasePath}\config.json");

            var config = new ConfigurationBuilder().Add(configSource).Build();
            var builder = new WebHostBuilder(config);
            builder.UseServer("Microsoft.AspNet.Server.Kestrel");
            builder.UseServices(services => services.AddMvc());
            builder.UseStartup(appBuilder =>
            {
                appBuilder.UseStaticFiles();

                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        "Default",
                        "{controller}/{action}",
                        new { controller = "home", action = "index" });
                });
            });

            _hostingEngine = builder.Build();
            _shutdownServerDisposable = _hostingEngine.Start();
        }

        protected override void OnStop()
        {
            _shutdownServerDisposable.Dispose();
        }
    }
}