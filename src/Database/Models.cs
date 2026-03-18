using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models.Organization;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.Post;
using Models.Role;
using Models.User;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;
using Models.Attachment;

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
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var connString = $@"Host={config["host"]};
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
}

// dotnet ef migrations add [nameofmigrations]
// dotnet ef database update
