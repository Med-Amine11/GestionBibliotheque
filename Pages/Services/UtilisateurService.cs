using GestionBibliotheque.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace GestionBibliotheque.Services
{
    public class UtilisateurService
    {
        public static SqlConnection con;

        public static void OpenConnection()
        {
            con = new SqlConnection(DBConnection.getConnectionString());
            con.Open();
        }

        public static Utilisateur? Login(string email, string password)
        {
            Utilisateur utilisateur = null;
            try
            {
                OpenConnection();
                String Sql = @"
                   SELECT id_utilisateur, nom, prenom, email, password,  role , cin ,telephone , adresse , date_naissance ,actif
                   FROM Utilisateur
                   WHERE email = @email AND password = @password";

                using (SqlCommand cmd = new SqlCommand(Sql, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            utilisateur = new Utilisateur(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetString(5),
                                reader.GetString(6),
                                reader.GetString(7),
                                reader.GetString(8),
                                reader.GetDateTime(9),
                                reader.GetBoolean(10)
                                );
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("Je suis dans Utilisateur Service ");
                Console.WriteLine("Exception : " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return utilisateur;
        }


        public static List<Utilisateur> GetAllUsers()
        {
            List<Utilisateur> list = new List<Utilisateur>();
            string sql = "select * from utilisateur where role = 'utilisateur'";

            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Créer un nouvel objet pour chaque ligne
                            Utilisateur user = new Utilisateur();
                            user.Id_utilisateur = reader.GetInt32(0);
                            user.Nom = reader.GetString(1);
                            user.Prenom = reader.GetString(2);
                            user.Email = reader.GetString(3);
                            user.Password = reader.GetString(4);
                            user.Role = reader.GetString(5);
                            user.Cin = reader.GetString(6);
                            user.Telephone = reader.GetString(7);
                            user.Adresse = reader.GetString(8);
                            user.Date_Naissance = reader.GetDateTime(9);
                            user.Actif = reader.GetBoolean(10);

                            list.Add(user);
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
            return list;
        }


        public static int AddUser(Utilisateur utilisateur)
        {
            int ligne = 0;
            try
            {
                OpenConnection();

                String sql = @"insert into Utilisateur (nom , prenom , email , password ,role ,  cin , telephone , adresse , date_naissance , actif )
                              values(@Nom , @Prenom , @Email , @Password ,@Role ,  @Cin , @Telephone , @Adresse , @Date_naissance , @Actif  )";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", utilisateur.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", utilisateur.Prenom);
                    cmd.Parameters.AddWithValue("@Email", utilisateur.Email);
                    cmd.Parameters.AddWithValue("@Password", utilisateur.Password);
                    cmd.Parameters.AddWithValue("@Role", "Utilisateur");
                    cmd.Parameters.AddWithValue("@Cin", utilisateur.Cin);
                    cmd.Parameters.AddWithValue("@Telephone", utilisateur.Telephone);
                    cmd.Parameters.AddWithValue("@Adresse", utilisateur.Adresse);
                    cmd.Parameters.AddWithValue("@Date_Naissance", utilisateur.Date_Naissance);
                    cmd.Parameters.AddWithValue("@Actif", 1);

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

        public static int UpdateUser(Utilisateur utilisateur)
        {
            int ligne = 0;
            try
            {
                OpenConnection();

                String sql = @"Update utilisateur set nom = @Nom , prenom = @Prenom , email = @Email , password = @Password ,
                         cin = @Cin , telephone = @Telephone , adresse = @Adresse , date_naissance = @Date_naissance , 
                       actif = @Actif where id_utilisateur = @Id_utilisateur";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", utilisateur.Nom);
                    cmd.Parameters.AddWithValue("@Prenom", utilisateur.Prenom);
                    cmd.Parameters.AddWithValue("@Email", utilisateur.Email);
                    cmd.Parameters.AddWithValue("@Password", utilisateur.Password);
                    cmd.Parameters.AddWithValue("@Cin", utilisateur.Cin);
                    cmd.Parameters.AddWithValue("@Telephone", utilisateur.Telephone);
                    cmd.Parameters.AddWithValue("@Adresse", utilisateur.Adresse);
                    cmd.Parameters.AddWithValue("@Date_Naissance", utilisateur.Date_Naissance);
                    cmd.Parameters.AddWithValue("@Actif", utilisateur.Actif);
                    cmd.Parameters.AddWithValue("@Id_utilisateur", utilisateur.Id_utilisateur);

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

        public static int DeleteUser(int id_utilisateur)
        {
            int ligne = 0;
            try
            {
                OpenConnection();

                String sql = @"delete from utilisateur where id_utilisateur = @Id_utilisateur";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {

                    cmd.Parameters.AddWithValue("@Id_utilisateur", id_utilisateur);

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

        public static int CountUsersByNomPrenom(string nom, string prenom)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) 
                       FROM utilisateur 
                       WHERE Nom = @Nom AND Prenom = @Prenom";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);

                    // ExecuteScalar retourne la première colonne de la première ligne (ici COUNT(*))
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByNomPrenom(string nom, string prenom , int id)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) 
                       FROM utilisateur 
                       WHERE Nom = @Nom AND Prenom = @Prenom and id_utilisateur != @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Id", id);

                    // ExecuteScalar retourne la première colonne de la première ligne (ici COUNT(*))
                    count = (int)cmd.ExecuteScalar();
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

        public static int CountUsersByEmail(string email)
        {
            int count = 0;
            try
            {
                OpenConnection(); // ouvre la connexion à la base

                string sql = @"SELECT COUNT(*) 
                       FROM utilisateur 
                       WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    // ExecuteScalar retourne la première colonne de la première ligne (ici COUNT(*))
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByEmail(string email, int id)
        {
            int count = 0;
            try
            {
                OpenConnection(); // ouvre la connexion à la base

                string sql = @"SELECT COUNT(*) 
                       FROM utilisateur 
                       WHERE Email = @Email and id_utilisateur != @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Id", id);
                    // ExecuteScalar retourne la première colonne de la première ligne (ici COUNT(*))
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByTelephone(string telephone)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) FROM utilisateur WHERE Telephone = @Telephone";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Telephone", telephone);
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByTelephone(string telephone , int id)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) FROM utilisateur WHERE Telephone = @Telephone and id_utilisateur != @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Telephone", telephone);
                    cmd.Parameters.AddWithValue("@Id", id);
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByCin(string cin)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) FROM utilisateur WHERE Cin = @Cin";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Cin", cin);
                    count = (int)cmd.ExecuteScalar();
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
        public static int CountUsersByCin(string cin , int id)
        {
            int count = 0;
            try
            {
                OpenConnection();

                string sql = @"SELECT COUNT(*) FROM utilisateur WHERE Cin = @Cin and id_utilisateur != @Id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Cin", cin);
                    cmd.Parameters.AddWithValue("@Id", id);
                    count = (int)cmd.ExecuteScalar();
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
        public static int CheckUserInEmprunt(int id_utilisateur)
        {
            int count = 0;
            try
            {
                OpenConnection();
                String sql = @"select count(*) from emprunt where id_utilisateur = @Id_utilisateur";
                using (SqlCommand cmd = new SqlCommand(sql, con)) 
                {

                    cmd.Parameters.AddWithValue("@Id_Utilisteur", id_utilisateur);
                    count = (int)cmd.ExecuteScalar();
                }

            }catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return count;
        }

        public static Utilisateur GestUserById(int id)
        {
            Utilisateur utilisateur = null;

            try
            {
                OpenConnection();

                string Sql = @"SELECT nom, prenom, email, password,role, cin, telephone, adresse, date_naissance, actif
                       FROM utilisateur
                       WHERE id_utilisateur = @id";

                using (SqlCommand cmd = new SqlCommand(Sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       
                        if (reader.Read())
                        {
                            utilisateur = new Utilisateur(
                                            id,                                           // id_utilisateur
                                            reader.GetString(0),                          // nom
                                            reader.GetString(1),                          // prenom
                                            reader.GetString(2),                          // email
                                            reader.GetString(3),                          // password
                                            reader.GetString(4),                          // role
                                            reader.GetString(5),                          // cin
                                            reader.GetString(6),                          // telephone
                                            reader.GetString(7) ,                         // adresse
                                            reader.GetDateTime(8),                        // date_naissance
                                            reader.GetBoolean(9)                          // actif
                                        );


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

            return utilisateur;
        }

        public static int CountUsersSameNameDifferentId(String nom , String prenom , int id)
        {
            int count = 0;

            try
            {
                OpenConnection();
                String sql = @"select count(*) from utilisateur where nom = @Nom and prenom = @Prenom and id_utilisateur != @Id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nom" , nom  );
                    cmd.Parameters.AddWithValue("@Prenom" , prenom  );
                    cmd.Parameters.AddWithValue("@Id" , id  );

                    count = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine (ex.Message);
            }
            finally
            {con.Close();

            }
            return count; 
        }

    }
}

