using System.Threading;
using System.Threading.Tasks;

namespace D2S.Service
{
    public interface IService
    {
        Task Start(CancellationToken ct);
    }
}