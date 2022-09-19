using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HaberEcommerceSite.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthorController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 12;

            List<Author> listAuthor = null;

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
                //Busqueda de Autores con filtro en cuadro de busqueda
                listAuthor = unitOfWork.AuthorRepository.GetQuery("", author => author.Name.Contains(searchString)).ToList();
            }
            else
            {
                listAuthor = unitOfWork.AuthorRepository.GetQuery("").ToList();
                //Se elimina el autor con nombre 'Sin autor'
                listAuthor.Remove(listAuthor.First());
            }
            
            

            listAuthor.Sort((x, y) => string.Compare(x.Name, y.Name));

            ViewData["CurrentFilter"] = searchString;

            return View(PaginatedList<Author>.Create(listAuthor, pageIndex ?? 1, pageSize));
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if(ModelState.IsValid)
            {
                this.unitOfWork.AuthorRepository.Insert(author);

                this.unitOfWork.Save();

                return RedirectToAction("List", "Author");
            }

            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            Author author = this.unitOfWork.AuthorRepository.FindById("", entity => entity.ID == Guid.Parse(ID.ToString()));

            return View(author);
        }

        [HttpPost]
        public ActionResult Edit(Author authorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {                    

                    var model = this.unitOfWork.AuthorRepository.FindById("", Author => Author.ID == authorModel.ID);

                    model.Name = authorModel.Name;

                    this.unitOfWork.AuthorRepository.Update(model);

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "Author");
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
                Author author = this.unitOfWork.AuthorRepository.FindById("", Author => Author.ID == Guid.Parse(id.ToString()));
               
                this.unitOfWork.AuthorRepository.Delete(author);

                this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "Author");
        }
    }
}