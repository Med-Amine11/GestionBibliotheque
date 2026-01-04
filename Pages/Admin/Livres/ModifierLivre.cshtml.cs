using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionBibliotheque.Models;
using GestionBibliotheque.Services;
using GestionBibliotheque.Pages.Services;
using System.Data.SqlClient;

namespace GestionBibliotheque.Pages.Admin
{
    public class ModifierLivreModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public Livre Book { get; set; }
        public List<Categorie> Categories { get; set; }

        public List<Auteur> Auteurs { get; set; }

        public IWebHostEnvironment _env {  get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        public String MessageErr {  get; set; }
        public ModifierLivreModel(IWebHostEnvironment env)
        {
            _env = env;
        }
        private void LoadData()
        {
            Categories = CategorieService.getAllCategories();
            Auteurs = AuteurService.getAllAuthors();
        }

        public IActionResult OnGet()
        {

            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }

            Book = LivreService.GetBookById(Id);
            if (Book == null)
            {
                return NotFound();
            }
            LoadData(); 

            return Page();
        }

        public IActionResult OnPost()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("User_id")))
            {
                return RedirectToPage("/login");
            }
            LoadData();
            if (LivreService.CountBooksSameTitleDifferentId(Book.Titre,Id) > 0)
            {
                MessageErr = "Un autre livre existe avec le meme titre.";
                return Page();
            }

            if (Book.NombrePages < 5 || Book.NombrePages > 5000)
            {
                MessageErr = "Un livre doit avoir au moins 5 pages et au maximum 5000 pages.";
                return Page();
            }
            DateTime DateMin = new DateTime(1450, 1, 1);
            DateTime DateMax = DateTime.Today;
            if (Book.DatePublication < DateMin || Book.DatePublication > DateMax)
            {
                MessageErr = "La date de publication doit etre valide, au minimum (01/01/1450) et au maximum aujourd'hui.";
                return Page();
            }
            if (Book.NbExemplaires <= 0)
            {
                MessageErr = "Le nombre d'exemplaires doit être supérieur à 0.";
                return Page();
            }

            if (Book.NbDisponibles < 0)
            {
                MessageErr = "Le nombre disponible doit être supérieur ou égal à 0.";
                return Page();
            }


            int empruntsEnCours = LivreService.CountEmpruntEncoursPerBook(Id);

            if (Book.NbExemplaires != empruntsEnCours + Book.NbDisponibles)
            {
                MessageErr = "Le nombre d'exemplaires doit être égale à la somme des emprunts en cours "+empruntsEnCours+" et du stock disponible.";
                return Page();
            }

            //Récupérer le nom de la photo associcée au livre 
            String AncienNom = LivreService.GetImageBookById(Id);
            Book.Image = AncienNom;
         
            if (Image != null)
            {
                // Récupérer le chemin de l'ancienne photo
                string AncienChemin = Path.Combine(_env.WebRootPath, "images/livres", AncienNom);

                // Générer un nouveau nom pour la nouvelle image
                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);

                Book.Image = FileName; // mettre à jour le modèle

                string NouveauChemin = Path.Combine(_env.WebRootPath, "images/livres", FileName);

                // Sauvegarder le nouveau fichier
                using (var Stream = new FileStream(NouveauChemin, FileMode.Create))
                {
                    Image.CopyTo(Stream);
                }

                // Supprimer l'ancienne image si elle existe
                if (System.IO.File.Exists(AncienChemin))
                {
                    try
                    {
                        System.IO.File.Delete(AncienChemin);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Erreur lors de la suppression de l'image : " + ex.Message);
                    }
                }
            }
            if(LivreService.UpdateBook(Book) == 0)
            {
                MessageErr = "Un problème est survenu lors de la modification du livre."; 
                return Page();  
            }


            return RedirectToPage("/Admin/Livres/Livres"); 
        }
    }
}
