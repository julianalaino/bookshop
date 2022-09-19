using HaberEcommerceSite.Data;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HaberEcommerceSite.Controllers
{
    public class SiteController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IHostingEnvironment hostingEnvironment;

        public SmtpConfig smtpConfig { get; }

        public SiteController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IOptions<SmtpConfig> smtpConfig)
        {
            this.unitOfWork = unitOfWork;

            this.hostingEnvironment = hostingEnvironment;

            this.smtpConfig = smtpConfig.Value;
        }

        public IActionResult Index()
        {
            List<ImageCarousel> imagesCarousel = getImagesCarousel();

            List<BookViewModel> recommendedBooks = getRecommendedBooks();

            List<PromoViewModel> promos = getPromos();

            ViewBag.imagesCrousel = imagesCarousel;

            ViewBag.recommendedBooks = recommendedBooks;

            ViewBag.promos = promos;

            // Se elimina el primero, porque es la imagen activa.
            imagesCarousel.Remove(imagesCarousel.First());

            return View();
        }

        public IActionResult MorePromos() {
            List<PromoViewModel> promos = getPromos();
            return View(promos);
        }

        public IActionResult Errepar()
        {
            return View();
        }

        public IActionResult Erreius()
        {
            return View();
        }

        public IActionResult Editorial()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }      

        public IActionResult ContactErrepar()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            getInterestErrepar(items);

            return View(items);
        }

        private static void getInterestErrepar(List<SelectListItem> items)
        {
            items.Add(new SelectListItem
            {
                Text = "Tributaria",

                Value = "1"
            });

            items.Add(new SelectListItem
            {
                Text = "Laboral y Previsional",

                Value = "2"
            });

            items.Add(new SelectListItem
            {
                Text = "Sociedades y Concursos",

                Value = "3"
            });

            items.Add(new SelectListItem
            {
                Text = "Contabilidad y administración",

                Value = "4"
            });

            items.Add(new SelectListItem
            {
                Text = "Rural",

                Value = "5"
            });

            items.Add(new SelectListItem
            {
                Text = "Regional Buenos Aires",

                Value = "6"
            });

            items.Add(new SelectListItem
            {
                Text = "Regional Córdoba",

                Value = "7"
            });

            items.Add(new SelectListItem
            {
                Text = "Regional Santa Fe",

                Value = "8"
            });

            items.Add(new SelectListItem
            {
                Text = "Todas las temáticas",

                Value = "9"
            });
        }

        public IActionResult ContactErreius()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            getInterestErreius(items);

            return View(items);
        }

        private static void getInterestErreius(List<SelectListItem> items)
        {
            items.Add(new SelectListItem
            {
                Text = "Derecho Administrativo",

                Value = "1"
            });

            items.Add(new SelectListItem
            {
                Text = "Derecho Civil",

                Value = "2"
            });

            items.Add(new SelectListItem
            {
                Text = "Derecho de Familia",

                Value = "3"
            });

            items.Add(new SelectListItem
            {
                Text = "Derecho Laboral",

                Value = "4"
            });

            items.Add(new SelectListItem
            {
                Text = "Derecho Comercial y Societario",

                Value = "5"
            });

            items.Add(new SelectListItem
            {
                Text = "Derecho Procesal",

                Value = "6"
            });

            items.Add(new SelectListItem
            {
                Text = "Todas las temáticas",

                Value = "7"
            });
        }

        public IActionResult BookDescription(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            IQueryable<BookAuthor> booksAuthors = this.unitOfWork.BookAuthorRepository.FindListById("Book,Author", BookAuthor => BookAuthor.BookId == Guid.Parse(ID.ToString()));

            Book book = this.unitOfWork.BookRepository.FindById("Editorial,Category,Subcategory,BookCollection,Bookbinding", Book => Book.ID == Guid.Parse(ID.ToString()));

            List<String> listAuthors = new List<string>();

            foreach (BookAuthor bookAuthor in booksAuthors)
            {
                listAuthors.Add(bookAuthor.Author.ID.ToString());
            }

            var viewModel = new BookViewModel()
            {
                ISBN = book.ISBN,

                Title = book.Title,

                ID = book.ID,

                Subtitle = book.Subtitle,

                Authors = new MultiSelectList(this.unitOfWork.AuthorRepository.Get(), "ID", "Name", listAuthors),

                Author = listAuthors,

                Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get(), "ID", "Description"),

                Editorial = book.Editorial.ID.ToString(),

                Bookbindings = new SelectList(this.unitOfWork.BookbindingRepository.Get(), "ID", "Description"),

                Bookbinding = book.Bookbinding.ID.ToString(),

                BookCollections = new SelectList(this.unitOfWork.BookCollectionRepository.Get(), "ID", "Description"),

                BookCollection = book.BookCollection.ID.ToString(),

                Categories = new SelectList(this.unitOfWork.CategoryRepository.Get(), "ID", "Description"),

                Category = book.Category.ID.ToString(),

                Subcategories = new SelectList(this.unitOfWork.SubcategoryRepository.Get(), "ID", "Description"),

                Subcategory = book.Subcategory.ID.ToString(),

                Weight = book.Weight,

                Recommended = book.Recommended,

                Description = book.Description,

                Pages = book.Pages,

                Content = book.Content,

                ImagePath = book.ImagePath,

                Price = this.unitOfWork.BookPriceListViewModelRepository.GetPriceByBook(book.ID)

            };

            return View(viewModel);
        }

        public IActionResult PromoDescription(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            IQueryable<BookPromo> booksPromos = this.unitOfWork.BookPromoRepository.FindListById("Book,Promo", BookPromo => BookPromo.PromoId == Guid.Parse(ID.ToString()));

            Promo promo = this.unitOfWork.PromoRepository.Find(Guid.Parse(ID.ToString()));

            List<String> listBook = new List<string>();

            foreach (BookPromo bookPromo in booksPromos)
            {
                listBook.Add(bookPromo.Book.Title.ToString());
            }

            var viewModel = new PromoViewModel()
            {
                Price = promo.Price,

                Title = promo.Title,

                ID = promo.ID,

                Description = promo.Description,

                Books = new MultiSelectList(this.unitOfWork.BookRepository.Get(), "ID", "Title", listBook),

                Book = listBook,

                ImagePath = promo.ImagePath
            };

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult Contact(string name, string email, string phone,string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Email emailService = new Email(this.smtpConfig);

                    emailService.Subject = "Contacto";

                    emailService.Body = message + Environment.NewLine+ Environment.NewLine +"Nombre: " + name + Environment.NewLine +"Teléfono: "+ phone + Environment.NewLine  +"Email: " + email ;

                    emailService.AddTo("info@edicioneshaber.com.ar");//this.smtpConfig.To);

                    string text;

                    if (emailService.Send(out text))
                    {
                        ViewBag.Message = "Gracias por contactarse con nosotros, nos comunicaremos con usted a la brevedad";
                    }
                    else
                    {
                        ViewBag.MessageError = "Error al enviar el email, por favor contactese a nuestro número telefónico";
                    }             
                }
            }
            catch
            {
                return View();
            }

            return View();
        }

        [HttpPost]
        public IActionResult ContactErreius(List<SelectListItem> items)
        {   
            try
            {
                if (ModelState.IsValid)
                {

                    String email = Request.Form["email"].ToString();

                    String name = Request.Form["name"].ToString();

                    String phone = Request.Form["phone"].ToString();

                    String city = Request.Form["city"].ToString();

                    String profession = Request.Form["profession"].ToString();

                    String specialty = Request.Form["specialty"].ToString();

                    String esSuscriptor= Request.Form["esSuscriptor"].ToString();

                    String format = Request.Form["format"].ToString();

                    String message= Request.Form["message"].ToString();

                    String interests = "";

                    String numSuscriptor = "";

                    String isSuscriptor = "NO";

                    if ("on".Equals(esSuscriptor))
                    {
                        numSuscriptor = Request.Form["numSuscriptor"].ToString();

                        isSuscriptor = "SÍ";
                    }

                    foreach (SelectListItem item in items)
                    {
                        if (item.Selected)
                        {
                            interests = interests + "  " + item.Text;
                        }
                    }

                    Email emailService = new Email(this.smtpConfig);

                    emailService.Subject = "Contacto web - Erreius";

                    emailService.Body = message + Environment.NewLine + Environment.NewLine + "Nombre: " + name + Environment.NewLine + "Teléfono: " + phone + Environment.NewLine + "Email: " + email + Environment.NewLine
                        + "Localidad: " + city + Environment.NewLine + "Profesión: "+ profession + Environment.NewLine +"Especialidad: "+specialty+ Environment.NewLine + "Formato: "+format+ Environment.NewLine+ 
                        "Temáticas de interes: "+interests+ Environment.NewLine+"Es Suscriptor: "+isSuscriptor+ Environment.NewLine+"Número de Suscriptor: "+ numSuscriptor;

                    emailService.AddTo(this.smtpConfig.Erreius);

                    string text;

                    if (emailService.Send(out text))
                    {
                        ViewBag.Message = "A la brevedad será contactado por un representante comercial.";
                    }
                    else
                    {
                        ViewBag.MessageError = "Error al enviar el email, por favor contactese a nuestro número telefónico";
                    }
                }
            }
            catch
            {
                return View();
            }

            List<SelectListItem> interest = new List<SelectListItem>();

            getInterestErreius(interest);

            return View(interest);
        }

        [HttpPost]
        public IActionResult ContactErrepar(List<SelectListItem> items)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String email = Request.Form["email"].ToString();

                    String name = Request.Form["name"].ToString();

                    String phone = Request.Form["phone"].ToString();

                    String city = Request.Form["city"].ToString();

                    String profession = Request.Form["profession"].ToString();

                    String format = Request.Form["format"].ToString();


                    String esSuscriptor = Request.Form["esSuscriptor"].ToString();

                    String message = Request.Form["message"].ToString();

                    String interests = "";

                    String numSuscriptor = "";

                    String isSuscriptor = "NO";

                    if ("on".Equals(esSuscriptor))
                    {
                        numSuscriptor = Request.Form["numSuscriptor"].ToString();

                        isSuscriptor = "SÍ";
                    }

                    foreach (SelectListItem item in items)
                    {
                        if (item.Selected)
                        {
                            interests = interests + "  " + item.Text;
                        }
                    }

                    Email emailService = new Email(this.smtpConfig);

                    emailService.Subject = "Contacto web - Errepar";

                    emailService.Body = message + Environment.NewLine + Environment.NewLine + "Nombre: " + name + Environment.NewLine + "Teléfono: " + phone + Environment.NewLine + "Email: " + email + Environment.NewLine
                        + "Localidad: " + city + Environment.NewLine + "Profesión: " + profession + Environment.NewLine + "Formato: " + format + Environment.NewLine +
                        "Temáticas de interes: " + interests + Environment.NewLine + "Es Suscriptor: " + isSuscriptor + Environment.NewLine + "Número de Suscriptor: " + numSuscriptor;

                    emailService.AddTo( this.smtpConfig.Errepar);

                    string text;

                    if (emailService.Send(out text))
                    {
                        ViewBag.Message = "A la brevedad será contactado por un representante comercial.";
                    }
                    else
                    {
                        ViewBag.MessageError = "Error al enviar el email, por favor contactese a nuestro número telefónico";
                    }
                }
            }
            catch
            {
                return View();
            }

            List<SelectListItem> interest = new List<SelectListItem>();

            getInterestErrepar(interest);

            return View(interest);
        }

        public IActionResult Shop(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<BookViewModel> list = new List<BookViewModel>();

            IQueryable<Book> listBook = null;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                // Busqueda de libros con filtro en cuadro de busqueda.
                listBook = unitOfWork.BookRepository.GetQuerySearch(searchString, "Editorial,Category,Subcategory,BookCollection,Bookbinding");
            }
            else
            {
                listBook = unitOfWork.BookRepository.GetQuery("Editorial,Category,Subcategory,BookCollection,Bookbinding");
            }

            foreach (Book book in listBook)
            {
                // Busqueda de autores en la tabla Author.
                var listBookAuthors = unitOfWork.BookAuthorRepository.GetQuery("Author", Book => Book.BookId == book.ID);

                List<String> listAuthors = new List<string>();

                foreach (BookAuthor bookAuthor in listBookAuthors)
                {
                    listAuthors.Add(bookAuthor.Author.Name);
                }

                String authors = String.Join(", ", listAuthors);

                var viewModel = new BookViewModel()
                {
                    ISBN = book.ISBN,

                    Title = book.Title,

                    ID = book.ID,

                    Subtitle = book.Subtitle,

                    Authors = new SelectList(this.unitOfWork.AuthorRepository.Get(), "ID", "Name"),

                    Author = listAuthors,

                    Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get(), "ID", "Description"),

                    Editorial = book.Editorial.Description,

                    Bookbindings = new SelectList(this.unitOfWork.BookbindingRepository.Get(), "ID", "Description"),

                    Bookbinding = book.Bookbinding.Description,

                    BookCollections = new SelectList(this.unitOfWork.BookCollectionRepository.Get(), "ID", "Description"),

                    BookCollection = book.BookCollection.Description,

                    Categories = new SelectList(this.unitOfWork.CategoryRepository.Get(), "ID", "Description"),

                    Category = book.Category.Description,

                    Subcategories = new SelectList(this.unitOfWork.SubcategoryRepository.Get(), "ID", "Description"),

                    Subcategory = book.Subcategory.Description,

                    Weight = book.Weight,

                    Description = book.Description,

                    Pages = book.Pages,

                    Content = book.Content,

                    ImagePath = book.ImagePath,

                    Price = this.unitOfWork.BookPriceListViewModelRepository.GetPriceByBook(book.ID)
                };

                list.Add(viewModel);
            }

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<BookViewModel>.Create(list, pageIndex ?? 1, pageSize));
        }

        public IActionResult UnderConstruction()
        {
            return View();
        }

        public IActionResult Carousel()
        {
            List<ImageCarousel> imagesCarousel = getImagesCarousel();

            return View(imagesCarousel);
        }

     
        [HttpPost]
        public ActionResult SaveImageCarousel(string nameImage, IFormFile imagesCarousel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string fileDestinationPath = this.hostingEnvironment.WebRootPath + @"\Images\Carousel";

                    var filePath = Path.Combine(fileDestinationPath, nameImage);
                       
                    if (imagesCarousel.Length > 0)
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            imagesCarousel.CopyTo(fileStream);
                        }
                    }   
                }
            }
            catch
            {
                return RedirectToAction("Carousel", "Site");
            }

            return RedirectToAction("Carousel", "Site");
        }

        private static List<ImageCarousel> getImagesCarousel()
        {
            List<ImageCarousel> imagesCarousel = new List<ImageCarousel>();

            Stack<string> dirs = new Stack<string>(20);

            if (! Directory.Exists("Web Root"))
            {
                throw new ArgumentException();
            }

            dirs.Push("Web Root");

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                string[] dirImage;

                try
                {
                    dirImage = Directory.GetDirectories("Web Root\\Images");
                }

                catch (UnauthorizedAccessException error)
                {
                    Console.WriteLine(error.Message);

                    continue;
                }
                catch (DirectoryNotFoundException error)
                {
                    Console.WriteLine(error.Message);

                    continue;
                }

                string[] files = null;

                try
                {
                    files = Directory.GetFiles("Web Root\\Images\\Carousel");
                }

                catch (UnauthorizedAccessException error)
                {
                    Console.WriteLine(error.Message);

                    continue;
                }

                catch (DirectoryNotFoundException error)
                {
                    Console.WriteLine(error.Message);

                    continue;
                }
                foreach (string file in files)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        ImageCarousel imageCarousel = new ImageCarousel();

                        imageCarousel.Name = fileInfo.Name;

                        imageCarousel.contentPath = "~/Images/Carousel/" + fileInfo.Name;

                        imagesCarousel.Add(imageCarousel);
                    }
                    catch (FileNotFoundException error)
                    {
                        Console.WriteLine(error.Message);

                        continue;
                    }
                }
            }

            return imagesCarousel;
        }

        private List<BookViewModel> getRecommendedBooks()
        {
            List<BookViewModel> list = new List<BookViewModel>();

            IQueryable<Book> listBook = null;

            listBook = unitOfWork.BookRepository.GetQuery("Editorial,Category,Subcategory,BookCollection,Bookbinding",Book=>Book.Recommended.Equals(true));
           
            foreach (Book book in listBook)
            {
                // Busqueda de autores en la tabla Author.
                var listBookAuthors = unitOfWork.BookAuthorRepository.GetQuery("Author", Book => Book.BookId == book.ID);

                List<String> listAuthors = new List<string>();

                foreach (BookAuthor bookAuthor in listBookAuthors)
                {
                    listAuthors.Add(bookAuthor.Author.Name);
                }

                var viewModel = new BookViewModel()
                {
                    ISBN = book.ISBN,

                    Title = book.Title,

                    ID = book.ID,

                    Subtitle = book.Subtitle,

                    Authors = new SelectList(this.unitOfWork.AuthorRepository.Get(), "ID", "Name"),

                    Author = listAuthors,

                    Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get(), "ID", "Description"),

                    Editorial = book.Editorial.Description,

                    Bookbindings = new SelectList(this.unitOfWork.BookbindingRepository.Get(), "ID", "Description"),

                    Bookbinding = book.Bookbinding.Description,

                    BookCollections = new SelectList(this.unitOfWork.BookCollectionRepository.Get(), "ID", "Description"),

                    BookCollection = book.BookCollection.Description,

                    Categories = new SelectList(this.unitOfWork.CategoryRepository.Get(), "ID", "Description"),

                    Category = book.Category.Description,

                    Subcategories = new SelectList(this.unitOfWork.SubcategoryRepository.Get(), "ID", "Description"),

                    Subcategory = book.Subcategory.Description,

                    Recommended = book.Recommended,

                    Weight = book.Weight,

                    Description = book.Description,

                    Pages = book.Pages,

                    Content = book.Content,

                    ImagePath = book.ImagePath
                };

                list.Add(viewModel);
            }

            return (list);
        }

        private List<PromoViewModel> getPromos()
        {
            List<PromoViewModel> list = new List<PromoViewModel>();

            IQueryable<Promo> listPromo = null;

            listPromo = unitOfWork.PromoRepository.GetQuery("",null);

            foreach (Promo promo in listPromo)
            {
                // Busqueda de autores en la tabla Author.
                var listPromoBook = unitOfWork.BookPromoRepository.GetQuery("Promo,Book", Promo => Promo.PromoId == promo.ID);

                List<String> listPromos = new List<string>();

                foreach (BookPromo bookPromo in listPromoBook)
                {
                    listPromos.Add(bookPromo.Book.Title);
                }

                var viewModel = new PromoViewModel()
                {
                    Price = promo.Price,

                    Title = promo.Title,

                    ID = promo.ID,

                    Description = promo.Description,

                    Books = new SelectList(this.unitOfWork.BookRepository.Get(), "ID", "Title"),

                    Book = listPromos,                  

                    ImagePath = promo.ImagePath
                };

                list.Add(viewModel);
            }

            return (list);
        }
    }
}
