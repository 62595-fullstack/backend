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

			Users johnDoe = new() { Id = "9001", Email = "johndoe@mock.com", FirstName = "John", UserName = "John Doe", Age = 30 };
			johnDoe.PasswordHash = hasher.HashPassword(johnDoe, "mock");

			Users janeSmith = new() { Id = "9002", Email = "janesmith@mock.com", FirstName = "Jane", UserName = "Jane Smith", Age = 28 };
			janeSmith.PasswordHash = hasher.HashPassword(janeSmith, "mock");

			Users chefMarco = new() { Id = "9003", Email = "chefmarco@mock.com", FirstName = "Marco", UserName = "Chef Marco", Age = 35 };
			chefMarco.PasswordHash = hasher.HashPassword(chefMarco, "mock");

			Users lisaGreen = new() { Id = "9004", Email = "lisagreen@mock.com", FirstName = "Lisa", UserName = "Lisa Green", Age = 25 };
			lisaGreen.PasswordHash = hasher.HashPassword(lisaGreen, "mock");

			Users galleryOne = new() { Id = "9005", Email = "galleryone@mock.com", FirstName = "Gallery", UserName = "Gallery One", Age = 30 };
			galleryOne.PasswordHash = hasher.HashPassword(galleryOne, "mock");

			await Add(db, friskFyr, frog, benson, johnDoe, janeSmith, chefMarco, lisaGreen, galleryOne);

			// Organizations
			await Add(db,
				new Organizations { Id = 123, Name = "Friske Gutter", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 998, Name = "HollyWood", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 1000, Name = "NetCompany", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 9001, Name = "DTU", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 9002, Name = "ITU", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 9003, Name = "KU", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 9004, Name = "DTU", CreatedDate = DateTime.UtcNow },
				new Organizations { Id = 9005, Name = "ITU", CreatedDate = DateTime.UtcNow }
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
				new UserOrganizationBindings { Id = 1000, UserId = 1000, OrganizationId = 1000, RoleId = 999 },
				new UserOrganizationBindings { Id = 9001, UserId = 9001, OrganizationId = 9001, RoleId = 999 },
				new UserOrganizationBindings { Id = 9002, UserId = 9002, OrganizationId = 9002, RoleId = 999 },
				new UserOrganizationBindings { Id = 9003, UserId = 9003, OrganizationId = 9003, RoleId = 999 },
				new UserOrganizationBindings { Id = 9004, UserId = 9004, OrganizationId = 9004, RoleId = 999 },
				new UserOrganizationBindings { Id = 9005, UserId = 9005, OrganizationId = 9005, RoleId = 999 }
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
				},
				new OrganizationEvents
				{
					Id = 9001,
					OrganizationId = 9001,
					Title = "Summer Music Festival",
					Description = "A weekend of live music, food trucks, and good vibes in the park.",
					CreatedDate = new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc),
					StartDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc),
					AgeLimit = 18,
					UserOrganizationBindingId = 9001
					// posterAvatar: "https://picsum.photos/seed/john/100/100"
					// likes: 245
					// comments: 38
					// shares: 12
				},
				new OrganizationEvents
				{
					Id = 9002,
					OrganizationId = 9002,
					Title = "Tech Conference 2026",
					Description = "Join industry leaders for talks on AI, web development, and cloud computing.",
					CreatedDate = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc),
					StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
					AgeLimit = 0,
					UserOrganizationBindingId = 9002
					// posterAvatar: "https://picsum.photos/seed/jane/100/100"
					// likes: 189
					// comments: 52
					// shares: 27
				},
				new OrganizationEvents
				{
					Id = 9003,
					OrganizationId = 9003,
					Title = "Food & Wine Tasting",
					Description = "Explore local flavors with curated wine pairings and gourmet dishes.",
					CreatedDate = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc),
					StartDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
					AgeLimit = 18,
					UserOrganizationBindingId = 9003
					// posterAvatar: "https://picsum.photos/seed/marco/100/100"
					// likes: 312
					// comments: 45
					// shares: 8
				},
				new OrganizationEvents
				{
					Id = 9004,
					OrganizationId = 9004,
					Title = "Outdoor Yoga Session",
					Description = "Start your morning with a peaceful yoga session by the lake.",
					CreatedDate = new DateTime(2026, 2, 17, 0, 0, 0, DateTimeKind.Utc),
					StartDate = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc),
					AgeLimit = 0,
					UserOrganizationBindingId = 9004
					// posterAvatar: "https://picsum.photos/seed/lisa/100/100"
					// likes: 97
					// comments: 14
					// shares: 3
				},
				new OrganizationEvents
				{
					Id = 9005,
					OrganizationId = 9005,
					Title = "Art Exhibition Opening",
					Description = "Discover stunning contemporary art pieces from emerging local artists.",
					CreatedDate = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc),
					StartDate = new DateTime(2026, 3, 28, 0, 0, 0, DateTimeKind.Utc),
					AgeLimit = 0,
					UserOrganizationBindingId = 9005
					// posterAvatar: "https://picsum.photos/seed/gallery/100/100"
					// likes: 156
					// comments: 23
					// shares: 19
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