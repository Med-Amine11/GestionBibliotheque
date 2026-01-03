namespace GestionBibliotheque.Models
{
    public class Livre
    {
        public int Id_livre { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public DateTime DatePublication { get; set; }
        public int NombrePages { get; set; }
        public string Image { get; set; }
        public int NbExemplaires { get; set; }
        public int NbDisponibles { get; set; }
       
        public int Id_auteur {  get; set; }

        public string NomAuteur {  get; set; }
        public string PrenomAuteur { get; set; }

        public int Id_categorie { get; set; }

        public string NomCategorie { get; set; }
        public Livre() { }

        public Livre(int id_livre, string titre, string description, DateTime datePublication,
                     int nombrePages, string image, int nbExemplaires, int nbDisponibles,
                     int id_auteur , string nomAuteur , string prenomAuteur , int id_categorie , string nomCategorie)
        {
            Id_livre = id_livre;
            Titre = titre;
            Description = description;
            DatePublication = datePublication;
            NombrePages = nombrePages;
            Image = image;
            NbExemplaires = nbExemplaires;
            NbDisponibles = nbDisponibles;
            Id_auteur = id_auteur;
            NomAuteur = nomAuteur;
            PrenomAuteur = prenomAuteur;
            Id_categorie = id_categorie;
            NomCategorie = nomCategorie; 
        }
    }
}
