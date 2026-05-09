using Dto;
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
			Users? user = await db.User.FirstOrDefaultAsync(u => u.Id == binding.UserId);
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

		public async Task<bool> updateEvent(int id, UpdateEventRequest req)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				OrganizationEvents? ev = await db.OrganizationEvent.FindAsync(id);
				if (ev == null) return false;
				if (req.Description != null) ev.Description = req.Description;
				if (req.Rules != null) ev.Rules = req.Rules;
				if (req.BracketResults != null) ev.BracketResults = req.BracketResults;
				await db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
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

		public async Task<bool> userJoinEvent(string userId, int organizationId)
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

				OrganizationEvents? ev = await db.OrganizationEvent.FindAsync(organizationId);
				UserOrganizationBindings? creatorBinding = ev != null
					? await db.UserOrganizationBinding.FindAsync(ev.UserOrganizationBindingId)
					: null;
				string? creatorUserId = creatorBinding?.UserId;

				if (ev == null || string.IsNullOrEmpty(creatorUserId) || creatorUserId == userId)
				{
					return true;
				}
				Users? joiner = await db.User.FirstOrDefaultAsync(u => u.Id == userId);
				string joinerName = joiner != null ? $"{joiner.FirstName} {joiner.LastName}" : "Someone";

				DataNotification notificationData = new();
				NotificationDto notification = await notificationData.Create(
					userId: creatorUserId,
					type: "event_join",
					message: $"{joinerName} joined your event '{ev.Title}'.",
					actorUserId: userId
				);
				NotificationStream.Publish(creatorUserId, notification);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> isUserRegistered(string userId, int eventId)
		{
			DatabaseContext db = new DatabaseContext();
			return await db.UserEventBinding.AnyAsync(b => b.UserId == userId && b.OrganizationEventsId == eventId);
		}

		public async Task<bool> userLeaveEvent(string userId, int eventId)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				UserEventBindings? binding = await db.UserEventBinding
					.FirstOrDefaultAsync(b => b.UserId == userId && b.OrganizationEventsId == eventId);
				if (binding == null) return false;
				db.UserEventBinding.Remove(binding);
				await db.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<List<Dto.EventParticipantDto>> getEventParticipants(int eventId)
		{
			DatabaseContext db = new DatabaseContext();
			List<UserEventBindings> bindings = await db.UserEventBinding
				.Where(b => b.OrganizationEventsId == eventId)
				.ToListAsync();

			List<Dto.EventParticipantDto> result = new();
			foreach (UserEventBindings binding in bindings)
			{
				if (binding.UserId == null) continue;
				Users? user = await db.User.FirstOrDefaultAsync(u => u.Id == binding.UserId);
				if (user != null)
					result.Add(new Dto.EventParticipantDto(binding.Id, user.Id ?? "", user.FirstName, user.LastName));
			}
			return result;
		}
	}
}
