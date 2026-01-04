using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class AuteursModel : PageModel
    {
        public String MessageErr {  get; set; }
        public List<Auteur> Auteurs {  get; set; }
        public IWebHostEnvironment _env {  get; set; }
        public AuteursModel(IWebHostEnvironment env)
        {
            _env = env;
           
        }
        private void LoadData()
        {
            Auteurs = AuteurService.getAllAuthors();

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
            if (AuteurService.CheckAuthorInBooks(id) > 0)
            {
                MessageErr = "L'auteur ne peut pas etre supprimé, il est lié à un ou plusieurs livres.";
                return Page(); 
            }
            String Photo = AuteurService.GetAuthorPhotoById(id);
            if(AuteurService.DeleteAuthorById(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la suppression de l'auteur.";
                return Page();
            }
            if (!String.IsNullOrEmpty(Photo))
            {
                String path = Path.Combine(_env.WebRootPath, "images/auteurs", Photo);
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
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
