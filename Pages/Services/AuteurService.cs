using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using System.Data.SqlClient;
namespace GestionBibliotheque.Pages.Services
{
    public class AuteurService
    {
        private static SqlConnection con;

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString());
            con.Open();
        }

        public static int addAuthor(Auteur auteur)
        {
            int ligne = 0;
            try
            {
                OpenConnection();
                String sql = @"insert into auteur(nom , prenom , date_naissance , date_deces , nationalite , biographie , photo)
                              values(@Nom , @Prenom , @Date_naissance , @Date_deces , @Nationalite , @Biographie , @Photo)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", auteur.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", auteur.Prenom);
                    cmd.Parameters.AddWithValue("@Date_naissance", auteur.DateNaissance);
                    cmd.Parameters.AddWithValue("@Date_deces", auteur.DateDeces);
                    cmd.Parameters.AddWithValue("@Nationalite", auteur.Nationalite);
                    cmd.Parameters.AddWithValue("@Biographie", auteur.Biographie);
                    cmd.Parameters.AddWithValue("@Photo", auteur.Photo);

                    ligne = cmd.ExecuteNonQuery();
                }
                con.Close();    
            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ligne;
        }

        public static List<Auteur> getAllAuthors()
        {
            List<Auteur> auteurs = new List<Auteur>();

            try
            {
                OpenConnection();

                string sql = "SELECT * FROM auteur";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Auteur auteur = new Auteur
                            {
                            };

                            auteurs.Add(auteur);
                        }
                    }
                }

                con.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return auteurs; // jamais null
        }
    }
}
