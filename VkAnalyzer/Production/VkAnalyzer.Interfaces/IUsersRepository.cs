using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IUsersRepository
    {
        IEnumerable<User> GetUsers();
        Task<IEnumerable<User>> GetUsersAsync();

        void AddUser(User id);
        Task AddUserAsync(User id);

        // todo: implement this method
        //bool IsUserTracked(long id);
    }
}
