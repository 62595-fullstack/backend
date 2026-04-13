using Microsoft.AspNetCore.Identity;
using Models.UserEventBinding;
using Models.UserOrganizationBinding;
using System.ComponentModel.DataAnnotations;

namespace Models.User;

public class Users : IdentityUser
{
	public required string FirstName { get; set; }

	public required int Age { get; set; }

	[Key]
	public override string? Email { get; set; }

	public ICollection<UserOrganizationBindings> UserOrganizationBindings { get; set; } = new List<UserOrganizationBindings>();

	public ICollection<UserEventBindings> UserEventBindings { get; set; } = new List<UserEventBindings>();

}