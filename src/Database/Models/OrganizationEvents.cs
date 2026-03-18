using Models.Organization;
using Models.UserEventBinding;

namespace Models.OrganizationEvent
{
    public class OrganizationEvents
    {
        public int Id { get; set; }
        public required int OrganizationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? StateDate { get; set; }
        public int AgeLimit { get; set; } = 0;
        public int? UserOrganizationBindingId { get; set; }

        public UserEventBindings? UserEventBinding { get; set; } = null!;
        public Organizations? Organization { get; set; } = null!;

    }
}
