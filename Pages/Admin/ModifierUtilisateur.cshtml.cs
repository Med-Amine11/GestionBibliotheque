using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class ModifierUtilisateurModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public int Id { get; set; } 
        [BindProperty]
        public Utilisateur NewUtilisateur { get; set; }

        public String MessageErr    { get; set; }

        public IActionResult OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            if(Id == 0)
            {
                return RedirectToPage("/Admin/Utilisateurs"); 
            }
            NewUtilisateur = UtilisateurService.GestUserById(Id);
            return Page();
        }

        public IActionResult OnPost()
        {

            NewUtilisateur.Id_utilisateur = Id; 
            if (UtilisateurService.CountUsersByNomPrenom(NewUtilisateur.Nom, NewUtilisateur.Prenom, Id) == 1)
            {
                MessageErr = "Un utilisateur avec ce nom et prénom existe déjà.";
                return Page();
            }
            if (UtilisateurService.CountUsersByEmail(NewUtilisateur.Email, Id) == 1)
            {
                MessageErr = "Cet email est déjà utilisé.";
                return Page();
            }
            if (UtilisateurService.CountUsersByCin(NewUtilisateur.Cin, Id) == 1)
            {
                MessageErr = "Ce CIN est déjà utilisé.";
                return Page();

            }
            if (UtilisateurService.CountUsersByTelephone(NewUtilisateur.Telephone,Id) == 1)
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
            if (UtilisateurService.UpdateUser(NewUtilisateur) == 0)
            {
                MessageErr = "Un problème est survenu lors de l'ajout du nouveau utilisateur.";
                return Page();
            }

            return RedirectToPage("/Admin/Utilisateurs"); 
        }
    }
}
