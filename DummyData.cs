using Models.Post;
using Models.User;
using Models.Role;
using Models.Organization;
using Models.OrganizationPost;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;

class DummyData
{
	private static async Task Add<T>(DatabaseContext db, params T[] entities) where T : class
	{
		await db.Set<T>().AddRangeAsync(entities);
		await db.SaveChangesAsync();
	}

	public static async void Initialize()
	{
		using (var db = new DatabaseContext())
		{
			db.Post.RemoveRange(db.Post);
			db.User.RemoveRange(db.User);
			db.Role.RemoveRange(db.Role);
			db.Organization.RemoveRange(db.Organization);
			db.OrganizationPost.RemoveRange(db.OrganizationPost);
			db.OrganizationEvent.RemoveRange(db.OrganizationEvent);
			db.UserEventBinding.RemoveRange(db.UserEventBinding);
			db.UserOrganizationBinding.RemoveRange(db.UserOrganizationBinding);

			await Add(db,
				new Users { Email = "crazyfrog@hotmail.com", Password = "bingbing", FirstName = "Crazy", Username = "Frog", Age = 2 },
				new Users { Id = 1000, Email = "bbbenson@hotmail.com", Password = "1234", FirstName = "Berry B.", Username = "Benson", Age = 2 }
			);

			await Add(db,
				new Organizations { Id = 1000, Name = "NetCompany", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 123, Name = "Friske Gutter", CreatedDate = DateTime.UtcNow },
				new Organizations { Name = "HollyWood", CreatedDate = DateTime.UtcNow }
			);

			await Add(db,
				new Roles { Id = 1000, Name = "Admin" },
				new Roles { Name = "Employee" }
			);

			await Add(db,
				new UserOrganizationBindings { Id = 1000, UserId = 1000, OrganizationId = 1000, RoleId = 999 },
				new UserOrganizationBindings { Id = 123, UserId = 123, OrganizationId = 123, RoleId = 999 }
			);

			await Add(db,
				new OrganizationEvents {
					Id = 1000,
					OrganizationId = 1000,
					Title = "Åbent hus i NetCompany",
					CreatedDate = DateTime.UtcNow,
					StateDate = DateTime.UtcNow,
					AgeLimit = 18,
					UserOrganizationBindingId = 1000
				},
				new OrganizationEvents
				{
					Id = 123,
					OrganizationId = 123,
					Title = "Faldskærmsudspringning i Fælledparken",
					Description = "Kom og tag med os på en spændende faldskærmsudspringningsoplevelse!",
					ImageUrl = "/images/dynamic-realistic-parachuting.jpg",
					CreatedDate = DateTime.UtcNow,
					StateDate = DateTime.UtcNow,
					AgeLimit = 18,
					UserOrganizationBindingId = 123
				}
			);

			await Add(db,
				new UserEventBindings { Id = 1000, UserId = 1000, OrganizationEventsId = 1000 },
				new UserEventBindings { Id = 123, UserId = 123, OrganizationEventsId = 123 }
			);

			await Add(db,
				new Posts { Title = "Fist Event Post", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Id = 1000, Title = "Hello World", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Title = "Bee Movie Trailer Night", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 }
			);

			await Add(db,
				new OrganizationPosts { Id = 1000, OrganizationId = 1000, PostId = 1000 }
			);
		}
	}
}
