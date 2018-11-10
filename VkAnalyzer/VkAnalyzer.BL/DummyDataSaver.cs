using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

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
