using System;
using VkAnalyzer.BE;

namespace VkAnalyzer.DbModels
{
    public class UserOnlineInfo
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public DateTime DateTime { get; set; }
        public OnlineInfo OnlineInfo { get; set; }
    }
}
