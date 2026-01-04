using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using System.IO; 
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class CategoriesModel : PageModel
    {
        
        public List<Categorie> categories {  get; set; }
        public String MessageErr {  get; set; }
        public IWebHostEnvironment _env;
        public CategoriesModel(IWebHostEnvironment env)
        {
            _env = env;
        }
         private void LoadData()
        {
            categories = CategorieService.getAllCategories();

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

        public IActionResult OnGetDelete(int id) {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }

            LoadData();
            if (CategorieService.CheckIdCategoryInLivres(id) > 0)
            {
                MessageErr = "Cette categorie ne peut pas etre supprime, elle est liée à un seul ou plusieurs livre(s).";
                return Page();
            }
            String Image = CategorieService.GetcategoryImageById(id);
            
            if(CategorieService.DeleteCategoryById(id) == 0)
            {
                MessageErr = "Un problème est survenu lors de la suppression de la catégorie.";
                return Page();
            }
            if (!string.IsNullOrEmpty(Image))
            {
                string FilePath = Path.Combine(_env.WebRootPath, "Images/categories", Image);
                try
                {
                    if (System.IO.File.Exists(FilePath))
                        System.IO.File.Delete(FilePath);
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
