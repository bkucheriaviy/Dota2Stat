using System;
using System.Threading.Tasks;

namespace Dota2Stat
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting service...");
            Task.Run(() => { new DataService().Start(); });
            Console.WriteLine("Data service started.");
        }
    }
}
