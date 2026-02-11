
using backend.getdata;
using Models.User;

public class Program
{
    public static void Main(string[] args)
    {
        Users user = new Users
        {
            Id = 1,
            Email = "test@test.com",
            Password = "password",
            FirstName = "test",
            Username = "tester",
            Age = 23

        };


        DataUser dataUser = new DataUser();


        bool v = dataUser.setUsers(user);



    }
}



// TODO run the database please use 
// docker run --name BookFace -e POSTGRES_PASSWORD=facebook -d -p 5432:5432 postgres
// dotnet ef datrabase update