using Models.Post;

namespace Models.OrganizationPost
{
    public class OrganizationPosts
    {
        public int Id { get; set; }

        public required int OrganizationId { get; set; }

        public int PostId { get; set; }

        public Posts Post { get; set; } = null!;
    }
}
