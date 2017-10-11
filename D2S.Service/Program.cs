using System;
using System.Threading;
using System.Threading.Tasks;
using D2S.Core;
using Microsoft.Extensions.Logging;

namespace Dota2Stat
{
    public class Program
    {
        private static readonly ILogger Log = ApplicationLogging.CreateLogger<Program>();

        static Program()
        {
            ApplicationLogging.LoggerFactory.AddConsole(false);
        }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            Log.LogInformation("Starting services...");
            Task.Run(async () => { await new DataService().Start(token); });
            Log.LogInformation("Services started.");
            Console.WriteLine("Type 'exit' to shutdown...");
            while (Console.ReadLine().ToLowerInvariant() != "exit")
            {
                Console.WriteLine("Type 'exit' to shutdown...");
            };
            Console.WriteLine("Goodbye!");
        }
    }
}