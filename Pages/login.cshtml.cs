using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Services;
using GestionBibliotheque.Models;
using Microsoft.AspNetCore.Http;
namespace GestionBibliotheque.Pages
{
    public class loginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

		public String Erreur {  get; set; }
         public loginModel()
        {
			Email = "";
			Password = "";
        }
		public void OnGet()
		{

		}
		public IActionResult OnPost()
		{
			try
			{

				Utilisateur utilisateur = UtilisateurService.Login(Email, Password);

				// Vérification utilisateur inexistant
				if (utilisateur == null)
				{
					Erreur =  "Email ou mot de passe incorrect";
					return Page();
				}

				// Vérification compte actif
				if (!utilisateur.Actif)
				{
                    Erreur = "Votre compte est désactivé";
					return Page();
				}
                HttpContext.Session.SetString("User_id", $"{utilisateur.Id_utilisateur}");
                // Vérification rôle
                if (utilisateur.Role == "admin")
				{
					return RedirectToPage("/Admin/Utilisateurs");
				}
				else // utilisateur normal
				{
					return RedirectToPage("/User/Index");
				}
			}
			catch (Exception ex)
			{
				// Gestion des erreurs
				Console.WriteLine("Je suis dans LoginModel ");
				Console.WriteLine("Exception : " + ex.Message);
                Erreur = "Une erreur est survenue ";
				return Page();
			}
		}

	}
}
