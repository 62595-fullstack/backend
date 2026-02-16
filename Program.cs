// TODO: To run the database please use:
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef database update

using Models.Post;
using Models.User;
using Models.Role;
using Models.Organization;
using Models.OrganizationPost;
using Models.OrganizationEvent;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;

using (var db = new DatabaseContext())
{
    db.Post.RemoveRange(db.Post);
    db.User.RemoveRange(db.User);
    db.Organization.RemoveRange(db.Organization);
    db.OrganizationPost.RemoveRange(db.OrganizationPost);
    db.OrganizationEvent.RemoveRange(db.OrganizationEvent);
    db.UserEventBinding.RemoveRange(db.UserEventBinding);
    db.UserOrganizationBinding.RemoveRange(db.UserOrganizationBinding);

    db.User.Add(new Users
    {
        Id = 10,
        Email = "bbbenson@hotmail.com",
        Password = "flowers_mmm",
        FirstName = "Berry B.",
        Username = "Benson",
        Age = 2,
    });

    db.Post.Add(new Posts
    {
        Title = "Fist Event Post",
        CreatedDate = DateTime.UtcNow,
        UserId = 1,
        OrganizationEventId = 1
    });

    db.Post.Add(new Posts
    {
        Title = "Hello World",
        CreatedDate = DateTime.UtcNow,
        UserId = 1,
        OrganizationEventId = 1
    });

    db.Organization.Add(new Organizations
    {
        Name = "HoneyFactory",
        CreatedDate = DateTime.UtcNow,
    });

    db.Organization.Add(new Organizations
    {
        Id = 10,
        Name = "NetCompany",
        CreatedDate = DateTime.UtcNow,
    });

    db.Role.Add(new Roles
    {
        Id = 10,
        Name = "Admin",
    });

    db.UserOrganizationBinding.Add(new UserOrganizationBindings
    {
        Id = 10,
        UserId = 10,
        OrganizationId = 10,
        RoleId = 10,
    });


    db.OrganizationEvent.Add(new OrganizationEvents
    {
        OrganizationId = 10,
        CreatedDate = DateTime.UtcNow,
        StateDate = DateTime.UtcNow,
        AgeLimit = 12,
        UserOrganizationBindingId = 10,
    });

    db.Post.Add(new Posts
    {
        Id = 10,
        Title = "Bee Movie Trailer Night",
        CreatedDate = DateTime.UtcNow,
        UserId = 10,
        OrganizationEventId = 10,
    });

    db.OrganizationPost.Add(new OrganizationPosts
    {
        Id = 10,
        OrganizationId = 10,
        PostId = 10,
    });

    await db.SaveChangesAsync();
}
