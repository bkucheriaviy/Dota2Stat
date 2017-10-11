using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dota2Stat
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            Console.WriteLine("Starting services...");
            Task.Run(async () => { await new DataService().Start(token); });
            
        }
    }
}