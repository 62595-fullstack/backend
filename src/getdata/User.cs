using Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.User;

namespace backend.getdata
{
	public class DataUser
	{
		public async Task<bool> AddUsers(RegisterCredentialsDto registerDto)
		{
			try
			{
				Users user = new Users
				{
					FirstName = registerDto.FirstName,
					LastName = registerDto.LastName,
					Email = registerDto.Email,
					DateOfBirth = registerDto.DateOfBirth,
					PasswordHash = registerDto.Password,
				};
				DatabaseContext db = new DatabaseContext();
				PasswordHasher<Users> ph = new PasswordHasher<Users>();

				user.PasswordHash = ph.HashPassword(user, user.PasswordHash!);
				await db.SaveChangesAsync();

				UserStore us = new UserStore(db);
				await us.CreateAsync(user);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}


		public async Task<bool> loginUsers(string email, string password)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				PasswordHasher<Users> ph = new PasswordHasher<Users>();

				Users? user = await getUserByEmail(email);
				if (user == null) return false;
				PasswordVerificationResult verificationResult = ph.VerifyHashedPassword(user, user.PasswordHash!, password);

				switch (verificationResult)
				{
					case PasswordVerificationResult.Success:
						return true;
					case PasswordVerificationResult.Failed:
						return false;
					default:
						return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}


		public async Task<Users?> getUserByEmail(string email)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				Users? user = await db.User.FirstOrDefaultAsync(u => u.Email == email);

				if (user != null)
				{
					return user;
				}
				else
				{
					throw new Exception("No User with Email");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<Users?> getUserByUserName(string userName)
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				Users? user = await db.User.FirstOrDefaultAsync(u => u.UserName == userName);

				if (user != null)
				{
					return user;
				}
				else
				{
					throw new Exception("No User with Username");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<List<Users>?> GetAllUsers()
		{
			try
			{
				DatabaseContext db = new DatabaseContext();
				List<Users> users = await db.User.ToListAsync();
				return users;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
	}
}