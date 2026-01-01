using GestionBibliotheque.Services;
using GestionBibliotheque.Models;


using System.Data.SqlClient;

namespace GestionBibliotheque.Pages.Services
{
    public class LivreService
    {
        private static SqlConnection con;

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString());
        }

        public static List<Livre> getAllBooks(int id_livre , int id_categorie)
        {
            List<Livre> liste = new List<Livre>();
            try
            {
                OpenConnection();

                con.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return liste;
        }
    }
}
