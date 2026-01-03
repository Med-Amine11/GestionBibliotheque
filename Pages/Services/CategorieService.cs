using System.Data.SqlClient;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;

namespace GestionBibliotheque.Pages.Services
{
    public class CategorieService
    {
        public static SqlConnection con;

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString());
            con.Open();
        }
        public static int AddCategory(Categorie categorie)
        {
            int ligne = 0;
            try
            {
                OpenConnection();
                String sql = @"insert into categorie (nom , photo , description) values 
                             (@Nom , @Photo , @Description )"; 
                using (SqlCommand cmd = new SqlCommand(sql , con ))
                {
                    cmd.Parameters.AddWithValue("@Nom", categorie.Nom);
                    cmd.Parameters.AddWithValue("@Photo", categorie.Image);
                    cmd.Parameters.AddWithValue("@Description", categorie.Description);

                    ligne = cmd.ExecuteNonQuery();
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Je suis ici.");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ligne;
        }
        public static int CountCategoryByName(string categoryName)
        {
            int ligne = 0;
            try
            {
                OpenConnection() ;
                String sql = "select count(*) from categorie where nom = @Nom";
                using (SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Nom", categoryName); 
                    ligne = (int)cmd.ExecuteScalar();
                }
            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ligne; 
        }

        public static List<Categorie> getAllCategories()
        {
            List<Categorie> categories = new List<Categorie>();

            try
            {
                OpenConnection();
                String sql = "select id_categorie , nom , photo , description  from categorie ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Categorie categorie = new Categorie();
                        categorie.Id_categorie = sqlDataReader.GetInt32(0); ;
                        categorie.Nom = sqlDataReader.GetString(1);
                        categorie.Image = sqlDataReader.GetString(2);
                        categorie.Description = sqlDataReader.GetString(3);
                        categories.Add(categorie);
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
            return categories; 
        }

        public static int DeleteCategoryById(int id)
        {
            int ligne = 0;
            try
            {
                OpenConnection();
                String sql = "delete from categorie where id_categorie = @id ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id" , id);
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

        public static Boolean CheckIdCategoryInLivres(int id)
        {
            int ligne = 0;
            try
            {
                OpenConnection ();
                String sql = "select count(id_livre) from livre where id_categorie = @Id";
                using (SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
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

            return ligne > 0;
        }

        public static String GetcategoryImageById(int id)
        {
            String image = null;
            try
            {
                OpenConnection();
                String sql = "select photo from categorie where id_categorie = @Id";
               
                using (SqlCommand cmd = new SqlCommand(sql, con)) 
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    Object result = cmd.ExecuteScalar();
                    if(result != null)
                    {
                        image = (String)result;
                    }
                }

            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return image; 
        }

        public static Categorie GetCategorieById(int id)
        {
            Categorie categorie = new Categorie(); 
            try
            {
                OpenConnection();
                String sql = "select id_categorie , nom , photo , description from categorie where id_categorie = @Id";
                using (SqlCommand cmd = new SqlCommand(sql , con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categorie.Id_categorie = reader.GetInt32(0);
                            categorie.Nom = reader.GetString(1);
                            categorie.Image = reader.GetString(2);
                            categorie.Description  = reader.GetString(3);
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine (ex.Message);
            }
            finally
            {
                con.Close();
            }
            return categorie;
        }

        public static int UpdateCategorie( Categorie categorie)
        {
            int ligne = 0;
            try
            {
                OpenConnection();
                String sql = "update categorie set nom = @Nom , description = @Description , photo = @Image where id_categorie = @Id"; 
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom" , categorie.Nom);
                    cmd.Parameters.AddWithValue("@Description", categorie.Description);
                    cmd.Parameters.AddWithValue("@Image", categorie.Image);
                    cmd.Parameters.AddWithValue("@Id", categorie.Id_categorie);

                    ligne = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine (0);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return ligne;
        }
    }
}
