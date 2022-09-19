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
    public class BookbindingController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BookbindingController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<Bookbinding> listBookbinding = null;

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
                //Busqueda de encuadernaciones con filtro en cuadro de busqueda
                listBookbinding = unitOfWork.BookbindingRepository.GetQuery("", bookbinding => bookbinding.Description.Contains(searchString)).ToList();
            }
            else
            {
                listBookbinding = unitOfWork.BookbindingRepository.GetQuery("").ToList();
                //Se saca de la lista la encuadernación con nombre 'Sin encuadernación'
                listBookbinding.Remove(listBookbinding.First());
            }

            

            listBookbinding.Sort((x, y) => string.Compare(x.Description, y.Description));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Bookbinding>.Create(listBookbinding, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bookbinding bookbinding)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.BookbindingRepository.Insert(bookbinding);

                this.unitOfWork.Save();

                return RedirectToAction("List", "Bookbinding");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Bookbinding bookbinding = this.unitOfWork.BookbindingRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(bookbinding);
        }

        [HttpPost]
        public ActionResult Edit(Bookbinding bookbindingModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var model = this.unitOfWork.BookbindingRepository.FindById("", Bookbinding => Bookbinding.ID == bookbindingModel.ID);

                    model.Description = bookbindingModel.Description;

                    this.unitOfWork.BookbindingRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Bookbinding");
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
                Bookbinding bookbinding = this.unitOfWork.BookbindingRepository.FindById("", Bookbinding => Bookbinding.ID == Guid.Parse(id.ToString()));

                this.unitOfWork.BookbindingRepository.Delete(bookbinding);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Bookbinding");
        }
    }
}