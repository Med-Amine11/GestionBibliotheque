namespace GestionBibliotheque.Models
{
    public class Penalite
    {
        public int Id_penalite { get; set; }
        public string Description { get; set; }
        public decimal Montant { get; set; }
        public int Id_emprunt { get; set; }

        public Penalite() { }

        public Penalite(int id_penalite, string description, decimal montant, int id_emprunt)
        {
            Id_penalite = id_penalite;
            Description = description;
            Montant = montant;
            Id_emprunt = id_emprunt;
        }
    }
}
