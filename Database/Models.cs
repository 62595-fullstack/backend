using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Organization;
using Models.OrganizationEvent;
using Models.OrganizationPost;
using Models.Post;
using Models.Role;
using Models.User;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;


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









    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connString = $@"Host={config["host"]};
                         Username={config["username"]};
                         Password={config["password"]};
                         Database={config["database"]}";

        optionsBuilder.UseNpgsql(connString);
    }
}






// dotnet ef migrations add InitialCreate
// dotnet ef database update
