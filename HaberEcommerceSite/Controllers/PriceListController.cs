using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.Pagination;
using HaberEcommerceSite.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HaberEcommerceSite.Controllers
{
    public class PriceListController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private readonly IUnitOfWork unitOfWork;        

        public PriceListController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            this.unitOfWork = unitOfWork;
        }

        public ActionResult List(int? pageIndex, string currentFilter, string searchString)
        {
            int pageSize = 3;

            List<PriceList> priceLists = new List<PriceList>();

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
               priceLists = unitOfWork.PriceListRepository.GetQuery("", priceList => priceList.Editorial.Description.ToString().Contains(searchString)).OrderByDescending(priceList=>priceList.ValidityDate).ToList();
                ViewData["CurrentFilter"] = searchString;

            }           

            return View(PaginatedList<PriceList>.Create(priceLists, pageIndex ?? 1, pageSize));

        }

        public ActionResult Create(PriceListViewModel viewModel)
        {
            List<BookPriceListViewModel> listBook = new List<BookPriceListViewModel>();
            string editorial = "";

            if (viewModel.Editorial != null) { 
            listBook = unitOfWork.BookPriceListViewModelRepository.GetBookOrderedByEditorial(Guid.Parse(viewModel.Editorial));
            if (listBook.Count > 0) {
                editorial = listBook.First().Editorial;
            }
            }
            var priceListViewModel = new PriceListViewModel
            {
                Editorial =editorial,
                Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get().OrderBy(e=>e.Description), "ID", "Description"),
                Books = listBook

            };
            return View(priceListViewModel);
        }
  



        public IActionResult GetEditorials(string term)
        {
            var result = this.unitOfWork.EditorialRepository.Get().Select(x=>x.Description).ToArray();
            return Json(result.Where(x =>
                x.StartsWith(term, StringComparison.CurrentCultureIgnoreCase)).ToArray());
        }
      

        [HttpPost]
        public ActionResult CreatePost(PriceListViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var model = new PriceList()
                    {
                        ValidityDate = viewModel.ValidityDate,

                        Observation = viewModel.Observation,

                        Editorial= this.unitOfWork.EditorialRepository.Find(Guid.Parse(viewModel.Editorial.ToString())) 

                       
                    };

                    this.unitOfWork.PriceListRepository.Insert(model);

                    foreach (BookPriceListViewModel bookViewModel in viewModel.Books)
                    {
                        var book = this.unitOfWork.BookRepository.Find(Guid.Parse(bookViewModel.BookID.ToString()));
                      
                        var ListDetailModel = new ListDetail()
                        {
                            Book = book,
                            Price = bookViewModel.currentPrice,
                            PriceList=model
                        };
                        this.unitOfWork.ListDetailRepository.Insert(ListDetailModel);
                     
                    }
                    

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "PriceList");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }
        
       public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PriceList priceList = this.unitOfWork.PriceListRepository.FindById("Editorial",pl=>pl.ID==Guid.Parse(id.ToString()));
            List<BookPriceListViewModel> books = this.unitOfWork.BookPriceListViewModelRepository.FindListById(Guid.Parse(id.ToString()));

            Promo promo = this.unitOfWork.PromoRepository.Find(Guid.Parse(id.ToString()));



            var viewModel = new PriceListViewModel()
            {
                ID = priceList.ID,
                Observation=priceList.Observation,
                ValidityDate=priceList.ValidityDate,
                Editorial = priceList.Editorial.ID.ToString() ,
                Editorials = new SelectList(this.unitOfWork.EditorialRepository.Get().OrderBy(editorial=>editorial.Description), "ID", "Description"),
                Books =books          
               
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(PriceListViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    

                    var model = this.unitOfWork.PriceListRepository.Find(viewModel.ID);

                    model.Observation = viewModel.Observation;

                    model.ValidityDate = viewModel.ValidityDate;

                    model.Editorial = this.unitOfWork.EditorialRepository.Find(Guid.Parse(viewModel.Editorial));


                    foreach (BookPriceListViewModel bookViewModel in viewModel.Books)
                    {                      
                        var listDetail = this.unitOfWork.ListDetailRepository.Find(Guid.Parse(bookViewModel.ListDetailID.ToString()));
                        listDetail.Price = bookViewModel.currentPrice;
                        this.unitOfWork.ListDetailRepository.Update(listDetail);
                    }
                    
                    this.unitOfWork.PriceListRepository.Update(model);                  

                    this.unitOfWork.Save();

                    return RedirectToAction("List", "PriceList");
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
                PriceList priceList = this.unitOfWork.PriceListRepository.Find(Guid.Parse(id.ToString()));

                List<ListDetail> listsDetail = this.unitOfWork.ListDetailRepository.FindListByPriceListId(Guid.Parse(id.ToString()));

                foreach (ListDetail listDetail in listsDetail) {

                    this.unitOfWork.ListDetailRepository.Delete(listDetail);

                }

               this.unitOfWork.PriceListRepository.Delete(priceList);

               this.unitOfWork.Save();
            }
            catch (DataException)
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";

            }

            return RedirectToAction("List", "PriceList");
        }
    }
}