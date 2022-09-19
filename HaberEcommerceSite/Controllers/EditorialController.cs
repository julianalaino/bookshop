using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HaberEcommerceSite.Controllers
{
    [Authorize]
    public class EditorialController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public EditorialController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<Editorial> listEditorial = null;

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
                // Busqueda de Editoriales con filtro en cuadro de busqueda.
                listEditorial = unitOfWork.EditorialRepository.GetQuery("", editorial => editorial.Description.Contains(searchString)).ToList();
            }
            else
            {
                listEditorial = unitOfWork.EditorialRepository.GetQuery("").ToList();
                // Se saca de la lista la editorial con nombre "Sin Editorial".
                listEditorial.Remove(listEditorial.First());
            }

          
            listEditorial.Sort((x, y) => string.Compare(x.Description, y.Description));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Editorial>.Create(listEditorial, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Editorial editorial)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.EditorialRepository.Insert(editorial);

                this.unitOfWork.Save();

                return RedirectToAction("List", "Editorial");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Editorial editorial = this.unitOfWork.EditorialRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(editorial);
        }

        [HttpPost]
        public ActionResult Edit(Editorial editorialModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var model = this.unitOfWork.EditorialRepository.FindById("", Editorial => Editorial.ID == editorialModel.ID);

                    model.Description = editorialModel.Description;

                    this.unitOfWork.EditorialRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Editorial");
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
                Editorial editorial = this.unitOfWork.EditorialRepository.FindById("", Editorial => Editorial.ID == Guid.Parse(id.ToString()));

                this.unitOfWork.EditorialRepository.Delete(editorial);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Editorial");
        }
    }
}