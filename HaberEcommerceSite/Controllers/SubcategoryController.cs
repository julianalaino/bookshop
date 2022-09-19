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
    public class SubcategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public SubcategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<Subcategory> listSubcategory = null;

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
                //Busqueda de subcategorias con filtro en cuadro de busqueda
                listSubcategory = unitOfWork.SubcategoryRepository.GetQuery("", subcategory => subcategory.Description.Contains(searchString)).ToList();
            }
            else
            {
                listSubcategory = unitOfWork.SubcategoryRepository.GetQuery("").ToList();

                //Se saca de la lista la subcategoria con nombre 'Sin subcategoria'
                listSubcategory.Remove(listSubcategory.First());
            }


            listSubcategory.Sort((x, y) => string.Compare(x.Description, y.Description));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Subcategory>.Create(listSubcategory, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.SubcategoryRepository.Insert(subcategory);

                this.unitOfWork.Save();

                return RedirectToAction("List", "Subcategory");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Subcategory subcategory = this.unitOfWork.SubcategoryRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(subcategory);
        }

        [HttpPost]
        public ActionResult Edit(Subcategory subcategoryModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var model = this.unitOfWork.SubcategoryRepository.FindById("", Subcategory => Subcategory.ID == subcategoryModel.ID);

                    model.Description = subcategoryModel.Description;

                    this.unitOfWork.SubcategoryRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Subcategory");
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
                Subcategory subcategory = this.unitOfWork.SubcategoryRepository.FindById("", Subcategory => Subcategory.ID == Guid.Parse(id.ToString()));

                this.unitOfWork.SubcategoryRepository.Delete(subcategory);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Subcategory");
        }
    }
}