


using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;

namespace Models.Organization
{
    public class Organizations
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public required DateTime CreatedDate { get; set; }
        public string Description { get; set; } = "";
        public int OrganizationPostId { get; set; }


        public UserOrganizationBindings? UserOrganizationBindings { get; set; } = null!;
        public OrganizationEvents? OrganizationEvent { get; set; } = null!;
        public ICollection<OrganizationPosts?> OrganizationPost { get; set; } = new List<OrganizationPosts>();

    }
}
