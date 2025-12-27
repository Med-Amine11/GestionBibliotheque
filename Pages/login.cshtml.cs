using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Services;
using GestionBibliotheque.Models; 
namespace GestionBibliotheque.Pages
{
    public class loginModel : PageModel
    {
        private readonly IConfiguration _configuration; 
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

		public String Erreur {  get; set; }
        public loginModel(IConfiguration configuration)
        {
			Email = "";
			Password = "";
            _configuration = configuration;
        }
        public void OnGet()
        {
			
        }
		public IActionResult OnPost()
		{
			try
			{
				UtilisateurService service = new UtilisateurService(_configuration.GetConnectionString("DefaultConnection"));

				Utilisateur utilisateur = service.Login(Email, Password);

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

				// Vérification rôle
				if (utilisateur.Role == "admin")
				{
					return RedirectToPage("/Admin/Index",utilisateur);
				}
				else // utilisateur normal
				{
					return RedirectToPage("/User/Index", utilisateur);
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
