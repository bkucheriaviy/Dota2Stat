using System;
using System.Threading;
using System.Threading.Tasks;
using D2S.Service;

namespace Dota2Stat
{
    public class DataService : IDataService
    {
        private const string NAS_PATH_BASE = @"//MyCloud";
        private const string DOTA2_EXTRACTION_PATH= @"Projects/Shared/Dota2Stat";
        private const string ITEMS = @"items.dvf";
        
        public async Task Start(CancellationToken ct)
        {
            while (ct.IsCancellationRequested)
            {
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}