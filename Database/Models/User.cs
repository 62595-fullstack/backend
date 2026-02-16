using Models.UserEventBinding;
using Models.UserOrganizationBinding;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Models.User;

public class Users
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string FirstName { get; set; }

    public required string Username { get; set; }

    public required int Age { get; set; }

    public ICollection<UserOrganizationBindings> UserOrganizationBindings { get; set; } = new List<UserOrganizationBindings>();

    public ICollection<UserEventBindings> UserEventBindings { get; set; } = new List<UserEventBindings>();
}
