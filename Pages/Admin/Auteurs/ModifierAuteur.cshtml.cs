using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class ModifierAuteurModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public Auteur Author { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public String MessageErr {  get; set; } 

        public IWebHostEnvironment _env { get; set; }

        public ModifierAuteurModel(IWebHostEnvironment env)
        {
            _env = env ; 
        }
        public IActionResult OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")) )
            {
                return RedirectToPage("/login"); 
            }
            Author = AuteurService.GetAuthorById(Id);
            return Page(); 

        }

        public IActionResult OnPost()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            if (AuteurService.CountAuthorsByNomPrenom(Author.Nom, Author.Prenom) > 1 )
            {
                MessageErr = "Un autre auteur existe avec le meme nom et prénom.";
                return Page();
            }
            DateTime DateMin = new DateTime(481 ,1,1);
            DateTime DateMax = DateTime.Today.AddYears(-20); 

            if(Author.DateNaissance < DateMin || Author.DateNaissance > DateMax)
            {
                MessageErr = "la date de naissance doit etre valide.";
                return Page();
            }

            if (Author.DateDeces.HasValue && Author.DateDeces.Value < Author.DateNaissance)
            {
                MessageErr = "La date de décés doit etre postérieure à la date de naissance.";
                return Page();
            }
          
            //Récupérer le nom de l'ancienne photo
            String AncienPhoto = AuteurService.GetAuthorPhotoById(Id);
            //Remplir le champs photo vide 
            Author.Photo = AncienPhoto;


            if (Photo != null )
            {
              //Générer un nouveau nom 
               String NouveauNom = Guid.NewGuid().ToString() + Path.GetExtension(AncienPhoto);
                //Récupérer le chemin du dossier auteurs dans images
                String Dossier = Path.Combine(_env.WebRootPath, "images/auteurs");
                // Créer le nouveau chemin 
                String NouveauChemin = Path.Combine(Dossier, NouveauNom);
                // Créer l'ancien chemin 
                String AncienChemin = Path.Combine(Dossier, AncienPhoto);

                using (var stream = new FileStream(NouveauChemin, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }
                try
                {
                    if (System.IO.File.Exists(AncienChemin))
                    {
                        System.IO.File.Delete(AncienChemin);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

                //Ecraser l'ancien nom par le nouveau nom
                Author.Photo = NouveauNom;
            }
            //Modifier l'auteur 
            if (AuteurService.UpdateAuthor(Author) == 0)
            {
                MessageErr = "Un problème est survenu lors de la modification de l'auteur.";
                return Page();
            }
            return RedirectToPage("/Admin/Auteurs/Auteurs");
        }
    }
}
