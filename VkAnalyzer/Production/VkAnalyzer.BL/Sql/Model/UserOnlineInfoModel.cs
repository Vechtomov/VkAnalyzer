using System;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL.Sql.Model
{
    public class UserOnlineInfoModel
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime DateTime { get; set; }
        public OnlineInfo OnlineInfo { get; set; }
    }
}
