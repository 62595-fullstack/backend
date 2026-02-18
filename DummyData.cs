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
	db.Role.RemoveRange(db.Role);
	db.Organization.RemoveRange(db.Organization);
	db.OrganizationPost.RemoveRange(db.OrganizationPost);
	db.OrganizationEvent.RemoveRange(db.OrganizationEvent);
	db.UserEventBinding.RemoveRange(db.UserEventBinding);
	db.UserOrganizationBinding.RemoveRange(db.UserOrganizationBinding);

	db.User.Add(new Users
	{
		Email = "crazyfrog@hotmail.com",
		Password = "bingbing",
		FirstName = "Crazy",
		Username = "Frog",
		Age = 2,
	});
	await db.SaveChangesAsync();

	db.User.Add(new Users
	{
		Id = 1000,
		Email = "bbbenson@hotmail.com",
		Password = "1234",
		FirstName = "Berry B.",
		Username = "Benson",
		Age = 2,
	});
	await db.SaveChangesAsync();

	db.Organization.Add(new Organizations
	{
		Id = 1000,
		Name = "NetCompany",
		CreatedDate = DateTime.UtcNow,
	});
	await db.SaveChangesAsync();

	db.Organization.Add(new Organizations
	{
		Name = "HollyWood",
		CreatedDate = DateTime.UtcNow,
	});
	await db.SaveChangesAsync();

	db.Role.Add(new Roles
	{
		Id = 1000,
		Name = "Admin",
	});
	await db.SaveChangesAsync();

	db.Role.Add(new Roles
	{
		Name = "Employee",
	});
	await db.SaveChangesAsync();

	db.UserOrganizationBinding.Add(new UserOrganizationBindings
	{
		Id = 1000,
		UserId = 1000,
		OrganizationId = 1000,
		RoleId = 1000,
	});

	db.OrganizationEvent.Add(new OrganizationEvents
	{
		Id = 1000,
		OrganizationId = 1000,
		CreatedDate = DateTime.UtcNow,
		StateDate = DateTime.UtcNow,
		AgeLimit = 18,
		UserOrganizationBindingId = 1000,
	});
	await db.SaveChangesAsync();

	db.UserEventBinding.Add(new UserEventBindings
	{
		Id = 1000,
		UserId = 1000,
		OrganizationEventsId = 1000,
	});

	db.Post.Add(new Posts
	{
		Title = "Fist Event Post",
		CreatedDate = DateTime.UtcNow,
		UserId = 1000,
		OrganizationEventId = 1000
	});
	await db.SaveChangesAsync();

	db.Post.Add(new Posts
	{
		Id = 1000,
		Title = "Hello World",
		CreatedDate = DateTime.UtcNow,
		UserId = 1000,
		OrganizationEventId = 1000,
	});
	await db.SaveChangesAsync();

	db.OrganizationPost.Add(new OrganizationPosts
	{
		Id = 1000,
		OrganizationId = 1000,
		PostId = 1000,
	});
	await db.SaveChangesAsync();

	db.Post.Add(new Posts
	{
		Title = "Bee Movie Trailer Night",
		CreatedDate = DateTime.UtcNow,
		UserId = 1000,
		OrganizationEventId = 1000,
	});
	await db.SaveChangesAsync();
}
