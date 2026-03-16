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
		Microsoft.EntityFrameworkCore.Metadata.IEntityType entityType = db.Model.FindEntityType(typeof(T))!;
		Microsoft.EntityFrameworkCore.Metadata.IKey primaryKey = entityType.FindPrimaryKey()!;

		foreach (T entity in entities)
		{
			object[] keyValues = primaryKey.Properties
				.Select(p => p.PropertyInfo!.GetValue(entity)!)
				.ToArray();

			if (await db.Set<T>().FindAsync(keyValues) == null)
				await db.Set<T>().AddAsync(entity);
		}

		await db.SaveChangesAsync();
	}

	public static async void Initialize()
	{
		using (DatabaseContext db = new DatabaseContext())
		{
			await Add(db,
				new Users { Id = 123, Email = "friskfyr@friskefyre.com", Password = "123", FirstName = "Frisk", Username = "Fyr", Age = 25 },
				new Users { Id = 999, Email = "crazyfrog@hotmail.com", Password = "bingbing", FirstName = "Crazy", Username = "Frog", Age = 2 },
				new Users { Id = 1000, Email = "bbbenson@hotmail.com", Password = "1234", FirstName = "Berry B.", Username = "Benson", Age = 2 }
			);

			await Add(db,
				new Organizations { Id = 123, Name = "Friske Gutter", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 998, Name = "HollyWood", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 1000, Name = "NetCompany", CreatedDate = DateTime.UtcNow }
			);

			await Add(db,
				new Roles { Id = 999, Name = "Employee" },
				new Roles { Id = 1000, Name = "Admin" }
			);

			await Add(db,
				new UserOrganizationBindings { Id = 123, UserId = 123, OrganizationId = 123, RoleId = 999 },
				new UserOrganizationBindings { Id = 1000, UserId = 1000, OrganizationId = 1000, RoleId = 999 }
			);

			await Add(db,
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
				},
				new OrganizationEvents
				{
					Id = 1000,
					OrganizationId = 1000,
					Title = "Åbent hus i NetCompany",
					CreatedDate = DateTime.UtcNow,
					StateDate = DateTime.UtcNow,
					AgeLimit = 18,
					UserOrganizationBindingId = 1000
				}
			);

			await Add(db,
				new UserEventBindings { Id = 123, UserId = 123, OrganizationEventsId = 123 },
				new UserEventBindings { Id = 1000, UserId = 1000, OrganizationEventsId = 1000 }
			);

			await Add(db,
				new Posts { Id = 998, Title = "Fist Event Post", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Id = 999, Title = "Bee Movie Trailer Night", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Id = 1000, Title = "Hello World", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 }
			);

			await Add(db,
				new OrganizationPosts { Id = 1000, OrganizationId = 1000, PostId = 1000 }
			);
		}
	}
}
