using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using VkAnalyzer.Interfaces;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL
{
    public class Interrogator : IInterrogator
    {
        // Кол-во попыток
        const int numberOfAttempts = 3;
        const int usersCountPerTime = 100;
        const int timerPeriod = 20 * 1000; // 30 секунд

        private IUserInfoSource userInfoSource;
        private IUserInfoRepository userRepository;
        private List<long> usersQueue;
        private Timer timer;

        public Interrogator(IUserInfoSource userInfoSource, IUserInfoRepository userRepository, List<long> users = null)
        {
            this.userInfoSource = userInfoSource;
            this.userRepository = userRepository;
            usersQueue = users ?? new List<long>();
        }

        public Task Start()
        {
            timer = new Timer(UpdateInfo, null, 0, timerPeriod);
            return Task.CompletedTask;
        }

        private async void UpdateInfo(object state)
        {
            var index = 0;
            if (usersQueue.Count == 0)
            {
                return;
            }

            var users = usersQueue.ToList();
            var infos = new List<UserOnlineInfo>();

            while (true)
            {
                var from = index * usersCountPerTime;

                if (from > usersQueue.Count - 1)
                {
                    break;
                }

                var part = await userInfoSource.GetOnlineInfo(users.Skip(from).Take(usersCountPerTime));
                infos.AddRange(part);

                index++;
            }

            await userRepository.SaveDataAsync(infos);
        }

        public bool AddUsers(IEnumerable<long> userIds)
        {
            var isFailed = true;

            foreach (var userId in userIds)
            {
                if (usersQueue.Contains(userId))
                    continue;

                isFailed = false;
                usersQueue.Add(userId);
            }

            return !isFailed;
        }

        public bool RemoveUser(long userId)
        {
            return usersQueue.Remove(userId);
        }

        public void Stop()
        {
            timer.Dispose();
        }
    }
}
