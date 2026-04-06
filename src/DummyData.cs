using Microsoft.AspNetCore.Identity;
using Models.Attachment;
using Models.Organization;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.Post;
using Models.Role;
using Models.User;
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

			T? existing = await db.Set<T>().FindAsync(keyValues);
			if (existing == null)
				await db.Set<T>().AddAsync(entity);
			else
				db.Entry(existing).CurrentValues.SetValues(entity);
		}

		await db.SaveChangesAsync();
	}

	public static async void Initialize()
	{
		using (DatabaseContext db = new())
		{
			PasswordHasher<Users> hasher = new();

			// Users
			Users friskFyr = new() { Id = "123", Email = "friskfyr@friskefyre.com", FirstName = "Frisk", UserName = "Frisk Fyr", Age = 25 };
			friskFyr.PasswordHash = hasher.HashPassword(friskFyr, "123");

			Users frog = new() { Id = "999", Email = "crazyfrog@hotmail.com", FirstName = "Crazy", UserName = "Crazy Frog", Age = 2 };
			frog.PasswordHash = hasher.HashPassword(frog, "bingbing");

			Users benson = new() { Id = "1000", Email = "bbbenson@hotmail.com", FirstName = "Berry B.", UserName = "Berry B. Benson", Age = 2 };
			benson.PasswordHash = hasher.HashPassword(benson, "1234");

			await Add(db, friskFyr, frog, benson);

			// Organizations
			await Add(db,
				new Organizations { Id = 123, Name = "Friske Gutter", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 998, Name = "HollyWood", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 1000, Name = "NetCompany", CreatedDate = DateTime.UtcNow }
			);

			// Roles
			await Add(db,
				new Roles { Id = 999, Name = "Employee" },
				new Roles { Id = 1000, Name = "Admin" }
			);

			// UserOrganizationBindings
			await Add(db,
				new UserOrganizationBindings { Id = 123, UserId = 123, OrganizationId = 123, RoleId = 999 },
				new UserOrganizationBindings { Id = 999, UserId = 999, OrganizationId = 998, RoleId = 999 },
				new UserOrganizationBindings { Id = 1000, UserId = 1000, OrganizationId = 1000, RoleId = 999 }
			);

			// OrganizationEvents
			await Add(db,
				new OrganizationEvents
				{
					Id = 123,
					OrganizationId = 123,
					Title = "Faldskærmsudspringning i Fælledparken",
					Description = "Kom og tag med os på en spændende faldskærmsudspringningsoplevelse!",
					CreatedDate = DateTime.UtcNow,
					StartDate = DateTime.UtcNow,
					AgeLimit = 18,
					UserOrganizationBindingId = 123
				},
				new OrganizationEvents
				{
					Id = 1000,
					OrganizationId = 1000,
					Title = "Åbent hus i NetCompany",
					CreatedDate = DateTime.UtcNow,
					StartDate = DateTime.UtcNow,
					AgeLimit = 18,
					UserOrganizationBindingId = 1000
				}
			);

			// Attach image to event 123 if not already linked (SetValues doesn't cascade to nav properties)
			var event123 = await db.OrganizationEvent.FindAsync(123);
			if (event123 != null && event123.AttachmentId == null)
			{
				var attachment = new Attachments
				{
					FileName = "dynamic-realistic-parachuting.jpg",
					FileType = "image/jpeg",
					Content = File.ReadAllBytes("../wwwroot/images/dynamic-realistic-parachuting.jpg"),
					CreatedDate = DateTime.UtcNow
				};
				db.Attachment.Add(attachment);
				await db.SaveChangesAsync();
				event123.AttachmentId = attachment.Id;
				await db.SaveChangesAsync();
			}

			// UserEventBindings
			await Add(db,
				new UserEventBindings { Id = 123, UserId = 123, OrganizationEventsId = 123 },
				new UserEventBindings { Id = 1000, UserId = 1000, OrganizationEventsId = 1000 }
			);

			// Posts
			await Add(db,
				new Posts { Id = 998, Title = "Fist Event Post", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Id = 999, Title = "Bee Movie Trailer Night", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 },
				new Posts { Id = 1000, Title = "Hello World", CreatedDate = DateTime.UtcNow, UserId = 1000, OrganizationEventId = 1000 }
			);

			// OrganizationPosts
			await Add(db,
				new OrganizationPosts { Id = 1000, OrganizationId = 1000, PostId = 1000 }
			);
		}
	}
}