using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VkAnalyzer.BE
{
    public class UserOnlineInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public long Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DateTime { get; set; }

        [BsonRepresentation(BsonType.Binary)]
        public OnlineInfo OnlineInfo { get; set; }
    }

	[BsonIgnoreExtraElements]
	public class MongoUser
	{
		[BsonId]
		public long Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public DateTime? AddedDateTime { get; set; }
		public Guid? AddedUser { get; set; }

		public IEnumerable<MongoOnlineInfo> Info { get; set; }
	}

	public class MongoOnlineInfo
	{
		public DateTime DateTime { get; set; }
		public OnlineInfo OnlineInfo { get; set; }
	}
}
