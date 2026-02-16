using Models.Organization;

namespace backend.getdata
{
    public class DataOrganization
    {
        public Organizations CreateOrganization(Organizations organizations)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                db.Add(organizations);

                db.SaveChanges();

                return organizations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return organizations;
            }
        }
    }
}
