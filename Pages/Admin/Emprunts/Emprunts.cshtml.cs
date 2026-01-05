using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class EmpruntsModel : PageModel
    {

        public List<Emprunt> Emprunts {  get; set; }    

        public String MessageErr {  get; set; }
        private void LoadData()
        {
            Emprunts = EmpruntService.getAllEmprunts(); 
        }
        public IActionResult OnGet()
        {
            if(String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

            LoadData(); 

            return Page(); 
        }

        public IActionResult OnPostPasserEnCours(int id )
        {

            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

            if(EmpruntService.SetEmpruntEnCours(id) == 0)
            {
                MessageErr = "Un problème est survenu lors du passage de l'emprunt en cours.";
                LoadData();
                return Page();
            }
            Notification notification = new Notification();
            notification.DateNotification = DateTime.Now;
            notification.Id_emprunt = id; 
            notification.TypeNotification =  "nouvel_emprunt";
            notification.Message = "Demande acceptée. Le livre sera disponible dès le " + EmpruntService.GetDateEmprunt(id)  ;
            if (NotificationService.addNotification(notification) == 0)
            {
                MessageErr = "Un problème est survenu lors de la création de la notification.";
            }
            LoadData();
            return Page();
        }

        public IActionResult OnPostTerminer(int id)
        {

            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

            if (EmpruntService.SetEmpruntTermine(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la fin de l'emprunt.";
            }
            LoadData(); 

            return Page();  
        }

        public IActionResult OnPostAnnuler(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

            if (EmpruntService.DeleteEmpruntById(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la suppression de l'emprunt.";
            }
            LoadData();

            return Page();
        }

        public IActionResult OnPostRappeler(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }
            Notification notification = new Notification();

            notification.Id_emprunt = id;
            notification.DateNotification = DateTime.Now;
            notification.TypeNotification = "rappel"; 
            notification.Message = "N’oubliez pas de rendre votre livre emprunté avant le " + EmpruntService.GetDateRetourEmprunt(id,3).ToString() +" pour éviter tout retard.";

            if (NotificationService.addNotification(notification) == 0) {
                MessageErr = "Un problème est survenu lors de l'envoi du rappel.";
            }
            LoadData();

            return Page();
        }

        public IActionResult OnPostEnRetard(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

            if(EmpruntService.SetEmpruntEnRetard(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la mise en retard de l'emprunt.";
                LoadData();

                return Page();
            }
            Notification notification = new Notification();
            notification.Id_emprunt = id;
            notification.DateNotification = DateTime.Now;
            notification.TypeNotification = "alert";
            notification.Message = "votre livre est en retard de plus de 3 jours. Passez à la bibliothèque pour connaître la pénalité.";
            if (NotificationService.addNotification(notification) == 0)
            {
                MessageErr = "Un problème est survenu lors de l'envoi d'avertissement.";
            }
            LoadData();

            return Page();

        }

        public IActionResult OnPostAjouterPenalite(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {

                return RedirectToPage("/login");

            }

 
            Penalite penalite = new Penalite();
            penalite.Id_emprunt = id;
            penalite.Montant = (DateTime.Today - EmpruntService.GetDateRetourEmprunt(id, 0)).Days * 10; 
            penalite.Description = "Le paiement de votre pénalité a été enregistré. Votre situation est désormais régularisée auprès de la bibliothèque.";

            if (PenaliteService.AddPenalite(penalite) == 0)
            {
                MessageErr = "Un problème est survenu lors de l'ajout de la pénalité.";
                LoadData();
                return Page();
            }

            if(EmpruntService.SetDateRetourTodayDate(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la modification de la date de retour effective.";
            }
            LoadData();
            return Page();
        }

    }


}
