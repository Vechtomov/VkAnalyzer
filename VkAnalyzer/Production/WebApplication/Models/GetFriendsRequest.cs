using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
	public class GetFriendsRequest
	{
		[Required]
		public long UserId { get; set; }
	}
}
