using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using VkAnalyzer.Interfaces;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL
{
    public class Tracker : ITracker
    {
        private const int UsersCountPerTime = 1000;
        private const int TimerPeriod = 20 * 1000; // 20 секунд

        private readonly IUserInfoSource _userInfoSource;
        private readonly IUserInfoRepository _userRepository;
        private readonly List<long> _usersQueue;
        private readonly Dictionary<long, OnlineInfo> _lastInfo = new Dictionary<long, OnlineInfo>();
        private Timer _timer;

        public Tracker(IUserInfoSource userInfoSource, IUserInfoRepository userRepository, List<long> users = null)
        {
            _userInfoSource = userInfoSource ?? throw new ArgumentNullException(nameof(userInfoSource));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _usersQueue = users ?? new List<long>();
        }

        public Task Start()
        {
            _timer = new Timer(UpdateInfo, null, 0, TimerPeriod);
            return Task.CompletedTask;
        }

        private async void UpdateInfo(object state)
        {
            var index = 0;
            if (_usersQueue.Count == 0)
            {
                return;
            }

            var users = _usersQueue.ToList();
            var infos = new List<UserOnlineInfo>();

            while (true)
            {
                var from = index * UsersCountPerTime;

                if (from > _usersQueue.Count - 1)
                {
                    break;
                }

                var currentUsers = users.Skip(from).Take(UsersCountPerTime);

                var userInfos = (await _userInfoSource.GetOnlineInfo(currentUsers)).ToList();
                var updateList = new List<UserOnlineInfo>();

                foreach (var info in userInfos)
                {
                    if (!IsLastInfoDifferent(info)) continue;

                    _lastInfo[info.Id] = info.OnlineInfo;
                    updateList.Add(info);
                }

                infos.AddRange(updateList);

                index++;
            }

            await _userRepository.SaveDataAsync(infos);
        }

        private bool IsLastInfoDifferent(UserOnlineInfo info)
        {
            return !_lastInfo.ContainsKey(info.Id) || _lastInfo[info.Id] != info.OnlineInfo;
        }

        public bool AddUsers(IEnumerable<long> userIds)
        {
            var isFailed = true;

            foreach (var userId in userIds)
            {
                if (_usersQueue.Contains(userId))
                    continue;

                isFailed = false;
                _usersQueue.Add(userId);
            }

            return !isFailed;
        }

        public bool RemoveUser(long userId)
        {
            return _usersQueue.Remove(userId);
        }

        public void Stop()
        {
            _timer.Dispose();
        }
    }
}
