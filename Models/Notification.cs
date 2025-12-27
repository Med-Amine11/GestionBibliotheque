namespace GestionBibliotheque.Models
{
    public class Notification
    {
        public int Id_notification { get; set; }
        public string Message { get; set; }
        public string TypeNotification { get; set; }
        public DateTime DateNotification { get; set; }
        public int Id_emprunt { get; set; }

        public Notification() { }

        public Notification(int id_notification, string message,
                            string typeNotification, DateTime dateNotification,
                            int id_emprunt)
        {
            Id_notification = id_notification;
            Message = message;
            TypeNotification = typeNotification;
            DateNotification = dateNotification;
            Id_emprunt = id_emprunt;
        }
    }
}
