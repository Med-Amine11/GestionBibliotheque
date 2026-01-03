namespace GestionBibliotheque.Models
{
    public class Auteur
    {
        public int Id_auteur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateNaissance { get; set; }
        public DateTime? DateDeces { get; set; }
        public string Nationalite { get; set; }
        public string Biographie { get; set; }
        public string Photo { get; set; }

        // Constructeur par défaut
        public Auteur() { }

        // Constructeur avec paramètres
        public Auteur(int id_auteur, string nom, string prenom, DateTime dateNaissance,
                      DateTime? dateDeces, string nationalite, string biographie, string photo)
        {
            Id_auteur = id_auteur;
            Nom = nom;
            Prenom = prenom;
            DateNaissance = dateNaissance;
            DateDeces = dateDeces;
            Nationalite = nationalite;
            Biographie = biographie;
            Photo = photo;
        }

    }
}
