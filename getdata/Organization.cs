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


        public Organizations? GetOrganizationByName(string OrganizationName)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                
                db.Add(OrganizationName);

                db.SaveChanges();
                
                return db.Organization.TakeLast(1).First();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
