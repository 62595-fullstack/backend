using Models.User;

namespace backend.getdata
{
    public class DataUser
    {
        public bool setUsers(Users user)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                db.User.Add(user);

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Users? getUserByEmail(string email)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                Users? user = db.User.FirstOrDefault(u => u.Email == email);

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

        public Users? getUserByUserName(string userName)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                Users? user = db.User.FirstOrDefault(u => u.Username == userName);

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

        public List<Users>? GetAllUsers()
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                List<Users> users = db.User.ToList();
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
