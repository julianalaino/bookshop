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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<Category> listCategory = null;

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
                // Busqueda de categorias con filtro en cuadro de busqueda.
                listCategory = unitOfWork.CategoryRepository.GetQuery("", category => category.Description.Contains(searchString)).ToList();
            }
            else
            {
                listCategory = unitOfWork.CategoryRepository.GetQuery("").ToList();
                // Se saca de la lista la categoria con nombre "Sin categoria".
                listCategory.Remove(listCategory.First());
            }

            


            listCategory.Sort((x, y) => string.Compare(x.Description, y.Description));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Category>.Create(listCategory, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.CategoryRepository.Insert(category);

                this.unitOfWork.Save();

                return RedirectToAction("List", "Category");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Category category = this.unitOfWork.CategoryRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category categoryModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var model = this.unitOfWork.CategoryRepository.FindById("", Category => Category.ID == categoryModel.ID);

                    model.Description = categoryModel.Description;

                    this.unitOfWork.CategoryRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Category");
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
                Category category = this.unitOfWork.CategoryRepository.FindById("", Category => Category.ID == Guid.Parse(id.ToString()));

                this.unitOfWork.CategoryRepository.Delete(category);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Category");
        }
    }
}