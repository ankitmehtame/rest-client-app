using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace RestClientApp
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Starting...");
            try
            {
                var root = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Is(LogEventLevel.Debug)
                    .WriteTo.Console()
                    .WriteTo.File("Logs/RestClientApp-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error configuring logging - " + ex.Message);
                return -1;
            }
            string urlFile = "Url.txt";
            if (args.Length > 0)
            {
                urlFile = args[0];
            }
            string contentFile = "Content.txt";
            if (args.Length > 1)
            {
                contentFile = args[1];
            }
            Task.Factory.StartNew(() => Run(urlFile, contentFile)).Unwrap().Wait();
            return 0;
        }

        private static async Task Run(string urlFile, string contentFile)
        {
            Log.Debug("Application start");
            var client = new HttpClient();
            string contentBody = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(contentFile) && File.Exists(contentFile))
                {
                    contentBody = await File.ReadAllTextAsync(contentFile);
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Exception type is {ex.GetType().FullName} for contentFile {contentFile}");
                Log.Warning($"Error {ex.Message} {ex.InnerException?.Message}");
            }
            var content = new StringContent(contentBody);
            try
            {
                var url = File.ReadAllText(urlFile);
                Log.Debug($"Calling POST {url}");
                var x = await client.PostAsync(url, content);
                Log.Debug($"Status: {x.StatusCode}");
            }
            catch (Exception ex)
            {
                Log.Debug($"Exception type is {ex.GetType().FullName} for urlFile {urlFile}");
                Log.Warning($"Error {ex.Message} {ex.InnerException?.Message}");
            }
            Log.Debug("Application end");
        }
    }
}
