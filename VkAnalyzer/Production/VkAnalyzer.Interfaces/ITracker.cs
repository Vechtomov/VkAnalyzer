using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkAnalyzer.Interfaces
{
    public interface ITracker
    {
        Task Start();
        bool AddUsers(IEnumerable<long> ids);
        bool RemoveUser(long id);
        void Stop();
    }
}
