using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Pages.Services;
using Microsoft.AspNetCore.DataProtection.Repositories;
namespace GestionBibliotheque.Pages.Admin
{
    public class AjouterAuteurModel : PageModel
    {
        [BindProperty]
        public Auteur Author { get; set; } 
        public IFormFile Photo { get; set; }
        public String MessageErr {  get; set; }

        public IWebHostEnvironment _env { get; set; }

        public AjouterAuteurModel(IWebHostEnvironment env)
        {
            _env = env;
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
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            if (AuteurService.CountAuthorsByNomPrenom(Author.Nom , Author.Prenom) > 0)
            {
                MessageErr = "Un autre existe avec le meme nom et prenom.";

                return Page();
            }

            DateTime dateMin = new DateTime(428, 1, 1);
            DateTime dateMax = DateTime.Today.AddYears(-20);
            if (Author.DateNaissance < dateMin || Author.DateNaissance > dateMax)
            {
                MessageErr = "la date de naissance doit etre valide.";
                return Page();
            }
            if(Author.DateDeces.HasValue &&  Author.DateDeces.Value < Author.DateNaissance)
            {
                MessageErr = "La date de décés doit etre postérieure à la date de naissance.";
                return Page(); 
            }

            String dossier = Path.Combine(_env.WebRootPath, "images/auteurs"); 
            Directory.CreateDirectory(dossier);
            String FileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
            String CheminComplet = Path.Combine(dossier, FileName);
            using(var Stream = new FileStream(CheminComplet,FileMode.Create))
            {
                Photo.CopyTo(Stream);
            }
            Author.Photo = FileName; 
            AuteurService.addAuthor(Author);
            return  RedirectToPage("/Admin/Auteurs/Auteurs" ) ; 
        }
    }
}
