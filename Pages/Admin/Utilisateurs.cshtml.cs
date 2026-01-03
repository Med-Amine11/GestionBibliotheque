using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services; 
namespace GestionBibliotheque.Pages.Admin
{
    public class UtilisateursModel : PageModel
    {



		public List<Utilisateur> Utilisateurs { get; set; }
        public String MessageErr { get; set; }
        public String MessageSucc { get; set; }
        public UtilisateursModel()
        {
            Utilisateurs = Utilisateurs = UtilisateurService.GetAllUsers();

        }
        public IActionResult OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            return Page(); 
        }

        public IActionResult OnGetDelete(int id )
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }

            if (UtilisateurService.CheckUserInEmprunt(id) > 0)
            {
                MessageErr = "L'utilisateur a des emprunts ne peut pas etre supprimé.";
                return Page();
            }
            else if (UtilisateurService.DeleteUser(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la suppression de l'utilisateur.";
                return Page();

            }
            else { MessageSucc = "Utilisateur supprimé avec suucès."; }
           
            return RedirectToPage("/Admin/Utilisateurs"); ;
        }
    }
}
