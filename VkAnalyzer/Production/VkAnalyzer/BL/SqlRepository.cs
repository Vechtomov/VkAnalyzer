using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class SqlRepository : IUserInfoRepository, IUsersRepository
    {
        private readonly UsersDbContext _dbContext;

        public SqlRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            _dbContext.UserOnlineInfos.AddRange(infos.Select(info => new DbModels.UserOnlineInfo()
            {
                OnlineInfo = info.OnlineInfo,
                DateTime = info.DateTime,
                UserId = info.Id
            }));
            _dbContext.SaveChanges();
        }

        public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            await _dbContext.UserOnlineInfos.AddRangeAsync(infos.Select(info => new DbModels.UserOnlineInfo()
            {
                OnlineInfo = info.OnlineInfo,
                DateTime = info.DateTime,
                UserId = info.Id
            }));
            await _dbContext.SaveChangesAsync();
        }

        public UserOnlineData ReadData(long id, DateTime from, DateTime to)
        {
            return new UserOnlineData
            {
                Id = id,
                OnlineInfos = _dbContext.UserOnlineInfos
                    .Where(info => info.UserId == id && info.DateTime >= from && info.DateTime <= to)
                    .Select(info => new DateOnline
                    {
                        OnlineInfo = info.OnlineInfo,
                        Date = info.DateTime,
                    })
            };
        }

        public Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to)
        {
            return Task.FromResult(new UserOnlineData
            {
                Id = id,
                OnlineInfos = _dbContext.UserOnlineInfos
                    .Where(info => info.UserId == id && info.DateTime >= from && info.DateTime <= to)
                    .Select(info => new DateOnline
                    {
                        OnlineInfo = info.OnlineInfo,
                        Date = info.DateTime,
                    })
            });
        }

        public IEnumerable<long> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<long>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public void AddUser(long id)
        {
            throw new NotImplementedException();
        }

        public Task AddUserAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
