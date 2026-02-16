using System.ComponentModel.DataAnnotations.Schema;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.UserOrganizationBinding;

namespace Models.Organization
{
    public class Organizations
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required DateTime CreatedDate { get; set; }
        public string Description { get; set; } = "";
        [ForeignKey("OrganizationPostId")]
        public int OrganizationPostId { get; set; }


        public UserOrganizationBindings? UserOrganizationBindings { get; set; } = null!;
        public OrganizationEvents? OrganizationEvent { get; set; } = null!;
        public List<OrganizationPosts> OrganizationPost { get; set; } = new List<OrganizationPosts>();
        // public ICollection<OrganizationPosts?> OrganizationPost { get; set; } = new List<OrganizationPosts>();

    }
}
