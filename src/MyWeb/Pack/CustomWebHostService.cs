using System;
using System.Diagnostics;
using System.ServiceProcess;
using FluentScheduler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WindowsService {
    internal class CustomWebHostService : WebHostService {
        private ILogger logger;

        public CustomWebHostService(IWebHost host) : base(host) {
            logger = host.Services
                .GetRequiredService<ILogger<CustomWebHostService>>();
        }

        private void RestartWindowsService(string serviceName) {
            ServiceController serviceController = new ServiceController(serviceName);
            try {
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending))) {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
            } catch (Exception ex) {
                using (EventLog eventLog = new EventLog("MyWeb")) {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Information, 101, 1);
                }
            }
        }

        protected override void OnStarting(string[] args) {
            logger.LogInformation("OnStarting method called.");

            var registr = new Registry();
            registr
                .Schedule(() => {
                    RestartWindowsService("BCircle.MyWeb.exe");
                })
                .ToRunEvery(50).Seconds();

            JobManager.Initialize(registr);

            base.OnStarting(args);
        }

        protected override void OnStarted() {
            logger.LogInformation("OnStarted method called.");
            base.OnStarted();
        }

        protected override void OnStopping() {
            logger.LogInformation("OnStopping method called.");
            base.OnStopping();
        }
    }
}
