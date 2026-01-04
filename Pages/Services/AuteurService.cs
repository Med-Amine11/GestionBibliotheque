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
                    cmd.Parameters.AddWithValue("@Date_deces", auteur.DateDeces.HasValue? auteur.DateDeces : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nationalite", auteur.Nationalite);
                    cmd.Parameters.AddWithValue("@Biographie", auteur.Biographie);
                    cmd.Parameters.AddWithValue("@Photo", auteur.Photo);

                    ligne = cmd.ExecuteNonQuery();
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
            return ligne;
        }

       
        public static int CountAuthorsByNomPrenom(String Nom , String Prenom)
        {
            int ligne = 0; 
            try
            {
                OpenConnection();
                String sql = "select count(*) from auteur where nom = @Nom and prenom = @Prenom"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Nom", Nom);
                    cmd.Parameters.AddWithValue("@Prenom", Prenom);

                    ligne = (int)cmd.ExecuteScalar();
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
            return ligne;
        }
        public static List<Auteur> getAllAuthors()
        {
            List<Auteur> auteurs = new List<Auteur>();

            try
            {
                OpenConnection();
                String sql = "select * from auteur";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) {
                            Auteur auteur = new Auteur(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDateTime(3),
                                reader.IsDBNull(4) ?  null :  reader.GetDateTime(4) ,
                                reader.GetString(5),
                                reader.GetString(6),
                                reader.GetString(7));
                        auteurs.Add(auteur);

                    }

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
            return auteurs;
        }

        public static int CheckAuthorInBooks(int id)
        {
            int ligne = 0;
            try
            {
                OpenConnection();
                String sql = "select count(*) from livre where id_auteur = @Id";
                using (SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id); 
                    ligne = (int)cmd.ExecuteScalar();
                }
            }catch(SqlException ex)
            {
                Console.WriteLine (ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ligne;
        }

        public static String GetAuthorPhotoById(int id)
        {
            String photo = null;
            try
            {
                OpenConnection();
                String sql = "select photo from auteur where id_auteur = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    Object result = cmd.ExecuteScalar();
                    if(result != null)
                        photo = (String)result;
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
            return photo;
        }
        public static int DeleteAuthorById(int id)
        {
            int ligne = 0;
            try
            {
                OpenConnection ();
                String sql = "delete from auteur where id_auteur = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id" , id);
                    ligne = cmd.ExecuteNonQuery();
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
            return ligne;
        }

        public static Auteur GetAuthorById(int id)
        {
            Auteur auteur = null;
            try
            {
                OpenConnection();
                String sql = "select nom , prenom , date_naissance , date_deces , nationalite , biographie , photo from auteur where id_auteur = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            auteur = new Auteur(id, 
                                reader.GetString(0) , 
                                reader.GetString(1) ,
                                reader.GetDateTime(2) , 
                                reader.IsDBNull(3) ? null : reader.GetDateTime(4) , 
                                reader.GetString(4) , 
                                reader.GetString(5) ,   
                                reader.GetString(6) 
                                
                                );
                        }
                    }
                }

               
            }catch(SqlException ex){
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return auteur; 
        }

        public static int UpdateAuthor(Auteur author)
        {
            int ligne = 0;

            try
            {
                OpenConnection();

                string sql = @"UPDATE auteur 
                       SET nom = @Nom,
                           prenom = @Prenom,
                           date_naissance = @Date_naissance,
                           date_deces = @Date_deces,
                           nationalite = @Nationalite,
                           biographie = @Biographie,
                           photo = @Photo
                       WHERE id_auteur = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id", author.Id_auteur);
                    cmd.Parameters.AddWithValue("@Nom", author.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", author.Prenom);
                    cmd.Parameters.AddWithValue("@Date_naissance", author.DateNaissance);

                    // Date décès nullable
                    cmd.Parameters.AddWithValue(
                        "@Date_deces",
                        author.DateDeces.HasValue ? author.DateDeces : DBNull.Value
                    );

                    cmd.Parameters.AddWithValue("@Nationalite", author.Nationalite);
                    cmd.Parameters.AddWithValue("@Biographie", author.Biographie);

                    // Photo nullable
                    cmd.Parameters.AddWithValue(
                        "@Photo",
                        author.Photo
                    );

                    ligne = cmd.ExecuteNonQuery();
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

            return ligne;
        }

        public static int CountAuthorsSameNameDifferentId(String nom ,  String prenom , int id)
        {
            int count = 0;

            try
            {
                OpenConnection();
                String sql = "select count(*) from auteur where nom = @Nom and prenom = @Prenom and id_auteur != @Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", nom);  
                    cmd.Parameters.AddWithValue("@Prenom", prenom);  
                    cmd.Parameters.AddWithValue("@Id", id);  
                    count = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine (ex.Message);
            }
            finally
            {
                con.Close ();
            }
            return count;
        }
    }
}
