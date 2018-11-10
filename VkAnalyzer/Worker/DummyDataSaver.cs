using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkAnalyzer.BL
{
    public class DummyDataSaver : IDataSaver
    {
        public Task SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            throw new NotImplementedException();
        }
    }
}
