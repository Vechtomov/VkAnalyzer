using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkAnalyzer.BE
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? AddedDateTime { get; set; }
		public Guid? AddedUser { get; set; }

		public static User Parse(string str)
		{
			var splitted = str.Split(';', StringSplitOptions.RemoveEmptyEntries);
			return new User
			{
				Id = long.Parse(splitted[0]),
				FirstName = splitted.Length > 1 ? splitted[1] : null,
				LastName = splitted.Length > 2 ? splitted[2] : null,
				AddedDateTime = splitted.Length > 3 ? (DateTime?)DateTime.Parse(splitted[3]) : null,
				AddedUser = splitted.Length > 4 ? (Guid?)Guid.Parse(splitted[4]) : null,
			};
		}

		public override string ToString()
		{
			return $"{Id};{FirstName};{LastName};{AddedDateTime};{AddedUser};";
		}
	}
}
