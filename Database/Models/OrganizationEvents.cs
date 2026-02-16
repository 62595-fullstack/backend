using System.ComponentModel.DataAnnotations.Schema;
using Models.Organization;
using Models.UserEventBinding;

namespace Models.OrganizationEvent
{
    public class OrganizationEvents
    {
        public int Id { get; set; }

        [ForeignKey("OrganizationId")]
        public required int OrganizationId { get; set; }

        public required DateTime CreatedDate { get; set; }

        public required DateTime StateDate { get; set; }

        public int AgeLimit { get; set; } = 0;

        [ForeignKey("UserOrganizationBindingId")]
        public required int UserOrganizationBindingId { get; set; }


        public UserEventBindings? UserEventBinding { get; set; } = null!;
        public Organizations? Organization { get; set; } = null!;

    }
}
