using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class ModifierCategorieModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public int Id {  get; set; }
        [BindProperty]
        public Categorie Categorie { get; set; }
        public String MessageErr {  get; set; }
        public IFormFile UploadedImage { get; set; }

        public IWebHostEnvironment _env;
        public ModifierCategorieModel(IWebHostEnvironment env)
        {
           _env = env;
        }
        public IActionResult OnGet(int id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login"); 
            }

            Categorie = CategorieService.GetCategorieById(id);
            return Page(); 
        }
        public IActionResult  OnPost()
        {
            Categorie.Image = CategorieService.GetcategoryImageById(Categorie.Id_categorie);
            if(CategorieService.CountCategoryByName(Categorie.Nom) > 1)
            {
                MessageErr = "Elle existe une autre catégorie avec le meme nom.";
                return Page();
            }
            // Si l'utilisateur a donné une image 
            if(UploadedImage != null) {

                // Récupérer le chemin du dossier categories dans images dans wwwroot
                String dossier = Path.Combine(_env.WebRootPath, "images/categories");

                // Créer un nouveau nom pour l'image modifié
                String FileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedImage.FileName);

                // Créer le chemin du nouvelle image
                String NouveauCheminComplet = Path.Combine(dossier, FileName);

                // Récupérer le chemin de l'ancienne image 
                String AncienCheminComplet = Path.Combine(dossier, Categorie.Image);

                // Upload de la nouvelle image
                using (var stream = new FileStream(NouveauCheminComplet, FileMode.Create))
                {
                    UploadedImage.CopyTo(stream);
                }

                //Supprimer l'ancienne image
                try
                {
                    if (System.IO.File.Exists(AncienCheminComplet))
                        System.IO.File.Delete(AncienCheminComplet);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Categorie.Image = FileName;
            }
           
            // Modifier la catégorie dans la base de données
            if (CategorieService.UpdateCategorie(Categorie) == 0)
            {
                MessageErr = "Un problème est survenu lord de la modification de la catégorie";
                return Page();
            }
            return RedirectToPage("/Admin/Categories/Categories");
        }
    }
}
