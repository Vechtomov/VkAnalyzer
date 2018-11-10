using System.Collections.Generic;
using VkAnalyzer.BE;

namespace VkAnalyzer.Models
{
    public class UsersResponse
    {
        public int TotalCount { get; set; }
        public int Count { get; set; }
        public int Offset { get; set; }
        public IEnumerable<UserInfo> Users { get; set; }
    }
}
