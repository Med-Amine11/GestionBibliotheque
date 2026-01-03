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
            con.Open(); 
        }

        public static List<Livre> getAllBooks()
        {
            List<Livre> liste = new List<Livre>();
            try
            {
                OpenConnection();
                if(con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connexion Réussi !");
                }
                else
                {
                    Console.WriteLine("Connexion Echoué!");
                }
                string sql = @"select li.id_livre , c.id_categorie , c.nom, a.id_auteur , a.nom , a.prenom , li.titre , 
                                li.description , li.date_publication , li.nombre_pages , li.image , li.nb_exemplaires , li.nb_disponibles 
                                from livre li join auteur a on  li.id_auteur = a.id_auteur 
                                join categorie c on c.id_categorie = li.id_categorie
                                ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Livre livre = new Livre(reader.GetInt32(0),          // id_livre
                             reader.GetString(6),         // titre
                             reader.GetString(7),         // description
                             reader.GetDateTime(8),       // date_publication
                             reader.GetInt32(9),          // nombre_pages
                             reader.GetString(10),        // image
                             reader.GetInt32(11),         // nb_exemplaires
                             reader.GetInt32(12),         // nb_disponibles
                             reader.GetInt32(3),          // id_auteur
                             reader.GetString(4),         // nom auteur
                             reader.GetString(5),         // prenom auteur
                             reader.GetInt32(1),          // id_categorie
                             reader.GetString(2));          // nom categorie

                            liste.Add(livre);
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
            return liste;
        }

        public static int CountEmpruntPerBook(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = "select count(*) from emprunt id_livre = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Id" , id);

                    count = (int) cmd.ExecuteScalar();
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

        public static int DeleteBookById(int id)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = @"delete from livre where id_livre = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
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
                con.Close ();
            }

            return count;
        }

        public static String GetImageBookById(int id)
        {
            String image = null;
            try
            {
                OpenConnection ();
                String sql = "select image from livre where id_livre = @Id";
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id); 
                    Object result = cmd.ExecuteScalar();
                    if(result != null)
                    {
                        image = (String) result;
                    }
                }
            }
            catch (SqlException ex )
            {
                Console.WriteLine (ex.Message);
            }
            finally
            {
                con.Close(); 
            }
            return image;
        }

    public static int AddBook(Livre livre)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = @"INSERT INTO Livre
                        (
                            id_auteur,
                            id_categorie,
                            titre,
                            description,
                            date_publication,
                            nombre_pages,
                            image,
                            nb_exemplaires,
                            nb_disponibles
                        )
                        VALUES
                        (
                            @Id_auteur,
                            @Id_categorie,
                            @Titre,
                            @Description,
                            @Date_publication,
                            @Nombre_pages,
                            @Image,
                            @Nb_exemplaires,
                            @Nb_disponibles
                        )"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id_auteur", livre.Id_auteur);
                    cmd.Parameters.AddWithValue("@Id_categorie", livre.Id_categorie);
                    cmd.Parameters.AddWithValue("@Titre", livre.Titre);
                    cmd.Parameters.AddWithValue("@Description", livre.Description);
                    cmd.Parameters.AddWithValue("@Date_publication", livre.DatePublication);
                    cmd.Parameters.AddWithValue("@Nombre_pages", livre.NombrePages);
                    cmd.Parameters.AddWithValue("@Image", livre.Image);
                    cmd.Parameters.AddWithValue("@Nb_exemplaires", livre.NbExemplaires);
                    cmd.Parameters.AddWithValue("@Nb_disponibles", livre.NbDisponibles);

                    count = cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex )
            {
               Console.WriteLine (ex.Message);
            }
            finally
            {
                con.Close(); 
            }

            return count;
        }

        public static int CountBooksByTitle(string title)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = @"select count(*) from livre where titre = @Title"; 
                using(SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Title", title); 

                    count = (int) cmd.ExecuteScalar();
                }
            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close ();
            }

            return count; 
        }
    }
}
