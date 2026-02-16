using Models.UserOrganizationBinding;

namespace Models.Role
{
    public class Roles
    {
        public int Id { get; set; }

        public required string Name { get; set; }


        public ICollection<UserOrganizationBindings> UserOrganizationBinding { get; set; } = new List<UserOrganizationBindings>();
    }
}
