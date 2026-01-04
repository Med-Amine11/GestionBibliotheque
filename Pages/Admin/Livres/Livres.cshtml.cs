using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class LivresModel : PageModel
    {
        public List<Livre> Livres { get; set; }
        public string MessageErr { get; set; }

        public IWebHostEnvironment _env { get; set; }

        public LivresModel(IWebHostEnvironment env)
        {

            _env = env;

        }
        private void LoadData()
        {
            Livres = LivreService.getAllBooks();
        }
        public IActionResult OnGet()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            LoadData();
            return Page();

        }

        public IActionResult OnGetDelete(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }

            LoadData();
            if (LivreService.CountEmpruntPerBook(id) > 0)
            {
                MessageErr = "Le livre ne peut pas etre supprimé, il est associé à un ou plusieurs emprunt(s).";
                return Page();
            }

            //Récupérer le nom de l'image associé au livre
            String Image = LivreService.GetImageBookById(id);

            //Tenter de supprimer le livre par l'id
            if (LivreService.DeleteBookById(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la suppression du livre.";
                return Page();
            }

            // Récupérer le chemin complet de l'image dans le projet
            String chemin = Path.Combine(_env.WebRootPath, "images/livres", Image);

            if (System.IO.File.Exists(chemin))
            {
                try
                {
                    System.IO.File.Delete(chemin);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return RedirectToPage();
        }

    }
}
