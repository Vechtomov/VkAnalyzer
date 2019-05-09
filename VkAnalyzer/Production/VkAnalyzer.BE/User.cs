using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkAnalyzer.BE
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Photo { get; set; }
		public string ScreenName { get; set; }
		public DateTime? LastOnline { get; set; }
		public Dictionary<string,string> AdditionalInfo { get; set; }
		public DateTime? AddedDateTime { get; set; }

		public static User Parse(string str)
		{
			var splitted = str.Split(';', StringSplitOptions.RemoveEmptyEntries);
			return new User
			{
				Id = long.Parse(splitted[0]),
				FirstName = splitted.Length > 1 ? splitted[1] : null,
				LastName = splitted.Length > 2 ? splitted[2] : null,
				AddedDateTime = splitted.Length > 3 ? (DateTime?)DateTime.Parse(splitted[3]) : null,
			};
		}

		public override string ToString()
		{
			return $"{Id};{FirstName};{LastName};{AddedDateTime};";
		}
	}
}
