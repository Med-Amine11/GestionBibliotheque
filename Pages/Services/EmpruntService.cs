using GestionBibliotheque.Services;
using System.Data.SqlClient;
using GestionBibliotheque.Models;
using System;
using System.Data;
namespace GestionBibliotheque.Pages.Services
{
    public class EmpruntService
    {
        private static SqlConnection con; 

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString()); 
            con.Open(); 
        }

        public static List<Emprunt> getAllEmprunts()
        {
            List<Emprunt> Emprunts = new List<Emprunt>();
            try
            {
                OpenConnection();

                String sql = @"select e.id_emprunt , e.id_utilisateur , l.id_livre , date_emprunt , 
                                       date_retour_prevue, date_retour_effective , statut  , u.nom , u.prenom , l.titre
                                from emprunt e join livre l on l.id_livre = e.id_livre join utilisateur u on u.id_utilisateur = e.id_utilisateur 
                                where e.statut != 'termine' AND NOT EXISTS (
                                                                                SELECT 1
                                                                                FROM penalite p
                                                                                WHERE p.id_emprunt = e.id_emprunt
                                                                            )";
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Emprunt Emprunt = new Emprunt(
                                reader.GetInt32(0),
                                reader.GetDateTime(3),
                                reader.GetDateTime(4),
                                null,
                                reader.GetString(6),
                                reader.GetInt32(2),
                                reader.GetInt32(1), 
                                reader.GetString(7) ,
                                reader.GetString(8),
                                reader.GetString(9)
                                 );
                            Emprunts.Add(Emprunt);
                        }

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            } finally { 
            
                con.Close();
            }

            return Emprunts; 
        }

        public static int SetEmpruntEnCours(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = "update emprunt set statut = 'en_cours' where id_emprunt = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id); 
                    count = cmd.ExecuteNonQuery();  
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            } finally {
                con.Close();
            }
            return count;
        }

        public static int SetEmpruntTermine(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = "update emprunt set statut = 'termine', date_retour_effective = @Date where id_emprunt = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Date" , DateTime.Today);
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
        public static int SetEmpruntEnRetard(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = "update emprunt set statut = 'en_retard' where id_emprunt = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
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

        public static int DeleteEmpruntById(int id)
        {
            int count = 0;

            try
            {
                OpenConnection();
                String sql = "delete from emprunt where id_emprunt = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id); 
                    count = cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message) ;
            }
            finally
            {
                con.Close();   

            }
            return count;
        }

        public static string GetDateEmprunt(int id)
        {
            String dateTime = "";
            try
            {
                OpenConnection();
                String sql = "select date_emprunt from emprunt where id_emprunt = @Id ";
                using (SqlCommand cmd = new SqlCommand (sql ,con))
                {
                    cmd.Parameters.AddWithValue("@Id", id); 
                    Object reader = cmd.ExecuteScalar();
                    if(reader != null )
                    {
                        DateTime dt = Convert.ToDateTime(reader);
                        dateTime = dt.ToString("dd/MM/yyyy");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine (ex.Message) ;
            }
            finally
            {
                con.Close(); 
            }
            return dateTime ;
        }
        public static DateTime GetDateRetourEmprunt(int id , int days)
        {
            DateTime dateTime = DateTime.Today;
            try
            {
                OpenConnection();
                String sql = "select date_retour_prevue from emprunt where id_emprunt = @Id ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    Object reader = cmd.ExecuteScalar();
                    if (reader != null)
                    {
                        dateTime = Convert.ToDateTime(reader).AddDays(days);
                    }
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

            return dateTime;
        }


        public static int SetDateRetourTodayDate(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = "UPDATE emprunt SET date_retour_effective = @Today WHERE id_emprunt = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    // Paramètres sécurisés
                    cmd.Parameters.Add("@Today", SqlDbType.DateTime).Value = DateTime.Today;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine( ex.Message);
            }
            finally
            {
                con.Close();
            }

            return count; 
        }


    }

}
