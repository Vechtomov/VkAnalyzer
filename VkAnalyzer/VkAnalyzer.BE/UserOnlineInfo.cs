﻿using System;

namespace VkAnalyzer.BE
{
    public class UserOnlineInfo
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public OnlineInfo OnlineInfo { get; set; }
    }
}
