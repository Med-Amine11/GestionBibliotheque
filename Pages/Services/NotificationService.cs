using System.Data.SqlClient;
using GestionBibliotheque.Services; 
using GestionBibliotheque.Models; 
namespace GestionBibliotheque.Pages.Services
{
    public class NotificationService
    {
        private static SqlConnection con; 

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString()); 
            con.Open();
        }

        public static int addNotification(Notification notification)
        {
            int count = 0;

            try
            {
                OpenConnection();
                string sql = @"insert into Notification (message , type_notification , date_notification , id_emprunt )
                                           values(@Message , @Type_notification , @Date_notification , @Id_emprunt) ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Message", notification.Message);
                    cmd.Parameters.AddWithValue("@Type_notification", notification.TypeNotification);
                    cmd.Parameters.AddWithValue("@Date_notification", notification.DateNotification);
                    cmd.Parameters.AddWithValue("@Id_emprunt", notification.Id_emprunt);

                    count  = cmd.ExecuteNonQuery();
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
