using Models.Attachment;
using Models.Organization;
using Models.UserEventBinding;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.OrganizationEvent
{
	public class OrganizationEvents
	{
		public int Id { get; set; }
		public required int OrganizationId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int? AttachmentId { get; set; }
		public Attachments? Attachment { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime StartDate { get; set; } = DateTime.UtcNow;
		public int AgeLimit { get; set; } = 0;
		public required int UserOrganizationBindingId { get; set; }

		[NotMapped]
		public string CreatorName { get; set; } = string.Empty;

		public UserEventBindings? UserEventBinding { get; set; } = null!;
		public Organizations? Organization { get; set; } = null!;

	}
}