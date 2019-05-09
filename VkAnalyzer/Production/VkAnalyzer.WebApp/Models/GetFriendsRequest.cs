using System.ComponentModel.DataAnnotations;

namespace VkAnalyzer.WebApp.Models
{
	public class GetFriendsRequest
	{
		[Required]
		public long UserId { get; set; }
	}
}
