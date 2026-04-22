
using Models.Attachment;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.User;
using System.Reflection.Metadata;

namespace Models.Post
{
	public class Posts
	{
		public int Id { get; set; }
		
		public required string Title { get; set; }
		public string BodyText { get; set; } = "";

		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		public DateTime? LastUpdateDate { get; set; } = null;
		public required string UserId { get; set; }
		public required int OrganizationEventId { get; set; }

		public ICollection<Attachments> Attachment { get; set; } = new List<Attachments>();

		public Users? User { get; set; } = null!;
		public OrganizationEvents? OrganizationEvent { get; set; } = null!;

		//TODO: public required OrganizationPosts PostId { get; set; }
		public OrganizationPosts PostId { get; set; }
	}
}