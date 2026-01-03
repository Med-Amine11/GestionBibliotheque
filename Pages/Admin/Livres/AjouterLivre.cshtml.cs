using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
namespace GestionBibliotheque.Pages.Admin
{
    public class AjouterLivreModel : PageModel
    {
        [BindProperty]
        public Livre Book { get; set; }
        public List<Categorie> Categories { get; set; }

        public List<Auteur> Auteurs { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public IWebHostEnvironment _env { get; set; }

        public String MessageErr {  get; set; }
        public AjouterLivreModel(IWebHostEnvironment env)
        {
            _env = env; 
            Categories = CategorieService.getAllCategories();
            Auteurs = AuteurService.getAllAuthors();
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

            if(LivreService.CountBooksByTitle(Book.Titre) > 0)
            {
                MessageErr = "Un autre livre existe avec le meme titre.";
                return Page();
            }

            if (Book.NombrePages < 5 || Book.NombrePages > 5000)
            {
                MessageErr = "Un livre doit avoir au moins 5 pages et au maximum 5000 pages."; 
                return Page();
            }
            DateTime DateMin = new DateTime(1450 , 1  , 1);
            DateTime DateMax = DateTime.Today;
            if (Book.DatePublication < DateMin || Book.DatePublication > DateMax)
            {
                MessageErr = "La date de publication doit etre valide, au minimum (01/01/1450) et au maximum aujourd'hui."; 
                return Page();
            }
            if(Book.NbExemplaires <=  0)
            {
                MessageErr = "le nombre d'exemplaires doit etre supérieur strictement à 0.";
                return Page();
            }

            // lors de l'ajout le nombre disponibles de ce livre  doit etre égale au nombre d'exemplaires
            Book.NbDisponibles = Book.NbExemplaires;

            //Créer le chemin du dossier livres ou les images des livres seront stockées
            String dossier = Path.Combine(_env.WebRootPath, "images/livres"); 
            
            //Créer le dossier
            Directory.CreateDirectory(dossier);

            
            //Créer un nouveau nom pour l'image avec l'extention de l'image insérée
            String FileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

            //Remplir le champ Image par le nouveau nom avant d'ajouter le livre dans la bd 
            Book.Image = FileName;

            //Tenter d'ajouter le livre dans la bd
            if (LivreService.AddBook(Book) == 0)
            {
                MessageErr = "Un problème est survenu lors de l'insertion du livre.";
                return Page(); 
            }
            //Créer le chemin de l'image 
            String Chemin = Path.Combine(dossier , FileName);
            //Créer un objet de type flux de données d'un fichier en donnant en entrée le chemin et le mode 
            using(var Stream = new FileStream(Chemin , FileMode.Create))
            {
                Image.CopyTo(Stream);
            }
            return RedirectToPage("/Admin/Livres/Livres");
        }
    }
}
