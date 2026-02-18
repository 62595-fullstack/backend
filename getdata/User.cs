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
                await db.User.AddAsync(user);

                await db.SaveChangesAsync();

                return true;
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
                Users? user = await db.User.FirstOrDefaultAsync(u => u.Username == userName);

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
