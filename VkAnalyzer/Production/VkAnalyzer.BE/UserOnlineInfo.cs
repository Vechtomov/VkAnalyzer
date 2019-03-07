using MongoDB.Bson.Serialization.Attributes;
using System;

namespace VkAnalyzer.BE
{
    public class UserOnlineInfo
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public long Id { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime DateTime { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Binary)]
        public OnlineInfo OnlineInfo { get; set; }
    }
}
