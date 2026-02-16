using Microsoft.EntityFrameworkCore;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=facebook;Database=BookFace");
}






// dotnet ef migrations add [nameofmigrations]
// dotnet ef database update