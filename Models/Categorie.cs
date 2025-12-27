namespace GestionBibliotheque.Models
{
    public class Categorie
    {
        public int Id_categorie { get; set; }
        public string Nom { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }

        public Categorie() { }

        public Categorie(int id_categorie, string nom, string photo, string description)
        {
            Id_categorie = id_categorie;
            Nom = nom;
            Photo = photo;
            Description = description;
        }
    }
}
