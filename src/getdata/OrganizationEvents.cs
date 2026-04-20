using Microsoft.EntityFrameworkCore;
using Models.OrganizationEvent;
using Models.User;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;

namespace backend.getdata
{
	public class DataOrganizationEvents
	{
		private async Task<string> resolveCreatorName(DatabaseContext db, int userOrganizationBindingId)
		{
			UserOrganizationBindings? binding = await db.UserOrganizationBinding.FindAsync(userOrganizationBindingId);
			if (binding?.UserId == null) return string.Empty;
			Users? user = await db.User.FindAsync(binding.UserId.Value.ToString());
			return user?.UserName ?? string.Empty;
		}

		public async Task<OrganizationEvents?> getOrganizationEventById(int eventId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				OrganizationEvents? ev = await db.OrganizationEvent
					.Include(x => x.Attachment)
					.FirstOrDefaultAsync(x => x.Id == eventId);
				if (ev != null)
					ev.CreatorName = await resolveCreatorName(db, ev.UserOrganizationBindingId);
				return ev;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		public async Task<List<OrganizationEvents>> getOrganizationEvents(int organizationId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();

				List<OrganizationEvents> events = await db.OrganizationEvent
					.Include(x => x.Attachment)
					.Where(x => x.OrganizationId == organizationId)
					.ToListAsync();

				foreach (OrganizationEvents ev in events)
					ev.CreatorName = await resolveCreatorName(db, ev.UserOrganizationBindingId);

				return events;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return new List<OrganizationEvents>();
			}
		}

		public async Task createOrganizationEvents(OrganizationEvents organizationEvents)
		{
			DatabaseContext db = new DatabaseContext();
			await db.OrganizationEvent.AddAsync(organizationEvents);
			await db.SaveChangesAsync();
		}

		public async Task<bool> deleteOrganizationEvent(int id)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				OrganizationEvents? ev = await db.OrganizationEvent.FindAsync(id);
				if (ev == null) return false;
				db.OrganizationEvent.Remove(ev);
				await db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> userJoinEvent(int userId, int organizationId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				UserEventBindings ueb = new UserEventBindings
				{
					UserId = userId,
					OrganizationEventsId = organizationId
				};

				await db.UserEventBinding.AddAsync(ueb);
				await db.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}
	}
}