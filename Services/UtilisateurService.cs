using GestionBibliotheque.Models;
using System.Data.SqlClient;

namespace GestionBibliotheque.Services
{
	public class UtilisateurService
	{
		private readonly String _connectionString; 

		public UtilisateurService(String connectionString)
		{
			_connectionString = connectionString;
		}


		public Utilisateur? Login(string email, string password)
		{
			Utilisateur utilisateur = null;
			try
			{
				using (SqlConnection con = new SqlConnection(_connectionString))
				{
					String Sql = @"
                   SELECT id_utilisateur, nom, prenom, email, password,  role , cin ,telephone , adresse , date_naissance ,actif
                   FROM Utilisateur
                   WHERE email = @email AND password = @password";

					using (SqlCommand cmd = new SqlCommand(Sql, con))
					{
						cmd.Parameters.AddWithValue("@email", email);
						cmd.Parameters.AddWithValue("@password", password);

						con.Open();

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
			}
			catch (SqlException ex)
			{
				Console.WriteLine("Je suis dans Utilisateur Service ");
				Console.WriteLine("Exception : " + ex.Message);
			}
			return utilisateur; 
		}
	}
}
