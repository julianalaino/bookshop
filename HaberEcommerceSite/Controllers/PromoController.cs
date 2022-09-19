using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HaberEcommerceSite.Controllers
{
    public class PromoController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private readonly IUnitOfWork unitOfWork;

        //Ruta donde se guardarán las carátulas
        private const string FILEPATH = "~/Images/Promos";

        public PromoController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<PromoViewModel> list = new List<PromoViewModel>();

            IQueryable<Promo> listPromo = null;

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
                listPromo = unitOfWork.PromoRepository.GetQuerySearch(searchString,"");
            }
            else
            {
                listPromo = unitOfWork.PromoRepository.GetQuery("");
            }

            foreach (Promo promo in listPromo)
            {
                //Busqueda de autores en la tabla bookAuthor
                var listBookPromo = unitOfWork.BookPromoRepository.GetQuery("Book", Promo => Promo.PromoId == promo.ID);

                List<String> listBooks = new List<string>();

                foreach (BookPromo bookPromo in listBookPromo)
                {
                    listBooks.Add(bookPromo.Book.Title);
                }

                var viewModel = new PromoViewModel()
                {
                    Price = promo.Price,

                    Title = promo.Title,

                    ID = promo.ID,

                    Books = new SelectList(this.unitOfWork.BookRepository.Get().OrderBy(book=>book.Title), "ID", "Name"),

                    Book = listBooks,
                    
                    Description= promo.Description,

                    ImagePath = promo.ImagePath
                };

                list.Add(viewModel);
            }

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<PromoViewModel>.Create(list, pageIndex ?? 1, pageSize));
        }

        public ActionResult Create()
        {
            var viewModel = new PromoViewModel()
            {
                Books = new SelectList(this.unitOfWork.BookRepository.Get(), "ID", "Title")               
            };

            return View(viewModel);
        }



        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IQueryable<BookPromo> booksPromos = this.unitOfWork.BookPromoRepository.FindListById("Book,Promo", BookPromo => BookPromo.PromoId == Guid.Parse(id.ToString()));

            Promo promo = this.unitOfWork.PromoRepository.Find(Guid.Parse(id.ToString()));

            List<String> listBook = new List<string>();

            foreach (BookPromo bookPromo in booksPromos)
            {
                listBook.Add(bookPromo.Book.ID.ToString());
            }

            var viewModel = new PromoViewModel()
            {
                Price = promo.Price,

                Title = promo.Title,

                ID = promo.ID,

                Description = promo.Description,

                Books = new MultiSelectList(this.unitOfWork.BookRepository.Get().OrderBy(book=>book.Title), "ID", "Title", listBook),

                Book = listBook,               

                ImagePath = promo.ImagePath
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(PromoViewModel viewModel, IFormFile file, string nameImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = ViewData;

                    if (file != null)
                    {


                        if (String.IsNullOrEmpty(nameImage))
                        {
                            nameImage = file.FileName;
                        }

                        else nameImage = nameImage + ".png";

                        string fileDestinationPath = this.hostingEnvironment.WebRootPath + @"\Images\Promos";

                        var filePath = Path.Combine(fileDestinationPath, nameImage);

                        var imagePath = FILEPATH + "/" + nameImage;//Con este formato se guarda en la base

                        viewModel.ImagePath = imagePath;

                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                        }
                    }
                    else
                    {
                        //Si no se selecciona una imagen, se guarda por defecto esta
                        viewModel.ImagePath = FILEPATH + "/MissingCover.png";
                    }

                    var model = new Promo()
                    {
                        Price = viewModel.Price,

                        Title = viewModel.Title,

                        Description = viewModel.Description,                      

                        ImagePath = viewModel.ImagePath
                    };

                    this.unitOfWork.PromoRepository.Insert(model);

                    foreach (string bookId in viewModel.Book)
                    {
                        var book = this.unitOfWork.BookRepository.Find(Guid.Parse(bookId));

                        var bookPromoModel = new BookPromo()
                        {
                            Book = book,
                            Promo = model
                        };

                        this.unitOfWork.BookPromoRepository.Insert(bookPromoModel);
                    }

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Promo");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(PromoViewModel viewModel, IFormFile file, string nameImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        if (!String.IsNullOrEmpty(nameImage) && !nameImage.Equals(file.FileName))
                        {
                            nameImage = nameImage + ".png";

                        }
                        else
                        {
                            if (String.IsNullOrEmpty(nameImage))
                            {
                                nameImage = file.FileName;
                            }
                        }
                        string fileDestinationPath = this.hostingEnvironment.WebRootPath + @"\Images\Covers";

                        var filePath = Path.Combine(fileDestinationPath, nameImage);//Con este formato se guarda en el file system

                        var imagePath = FILEPATH + "/" + nameImage;//Con este formato se guarda en la base

                        viewModel.ImagePath = imagePath;

                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                        }
                    }

                    var model = this.unitOfWork.PromoRepository.Find(viewModel.ID);

                    model.Price = viewModel.Price;

                    model.Title = viewModel.Title;

                    model.Description = viewModel.Description;                   

                    model.ImagePath = viewModel.ImagePath;

                    //Lógica del update de libros

                    //Encuentra los libros que pertenecen a una promo dada
                    var bookExist = this.unitOfWork.BookPromoRepository.GetBooksById(model.ID);

                    foreach (string bookId in viewModel.Book)
                    {
                        var book = this.unitOfWork.BookRepository.Find(Guid.Parse(bookId));

                        var bookPromoModel = new BookPromo()
                        {
                            Book = book,

                            Promo = model
                        };
                        //Si la promo no tiene asignado este libro , entonces se inserta
                        if (!bookExist.Contains(book))
                        {
                            this.unitOfWork.BookPromoRepository.Insert(bookPromoModel);
                        }
                    }

                    foreach (Book book in bookExist)
                    {

                        //Si un autor no esta en la lista que vuelve de la view entonces se elimina
                        if (!viewModel.Book.Contains(book.ID.ToString()))
                        {
                            var bookPromo = this.unitOfWork.BookPromoRepository.FindById("Promo,Book", BookPromo => BookPromo.PromoId == model.ID && BookPromo.BookId == book.ID);

                            this.unitOfWork.BookPromoRepository.Delete(bookPromo);
                        }

                    }

                    this.unitOfWork.PromoRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Promo");
                }

                return View();
            }
            catch (DataException data)
            {
                ModelState.AddModelError(data.Message, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");

                return View();
            }
        }

        public ActionResult Delete(Guid? id)
        {
            try
            {
                Promo promo = this.unitOfWork.PromoRepository.Find(Guid.Parse(id.ToString()));

                BookPromo bookPromo = this.unitOfWork.BookPromoRepository.FindById("Promo,Book", Promo => Promo.PromoId == Guid.Parse(id.ToString()));

                this.unitOfWork.BookPromoRepository.Delete(bookPromo);

                this.unitOfWork.PromoRepository.Delete(promo);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Promo");
        }
    }
}