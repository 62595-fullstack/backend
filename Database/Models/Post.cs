using System.ComponentModel.DataAnnotations.Schema;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.User;

namespace Models.Post
{
    public class Posts
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public string BodyText { get; set; } = "";

        public required DateTime CreatedDate { get; set; }
        public DateTime? LastUpdateDate { get; set; } = null;
        [ForeignKey("UserId")]
        public required int UserId { get; set; }
        [ForeignKey("OrganizationEventId")]
        public required int OrganizationEventId { get; set; }


        // public Users? User { get; set; } = null!;
        // public OrganizationEvents? OrganizationEvent { get; set; } = null!;

        // [ForeignKey("PostId")]
        // public OrganizationPosts PostId { get; set; }
    }
}
