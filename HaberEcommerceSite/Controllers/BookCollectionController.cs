using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace HaberEcommerceSite.Controllers
{
    public class BookCollectionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BookCollectionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<BookCollection> listBookCollection = null;

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
                //Busqueda de colecciones con filtro en cuadro de busqueda
                listBookCollection = unitOfWork.BookCollectionRepository.GetQuery("", bookCollection => bookCollection.Description.Contains(searchString)).ToList();
            }
            else
            {
                listBookCollection = unitOfWork.BookCollectionRepository.GetQuery("").ToList();
                //Se saca de la lista la coleccion con nombre 'Sin coleccion'
                listBookCollection.Remove(listBookCollection.First());
            }

            


            listBookCollection.Sort((x, y) => string.Compare(x.Description, y.Description));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<BookCollection>.Create(listBookCollection, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCollection bookCollection)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.BookCollectionRepository.Insert(bookCollection);

                this.unitOfWork.Save();

                return RedirectToAction("List", "BookCollection");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            BookCollection bookCollection = this.unitOfWork.BookCollectionRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(bookCollection);
        }

        [HttpPost]
        public ActionResult Edit(BookCollection bookCollectionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var model = this.unitOfWork.BookCollectionRepository.FindById("", BookCollection => BookCollection.ID == bookCollectionModel.ID);

                    model.Description = bookCollectionModel.Description;

                    this.unitOfWork.BookCollectionRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "BookCollection");
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
                BookCollection bookCollection = this.unitOfWork.BookCollectionRepository.FindById("", BookCollection => BookCollection.ID == Guid.Parse(id.ToString()));

                this.unitOfWork.BookCollectionRepository.Delete(bookCollection);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "BookCollection");
        }
    }
}