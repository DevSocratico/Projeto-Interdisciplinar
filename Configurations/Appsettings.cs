namespace LocFarma.Configurations
{
    public class Appsettings
    {
        public static string GetKeyConnectionString()
        {
            return "DefaultConnection";
        }

        public static string GetConnectionString()
        {
            return "Server=DESKTOP-JKNMU6D\\SQLEXPRESS;Database=LocFarma;Trusted_Connection=True;TrustServerCertificate=True;";
        }
    }
}
