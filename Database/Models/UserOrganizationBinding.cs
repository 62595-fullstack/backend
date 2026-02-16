using Models.Organization;
using Models.Role;
using Models.User;

namespace Models.UserOrganizationBinding
{
    public class UserOrganizationBindings
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? OrganizationId { get; set; }

        public int RoleId { get; set; }


        // Setts the forenkey
        public Roles? Role { get; set; } = null!;

        public Users? User { get; set; } = null!;

        public Organizations? Organization { get; set; } = null!;
    }
}
