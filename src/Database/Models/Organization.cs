using Models.Attachment;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.UserOrganizationBinding;

namespace Models.Organization
{
	public class Organizations
	{
		public int Id { get; set; }

		public required string Name { get; set; }
		public required DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public string Description { get; set; } = "";
		public int OrganizationPostId { get; set; }

		public Attachments? CoverPhoto { get; set; }
		public int? CoverPhotoId { get; set; }
		public Attachments? ProfilePicture { get; set; }
		public int? ProfilePictureId { get; set; }

		public UserOrganizationBindings? UserOrganizationBindings { get; set; } = null!;
		public ICollection<OrganizationEvents?> OrganizationEvents { get; set; } = new List<OrganizationEvents?>();
		public ICollection<OrganizationPosts?> OrganizationPost { get; set; } = new List<OrganizationPosts?>();

	}
}