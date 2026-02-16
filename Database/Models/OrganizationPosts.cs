using System.ComponentModel.DataAnnotations.Schema;
using Models.Post;

namespace Models.OrganizationPost
{
    public class OrganizationPosts
    {
        public int Id { get; set; }

        [ForeignKey("OrganizationId")]
        public required int OrganizationId { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        public Posts Post { get; set; } = null!;
    }
}
