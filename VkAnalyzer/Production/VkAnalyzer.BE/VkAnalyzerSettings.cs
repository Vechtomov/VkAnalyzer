using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkAnalyzer
{
    public class VkAnalyzerSettings
    {
        public string FileDataPath { get; set; }
        public string VkUserLogin { get; set; }
        public string VkUserPassword { get; set; }
        public uint AppId { get; set; }
    }
}
