using Microsoft.EntityFrameworkCore;
using Models.Attachment;
using Models.Organization;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.Post;
using Models.Role;
using Models.User;
using Models.UserEventBinding;
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
	public DbSet<UserOrganizationBindings> UserOrganizationBinding { get; set; }
	public DbSet<Attachments> Attachment { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		try
		{
			IConfigurationRoot config = new ConfigurationBuilder()
				.AddJsonFile("secrets.json")
				.AddUserSecrets(Assembly.GetExecutingAssembly())
				.Build();

			var connString = $@"Host={config["host"] ?? config["User-thing:host"]};
						 Port={config["port"] ?? config["User-thing:port"]};
                         Username={config["username"] ?? config["User-thing:username"]};
                         Password={config["password"] ?? config["User-thing:password"]};
                         Database={config["database"] ?? config["User-thing:database"]}";
			optionsBuilder.UseNpgsql(connString);
		}
		catch
		{
			System.Console.WriteLine("No connections");
		}
	}
}

// dotnet ef migrations add [nameofmigrations]
// dotnet ef database update