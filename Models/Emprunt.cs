namespace GestionBibliotheque.Models
{
    public class Emprunt
    {
        public int Id_emprunt { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetourPrevue { get; set; }
        public DateTime? DateRetourEffective { get; set; }
        public string Status { get; set; }
        public int Id_livre { get; set; }
        public int Id_utilisateur { get; set; }
        public String NomUtilisateur { get; set; }
        public String PrenomUtilisateur { get; set; }
        public String TitreLivre {  get; set; }
        public Emprunt() { }

        public Emprunt(int id_emprunt, DateTime dateEmprunt, DateTime dateRetourPrevue,
                     DateTime? dateRetourEffective, string status,
                     int id_livre, int id_utilisateur)
        {
            Id_emprunt = id_emprunt;
            DateEmprunt = dateEmprunt;
            DateRetourPrevue = dateRetourPrevue;
            DateRetourEffective = dateRetourEffective;
            Status = status;
            Id_livre = id_livre;
            Id_utilisateur = id_utilisateur;

        }
        public Emprunt(int id_emprunt, DateTime dateEmprunt, DateTime dateRetourPrevue,
                       DateTime? dateRetourEffective, string status,
                       int id_livre, int id_utilisateur , string nommUtilisateur , string prenomUtilisateur , string titreLivre)
        {
            Id_emprunt = id_emprunt;
            DateEmprunt = dateEmprunt;
            DateRetourPrevue = dateRetourPrevue;
            DateRetourEffective = dateRetourEffective;
            Status = status;
            Id_livre = id_livre;
            Id_utilisateur = id_utilisateur;
            NomUtilisateur = nommUtilisateur;
            PrenomUtilisateur = prenomUtilisateur;  
            TitreLivre = titreLivre;

        }
    }
}
