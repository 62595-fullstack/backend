using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.User;

namespace backend.getdata
{
    public class DataUser
    {
        public async Task<bool> setUsers(Users user)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                PasswordHasher<Users> ph = new PasswordHasher<Users>();

                Users AddToDataBase = user;
                AddToDataBase.PasswordHash = ph.HashPassword(AddToDataBase, user.PasswordHash!);

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


        public async Task<bool> loginUsers(Users user)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                PasswordHasher<Users> ph = new PasswordHasher<Users>();

                Users AddToDataBase = user;
                AddToDataBase.PasswordHash = ph.HashPassword(AddToDataBase, user.PasswordHash!);

                if(user.PasswordHash == AddToDataBase.PasswordHash)
                {
                    return true;
                }
                else
                {
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
