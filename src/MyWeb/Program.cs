using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WindowsService;

namespace MyWeb {
    public class Program {
        public static void Main(string[] args) {
            // CreateWebHostBuilder(args).Build().Run();

            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService) {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);

                var newArgs = args.Where(x => x != "--console").ToArray();
                CreateWebHostBuilder(newArgs).Build().RunAsCustomService();
            } else {
                CreateWebHostBuilder(args).Build().Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
