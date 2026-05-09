using Models.OrganizationEvent;
using Models.User;

namespace Models.UserEventBinding
{
	public class UserEventBindings
	{
		public int Id { get; set; }

		public string? UserId { get; set; }

		public required int OrganizationEventsId { get; set; }

		public Users? User { get; set; } = null!;

		public OrganizationEvents? OrganizationEvent { get; set; } = null!;
	}
}