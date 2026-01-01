namespace GestionBibliotheque.Models
{
    public class Categorie
    {
        public int Id_categorie { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }

        public String Image {  get; set; }
        public Categorie() { }

        public Categorie(int id_categorie, string nom ,string description , string image)
        {
            Id_categorie = id_categorie;
            Nom = nom;
            Description = description;
            Image = image;
        }
    }
}
