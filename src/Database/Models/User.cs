using Microsoft.AspNetCore.Identity;
using Models.UserEventBinding;
using Models.UserFriendship;
using Models.UserOrganizationBinding;
using System.ComponentModel.DataAnnotations;

namespace Models.User;

public class Users : IdentityUser
{
	[MaxLength(128)]
	public required string FirstName { get; set; }

	public required int Age { get; set; }

	[Key]
	public override string? Email { get; set; }

	public ICollection<UserOrganizationBindings> UserOrganizationBindings { get; set; } = new List<UserOrganizationBindings>();

	public ICollection<UserEventBindings> UserEventBindings { get; set; } = new List<UserEventBindings>();

	public ICollection<UserFriendships> FriendshipsAsUserA { get; set; } = new List<UserFriendships>();

	public ICollection<UserFriendships> FriendshipsAsUserB { get; set; } = new List<UserFriendships>();

}