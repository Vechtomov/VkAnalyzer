using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkAnalyzer.Interfaces
{
    public interface IUsersRepository
    {
        IEnumerable<long> GetUsers();
        Task<IEnumerable<long>> GetUsersAsync();

        void AddUser(long id);
        Task AddUserAsync(long id);

        // todo: implement this method
        //bool IsUserTracked(long id);
    }
}
