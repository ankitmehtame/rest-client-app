using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace RestClientApp
{
    class Program
    {
        private static ILog Log;

        static int Main(string[] args)
        {
            Console.WriteLine("Starting...");
            try
            {
                var root = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath);
                var logsDir = Path.Combine(root, "Logs");
                Console.WriteLine($"Creating dir {logsDir}");
                Directory.CreateDirectory(logsDir);
                var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(repo, new FileInfo("log4net.config"));
                Log = LogManager.GetLogger(typeof(Program));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error configuring log4net - " + ex.Message);
                return -1;
            }
            string urlFile = "Url.txt";
            if (args.Length > 0)
            {
                urlFile = args[0];
            }
            Task.Factory.StartNew(() => Run(urlFile)).Unwrap().Wait();
            return 0;
        }

        private static async Task Run(string urlFile)
        {
            Log.Debug("Application start");
            var client = new HttpClient();
            var content = new StringContent(string.Empty);
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
                Log.Warn($"Error {ex.Message} {ex.InnerException?.Message}");
            }
            Log.Debug("Application end");
        }
    }
}
