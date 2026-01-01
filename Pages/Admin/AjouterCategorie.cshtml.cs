using GestionBibliotheque.Models;
using GestionBibliotheque.Pages.Services;
using GestionBibliotheque.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace GestionBibliotheque.Pages.Admin
{
    public class AjouterCategorieModel : PageModel
    {
        [BindProperty]
        public Categorie Categorie { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public String MessageErr { get; set; }
        public IWebHostEnvironment _env;
        public AjouterCategorieModel(IWebHostEnvironment env)
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
            if (CategorieService.CountCategoryByName(Categorie.Nom) > 0)
            {
                MessageErr = "Une autre catégorie existe avec le meme nom.";
                return Page(); 
            }
             string directory = Path.Combine(_env.WebRootPath, "images/categories");

             Directory.CreateDirectory(directory);

             String nomPhoto = Guid.NewGuid().ToString() +Path.GetExtension(Image.FileName);

             Categorie.Image = nomPhoto;
            
            if(CategorieService.AddCategory(Categorie) == 0)
             {
                 MessageErr = "Un problème est survenu lors de l'insertion de la catégorie.";
                return Page();
            }

            string cheminComplet = Path.Combine(directory, nomPhoto);

            using (var stream = new FileStream(cheminComplet, FileMode.Create))
            {
                Image.CopyTo(stream);
            }
            return RedirectToPage("/Admin/Categories"); 

        }

    }
    }
