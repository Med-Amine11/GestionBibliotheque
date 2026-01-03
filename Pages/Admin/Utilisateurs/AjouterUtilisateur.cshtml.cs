using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionBibliotheque.Pages.Admin
{
    public class AjouterUtilisateurModel : PageModel
    {

        [BindProperty]
        public Utilisateur NewUtilisateur { get; set; }
        public String MessageErr { get; set; }

        public AjouterUtilisateurModel()
        {
            MessageErr = ""; 
            NewUtilisateur = new Utilisateur();
        }
        public IActionResult OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            return Page(); 
        }
        public IActionResult OnPost()
        {
            if(UtilisateurService.CountUsersByNomPrenom( NewUtilisateur.Nom , NewUtilisateur.Prenom) == 1)
            {
                MessageErr = "Un utilisateur avec ce nom et prénom existe déjà.";
                return Page(); 
            }
            if (UtilisateurService.CountUsersByEmail(NewUtilisateur.Email) == 1)
            {
                MessageErr = "Cet email est déjà utilisé." ;
                return Page();
            }
            if (UtilisateurService.CountUsersByCin(NewUtilisateur.Cin) == 1)
            {
                MessageErr = "Ce CIN est déjà utilisé.";
                return Page();

            }
            if (UtilisateurService.CountUsersByTelephone(NewUtilisateur.Telephone) == 1)
            {
                MessageErr = "Ce numéro de téléphone est déjà utilisé.";
                return Page();

            }
            DateTime DateMin = DateTime.Today.AddYears(-100);
            DateTime DateMax = DateTime.Today.AddYears(-18);
            if (NewUtilisateur.Date_Naissance < DateMin || NewUtilisateur.Date_Naissance > DateMax)
            {
                MessageErr = "La date de naissance n'est pas valide.";
                return Page();
            }
            if (UtilisateurService.AddUser(NewUtilisateur) == 0 )
            {
                MessageErr = "Un problème est survenu lors de l'ajout du nouveau utilisateur.";
                return Page();
            }

            return RedirectToPage("/Admin/Utilisateurs/Utilisateurs"); 
        }
    }
}
