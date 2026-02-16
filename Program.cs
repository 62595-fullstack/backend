// TODO: To run the database please use:
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef database update

using Models.Post;
using Models.User;
using Models.Organization;

using (var db = new DatabaseContext())
{
    db.User.RemoveRange(db.User);
    db.Post.RemoveRange(db.Post);
    db.Organization.RemoveRange(db.Organization);

    db.User.Add(new Users
    {
        Email = "bbbenson@hotmail.com",
        Password = "flowers_mmm",
        FirstName = "Berry B.",
        Username = "Benson",
        Age = 2,
    });

    db.Organization.Add(new Organizations
    {
        Name = "HoneyFactory",
        CreatedDate = DateTime.UtcNow,
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

    db.Post.Add(new Posts
    {
        Title = "Bee Movie Trailer Night",
        CreatedDate = DateTime.UtcNow,
        UserId = 1,
        OrganizationEventId = 1
    });

    await db.SaveChangesAsync();
}
