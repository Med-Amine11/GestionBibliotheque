namespace GestionBibliotheque.Services
{
	public class DBConnection
	{
		public static String getConnectionString()
		{
			return "Server=localhost;Database=gestionbiblio;Trusted_Connection=True;Encrypt=False;";
		}
	}
}
