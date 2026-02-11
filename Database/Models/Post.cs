

using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.User;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;

namespace Models.Post
{
    public class Posts
    {
        public int Id { get; set; }
        
        public required string Title { get; set; }
        public string BodyText { get; set; } = "";
        
        public required DateTime CreatedDate { get; set; }
        public DateTime? LastUpdateDate { get; set; } = null;
        public required int UserId { get; set; }
        public required int OrganizationEventId { get; set; }


        public Users? User { get; set; } = null!;
        public OrganizationEvents? OrganizationEvent { get; set; } = null!;

        public OrganizationPosts PostId { get; set; }
    }
}
