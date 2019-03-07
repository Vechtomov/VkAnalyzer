using System.Collections.Generic;

namespace VkAnalyzer.Models
{
    public class AddUsersRequest
    {
        public IEnumerable<long> Ids { get; set; }
    }
}
