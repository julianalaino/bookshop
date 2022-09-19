using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace HaberEcommerceSite.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private readonly IUnitOfWork unitOfWork;

        // Ruta donde se guardan las carátulas.
        private const string FILEPATH = "~/Images/Covers";

        public BookController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
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
                listBook = unitOfWork.BookRepository.GetQuerySearch(searchString,"Editorial,Category,Subcategory,BookCollection,Bookbinding").OrderBy(book => book.Title);
            }
            else
            {
                listBook = unitOfWork.BookRepository.GetQuery("Editorial,Category,Subcategory,BookCollection,Bookbinding").OrderBy(book => book.Title);
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

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<BookViewModel>.Create(list, pageIndex ?? 1, pageSize));
        }

        public ActionResult Create()
        {
            Guid idEditorial = this.unitOfWork.EditorialRepository.FindById("",editorial=>editorial.Description=="Sin Editorial").ID;

            Guid idBookBinding = this.unitOfWork.BookbindingRepository.FindById("", bookBinding => bookBinding.Description == "Sin Encuadernación").ID;

            Guid idBookCollection = this.unitOfWork.BookCollectionRepository.FindById("", bookCollection => bookCollection.Description == "Sin Colección").ID;

            Guid idCategory = this.unitOfWork.CategoryRepository.FindById("", category => category.Description == "Sin Categoría").ID;

            Guid idSubcategory = this.unitOfWork.SubcategoryRepository.FindById("", subcategory => subcategory.Description == "Sin Subcategoría").ID;


            var viewModel = new BookViewModel()
            {
                Authors = new SelectList(this.unitOfWork.AuthorRepository.Get().OrderBy(author=>author.Name), "ID", "Name"),

                Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get().OrderBy(editorial=>editorial.Description), "ID", "Description", Convert.ToString(idEditorial)),

                Bookbindings = new SelectList(this.unitOfWork.BookbindingRepository.Get().OrderBy(bookbinding=> bookbinding.Description), "ID", "Description", Convert.ToString(idBookBinding)),

                BookCollections = new SelectList(this.unitOfWork.BookCollectionRepository.Get().OrderBy(bookCollection => bookCollection.Description), "ID", "Description", Convert.ToString(idBookCollection)),

                Categories = new SelectList(this.unitOfWork.CategoryRepository.Get().OrderBy(category => category.Description), "ID", "Description", Convert.ToString(idCategory)),

                Subcategories = new SelectList(this.unitOfWork.SubcategoryRepository.Get().OrderBy(subcategory => subcategory.Description), "ID", "Description", Convert.ToString(idSubcategory))
            };
           

            return View(viewModel);
        }



        public ActionResult Edit(Guid? ID)
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

                Authors = new MultiSelectList(this.unitOfWork.AuthorRepository.Get().OrderBy(author=>author.Name), "ID", "Name", listAuthors),

                Author = listAuthors,

                Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get().OrderBy(editorial=>editorial.Description), "ID", "Description"),

                Editorial = book.Editorial.ID.ToString(),

                Bookbindings = new SelectList(this.unitOfWork.BookbindingRepository.Get().OrderBy(bookbinding => bookbinding.Description), "ID", "Description"),

                Bookbinding = book.Bookbinding.ID.ToString(),

                BookCollections = new SelectList(this.unitOfWork.BookCollectionRepository.Get().OrderBy(bookCollection => bookCollection.Description), "ID", "Description"),

                BookCollection = book.BookCollection.ID.ToString(),

                Categories = new SelectList(this.unitOfWork.CategoryRepository.Get().OrderBy(category => category.Description), "ID", "Description"),

                Category = book.Category.ID.ToString(),

                Subcategories = new SelectList(this.unitOfWork.SubcategoryRepository.Get().OrderBy(subcategory => subcategory.Description), "ID", "Description"),

                Subcategory = book.Subcategory.ID.ToString(),

                Weight = book.Weight,

                Recommended=book.Recommended,

                Description = book.Description,

                Pages = book.Pages,

                Content = book.Content,

                ImagePath = book.ImagePath
            };
       
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(BookViewModel viewModel, IFormFile file, string nameImage)
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

                        string fileDestinationPath = this.hostingEnvironment.WebRootPath + @"\\Images\\Covers";

                        var filePath = Path.Combine(fileDestinationPath, nameImage);

                        // Con este formato se guarda en la base de datos.
                        var imagePath = FILEPATH + "/" + nameImage;

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

                    var model = new Book()
                    {
                        ISBN = viewModel.ISBN,

                        Title = viewModel.Title,

                        Subtitle = viewModel.Subtitle,

                        Editorial = this.unitOfWork.EditorialRepository.Find(Guid.Parse(viewModel.Editorial)),

                        Category = this.unitOfWork.CategoryRepository.Find(Guid.Parse(viewModel.Category)),

                        Subcategory = this.unitOfWork.SubcategoryRepository.Find(Guid.Parse(viewModel.Subcategory)),

                        Weight = viewModel.Weight,

                        Recommended=viewModel.Recommended,

                        Description = viewModel.Description,

                        BookCollection = this.unitOfWork.BookCollectionRepository.Find(Guid.Parse(viewModel.BookCollection)),

                        Pages = viewModel.Pages,

                        Content = viewModel.Content,

                        Bookbinding = this.unitOfWork.BookbindingRepository.Find(Guid.Parse(viewModel.Bookbinding)),

                        ImagePath = viewModel.ImagePath
                    };

                    this.unitOfWork.BookRepository.Insert(model);

                    foreach (string authorId in viewModel.Author)
                    {
                        var author = this.unitOfWork.AuthorRepository.Find(Guid.Parse(authorId));

                        var bookAuthorModel = new BookAuthor()
                        {
                            Author = author,
                            Book = model
                        };

                        this.unitOfWork.BookAuthorRepository.Insert(bookAuthorModel);
                    }

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Book");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(BookViewModel viewModel, IFormFile file,string nameImage)
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

                        // Con este formato se guarda en el file system.
                        var filePath = Path.Combine(fileDestinationPath, nameImage);

                        // Con este formato se guarda en la base de datos.
                        var imagePath = FILEPATH + "/" + nameImage;

                        viewModel.ImagePath = imagePath;

                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }
                        }
                    }

                    var model = this.unitOfWork.BookRepository.FindById("Editorial,Category,Subcategory,BookCollection,Bookbinding", Book => Book.ID == viewModel.ID);

                    model.ISBN = viewModel.ISBN;

                    model.Title = viewModel.Title;

                    model.Subtitle = viewModel.Subtitle;                   

                    model.Editorial = this.unitOfWork.EditorialRepository.Find(Guid.Parse(viewModel.Editorial));

                    model.Category = this.unitOfWork.CategoryRepository.Find(Guid.Parse(viewModel.Category));

                    model.Subcategory = this.unitOfWork.SubcategoryRepository.Find(Guid.Parse(viewModel.Subcategory));

                    model.Weight = viewModel.Weight;

                    model.Recommended = viewModel.Recommended;

                    model.Description = viewModel.Description;

                    model.BookCollection = this.unitOfWork.BookCollectionRepository.Find(Guid.Parse(viewModel.BookCollection));

                    model.Pages = viewModel.Pages;

                    model.Content = viewModel.Content;

                    model.Bookbinding = this.unitOfWork.BookbindingRepository.Find(Guid.Parse(viewModel.Bookbinding));

                    model.ImagePath = viewModel.ImagePath;

                    // Lógica del update de autores.

                    // Encuentra los autores que pertenecen a un libro dado.
                    var authorExist = this.unitOfWork.BookAuthorRepository.GetAuthorsById(model.ID);
                    
                    foreach (string authorId in viewModel.Author)
                    {
                        var author = this.unitOfWork.AuthorRepository.Find(Guid.Parse(authorId));                        

                        var bookAuthorModel = new BookAuthor()
                        {
                            Author = author,

                            Book = model
                        };

                        // Si el libro no tiene asignado este autor, entonces se inserta.
                        if (! authorExist.Contains(author))
                        {
                            this.unitOfWork.BookAuthorRepository.Insert(bookAuthorModel);
                        }
                    }

                    foreach (Author author in authorExist)
                    {

                        // Si un autor no esta en la lista que vuelve de la view entonces se elimina.
                        if (!viewModel.Author.Contains(author.ID.ToString()))
                        {
                            var bookAuthor = this.unitOfWork.BookAuthorRepository.FindById("Author,Book", BookAuthor => BookAuthor.BookId == model.ID && BookAuthor.AuthorId == author.ID);

                            this.unitOfWork.BookAuthorRepository.Delete(bookAuthor);
                        }

                    }

                    this.unitOfWork.BookRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Book");
                }

                return View();
            }
            catch (DataException data)
            {
                ModelState.AddModelError(data.Message, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");

                return View();
            }
        }

        public ActionResult Delete(Guid? ID)
        {
            try
            {
                Book book = this.unitOfWork.BookRepository.FindById("Editorial,Category,Subcategory,BookCollection,Bookbinding", Book => Book.ID == Guid.Parse(ID.ToString()));

                BookAuthor bookAuthor = this.unitOfWork.BookAuthorRepository.FindById("Book,Author", Book => Book.BookId == Guid.Parse(ID.ToString()));

                this.unitOfWork.BookAuthorRepository.Delete(bookAuthor);

                this.unitOfWork.BookRepository.Delete(book);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }

            return RedirectToAction("List", "Book");
        }
    }
}