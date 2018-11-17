using System;
using System.Collections.Generic;

namespace VkAnalyzer.BE
{
    public class UserOnlineData
    {
        public long Id { get; set; }
        public IEnumerable<DateOnline> OnlineInfos { get; set; }
    }

    public struct DateOnline
    {
        public DateTime Date { get; set; }
        public OnlineInfo OnlineInfo { get; set; }
    }
}
