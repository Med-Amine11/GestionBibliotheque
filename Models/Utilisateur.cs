namespace GestionBibliotheque.Models
{
	public class Utilisateur
	{
		public int Id_utilisateur {  get; set; }
		public String Nom {  get; set; }

		public String Prenom { get; set; }

		public String Email { get; set; }

		public String Password { get; set; }

		public String Role {  get; set; }

		public String Cin {  get; set; }

		public String Telephone {  get; set; }

		public String Adresse {  get; set; }

		public DateTime Date_Naissance { get; set; }

		public Boolean Actif {  get; set; }
		public Utilisateur()
		{

		}

		public Utilisateur(int id_utilisateur, string nom, string prenom, string email, string password, string role, string cin, string telephone, string adresse, DateTime date_Naissance, bool actif)
		{
			Id_utilisateur = id_utilisateur;
			Nom = nom;
			Prenom = prenom;
			Email = email;
			Password = password;
			Role = role;
			Cin = cin;
			Telephone = telephone;
			Adresse = adresse;
			Date_Naissance = date_Naissance;
			Actif = actif;
		}

		
	}
}
