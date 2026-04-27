using Microsoft.EntityFrameworkCore;
using Models.Attachment;
using Models.Organization;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.Post;
using Models.Role;
using Models.User;
using Models.UserEventBinding;
using Models.UserFriendship;
using Models.UserOrganizationBinding;
using System.Reflection;

public class DatabaseContext : DbContext
{
	public DbSet<Users> User { get; set; }
	public DbSet<Organizations> Organization { get; set; }
	public DbSet<OrganizationEvents> OrganizationEvent { get; set; }
	public DbSet<OrganizationPosts> OrganizationPost { get; set; }
	public DbSet<Posts> Post { get; set; }
	public DbSet<Roles> Role { get; set; }
	public DbSet<UserEventBindings> UserEventBinding { get; set; }
	public DbSet<UserFriendships> UserFriendship { get; set; }
	public DbSet<UserOrganizationBindings> UserOrganizationBinding { get; set; }
	public DbSet<Attachments> Attachment { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		try
		{
			IConfigurationRoot config = new ConfigurationBuilder()
				.AddEnvironmentVariables()
				.AddUserSecrets(Assembly.GetExecutingAssembly())
				.Build();

			var connString = $@"Host={config["host"]};
						 Port={config["port"]};
                         Username={config["username"]};
                         Password={config["password"]};
                         Database={config["database"]}";
			optionsBuilder.UseNpgsql(connString);
		}
		catch
		{
			System.Console.WriteLine("No connections");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Users>()
			.HasAlternateKey(user => user.Id);

		modelBuilder.Entity<UserFriendships>()
			.HasIndex(friendship => new { friendship.UserAId, friendship.UserBId })
			.IsUnique();

		modelBuilder.Entity<UserFriendships>()
			.HasOne(friendship => friendship.UserA)
			.WithMany(user => user.FriendshipsAsUserA)
			.HasForeignKey(friendship => friendship.UserAId)
			.HasPrincipalKey(user => user.Id)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<UserFriendships>()
			.HasOne(friendship => friendship.UserB)
			.WithMany(user => user.FriendshipsAsUserB)
			.HasForeignKey(friendship => friendship.UserBId)
			.HasPrincipalKey(user => user.Id)
			.OnDelete(DeleteBehavior.Cascade);
	}
}

// dotnet ef migrations add [nameofmigrations]
// dotnet ef database update