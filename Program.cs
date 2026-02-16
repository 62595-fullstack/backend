
using backend.getdata;
using Models.User;

public class Program
{
    public static void Main(string[] args)
    {
        Users user = new Users
        {
            Email = "test@test.com",
            Password = "password",
            FirstName = "test",
            Username = "tester",
            Age = 23
        };


        DataUser dataUser = new DataUser();


        bool IsUserMade = dataUser.setUsers(user);

        if (!IsUserMade)
        {
            System.Console.WriteLine("User was not made.");
        }




    }
}



// TODO run the database please use 
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef database update