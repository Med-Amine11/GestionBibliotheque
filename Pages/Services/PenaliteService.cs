using System.Data.SqlClient;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services; 
namespace GestionBibliotheque.Pages.Services
{
    public class PenaliteService
    {
        private static SqlConnection con;

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString());
            con.Open();
        }

        public static int AddPenalite(Penalite penalite)
        {
            int count = 0;

            try
            {
                OpenConnection();
                string sql = "insert into penalite (description , montant , id_emprunt) values (@Description , @Montant , @Id ) ";
                using (SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Description", penalite.Description);
                    cmd.Parameters.AddWithValue("@Montant", penalite.Montant);
                    cmd.Parameters.AddWithValue("@Id", penalite.Id_emprunt);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
              Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return count; 
        }
    }
}
