

using System.Reflection.Metadata;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.User;
using Models.Attachment;

namespace Models.Post
{
        public class Posts
        {
                public int Id { get; set; }

                public required string Title { get; set; }
                public string BodyText { get; set; } = "";

                public required DateTime CreatedDate { get; set; } = DateTime.UtcNow;

                public DateTime? LastUpdateDate { get; set; } = null;
                public required int UserId { get; set; }
                public required int OrganizationEventId { get; set; }

                public ICollection<Attachments> Attachment { get; set; } = new List<Attachments>();

                public Users? User { get; set; } = null!;
                public OrganizationEvents? OrganizationEvent { get; set; } = null!;

                public OrganizationPosts PostId { get; set; }
        }
}
