using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VkAnalyzer.BL
{
    public class Interrogator
    {
        // Кол-во попыток
        const int numberOfAttempts = 3;

        private IUserInfoSource userInfoSource;
        private IDataSaver dataSaver;
        private List<long> usersQueue = new List<long>();
        public bool IsWorking { get; set; }

        public Interrogator(IUserInfoSource userInfoSource, IDataSaver dataSaver)
        {
            this.userInfoSource = userInfoSource;
            this.dataSaver = dataSaver;
        }

        public async Task Start(CancellationTokenSource cts = null)
        {
            var index = 0;
            while (true)
            {
                if (usersQueue.Count == 0 || (cts != null && cts.IsCancellationRequested))
                {
                    IsWorking = false;
                    return;
                }

                IsWorking = true;

                if (index == usersQueue.Count - 1)
                {
                    index = 0;
                }

                var user = usersQueue[index++];

                IEnumerable<UserOnlineInfo> infos;
                    infos = await userInfoSource.GetOnlineInfo(usersQueue);

                await dataSaver.SaveData(infos);
                await Task.Delay(100);
            }
        }

        public void AddUser(long userId)
        {
            usersQueue.Add(userId);
        }

        public void DeleteUser(long userId)
        {
            usersQueue.Remove(userId);
        }
    }
}
