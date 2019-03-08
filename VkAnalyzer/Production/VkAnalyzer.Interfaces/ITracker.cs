using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkAnalyzer.Interfaces
{
    public interface ITracker
    {
		/// <summary>
		/// Start tracker
		/// </summary>
		/// <returns></returns>
        Task Start();

		/// <summary>
		/// Adding users in tracker
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		bool AddUsers(IEnumerable<long> ids);

		/// <summary>
		/// Remove user from tracker
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        bool RemoveUser(long id);

		/// <summary>
		/// Stop tracker
		/// </summary>
        void Stop();
    }
}
