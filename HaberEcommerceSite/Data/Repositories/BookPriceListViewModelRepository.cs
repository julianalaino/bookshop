using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using HaberEcommerceSite.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Repositories
{
    public class BookPriceListViewModelRepository : Repository<BookPriceListViewModel>
    {

        private HaberContext databaseContext;

        private DbSet<BookPriceListViewModel> databaseSet;



        public BookPriceListViewModelRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<BookPriceListViewModel>();
        }




        public virtual List<BookPriceListViewModel> GetBookOrdered()
        {

            List<BookPriceListViewModel> books = new List<BookPriceListViewModel>();



            var query = from B in databaseContext.Books.AsQueryable()
                        join LD in databaseContext.ListDetail.AsQueryable() on new
                        {
                            ID = B.ID,
                            Column1 =
                         ((from PriceList in databaseContext.PriceList
                           join listDetail in databaseContext.ListDetail
                            on PriceList.ID equals listDetail.PriceList.ID into listDetail_join
                           from listDetail in listDetail_join.DefaultIfEmpty()
                           orderby
                            PriceList.ValidityDate descending
                           select PriceList.ID).FirstOrDefault())

                        }
                        equals new {ID=LD.Book.ID,Column1=LD.PriceList.ID }
                        into LD_join
                        from LD in LD_join.DefaultIfEmpty() 
                        orderby

                            B.Editorial.Description,
                                  ((from BA in databaseContext.BooksAuthors
                                    join A in databaseContext.Authors
                                          on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                                      equals new { AuthorID = A.ID, BookId = B.ID }
                                    orderby
                                      A.Name
                                    select A.Name).FirstOrDefault()),
                                    B.Title


                        select new
                        {

                            Editorial = B.Editorial.Description,
                            Author =
                 ((from BA in databaseContext.BooksAuthors
                   join A in databaseContext.Authors
                    on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                equals new { AuthorID = A.ID, BookId = B.ID }
                   orderby
                A.Name
                   select A.Name).FirstOrDefault()),
                            Title = B.Title,
                            currentPrice = LD != null ? LD.Price : 0,
                            ID = B.ID

                        };


            foreach (var book in query)
            {

                var bookPriceListViewModel = new BookPriceListViewModel()
                {
                    Author = book.Author,
                    Editorial = book.Editorial,
                    currentPrice = book.currentPrice,
                    Title = book.Title,
                    BookID = book.ID


                };
                books.Add(bookPriceListViewModel);

            }

            return books;

        }

        public virtual List<BookPriceListViewModel> GetBookOrderedByEditorial(Guid editorial)
        {

            List<BookPriceListViewModel> books = new List<BookPriceListViewModel>();



            var query = from B in databaseContext.Books.AsQueryable()
                        join LD in databaseContext.ListDetail.AsQueryable() on new
                        {
                            ID = B.ID,
                            Column1 =
                         ((from PriceList in databaseContext.PriceList
                           join listDetail in databaseContext.ListDetail
                            on PriceList.ID equals listDetail.PriceList.ID into listDetail_join
                           from listDetail in listDetail_join.DefaultIfEmpty()
                           orderby
                            PriceList.ValidityDate descending
                           select PriceList.ID).FirstOrDefault())

                        }
                        equals new { ID = LD.Book.ID, Column1 = LD.PriceList.ID }
                        into LD_join
                        from LD in LD_join.DefaultIfEmpty()
                        where B.Editorial.ID==editorial
                        orderby

                            B.Editorial.Description,
                                  ((from BA in databaseContext.BooksAuthors
                                    join A in databaseContext.Authors
                                          on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                                      equals new { AuthorID = A.ID, BookId = B.ID }
                                    orderby
                                      A.Name
                                    select A.Name).FirstOrDefault()),
                                    B.Title


                        select new
                        {

                            Editorial = B.Editorial.Description,
                            Author =
                 ((from BA in databaseContext.BooksAuthors
                   join A in databaseContext.Authors
                    on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                equals new { AuthorID = A.ID, BookId = B.ID }
                   orderby
                A.Name
                   select A.Name).FirstOrDefault()),
                            Title = B.Title,
                            currentPrice = LD != null ? LD.Price : 0,
                            ID = B.ID

                        };


            foreach (var book in query)
            {

                var bookPriceListViewModel = new BookPriceListViewModel()
                {
                    Author = book.Author,
                    Editorial = book.Editorial,
                    currentPrice = book.currentPrice,
                    Title = book.Title,
                    BookID = book.ID


                };
                books.Add(bookPriceListViewModel);

            }

            return books;

        }



        public virtual List<BookPriceListViewModel> FindListById(Guid IDPriceList)
        {
            List<BookPriceListViewModel> books = new List<BookPriceListViewModel>();



            var query = from B in databaseContext.Books.AsQueryable()
                        join LD in databaseContext.ListDetail.AsQueryable() on B.ID equals LD.Book.ID into LD_join
                        from LD in LD_join.DefaultIfEmpty()
                        join PL in databaseContext.PriceList.AsQueryable()

                           on new
                           {
                               ID = LD.PriceList.ID,
                               Column1 =
                         ((from PriceList in databaseContext.PriceList
                           where PriceList.ID == IDPriceList
                           orderby
                            PriceList.ValidityDate descending
                           select PriceList.ID).FirstOrDefault())

                           }
                       equals new { PL.ID, Column1 = PL.ID } into PL_join
                        from PL in PL_join.DefaultIfEmpty()
                        where PL.ID == IDPriceList
                        orderby

                            B.Editorial.Description,
                                  ((from BA in databaseContext.BooksAuthors
                                    join A in databaseContext.Authors
                                          on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                                      equals new { AuthorID = A.ID, BookId = B.ID }
                                    orderby
                                      A.Name
                                    select A.Name).FirstOrDefault()),
                                    B.Title


                        select new
                        {

                            Editorial = B.Editorial.Description,
                            Author =
                 ((from BA in databaseContext.BooksAuthors
                   join A in databaseContext.Authors
                    on new { AuthorID = BA.AuthorId, BookId = BA.BookId }
                equals new { AuthorID = A.ID, BookId = B.ID }
                   orderby
                A.Name
                   select A.Name).FirstOrDefault()),
                            Title = B.Title,
                            currentPrice = LD != null ? LD.Price : 0,
                            ID = B.ID,
                            ListDetailID=LD.ID
                            
                        };


            foreach (var book in query)
            {

                var bookPriceListViewModel = new BookPriceListViewModel()
                {
                    Author = book.Author,
                    Editorial = book.Editorial,
                    currentPrice = book.currentPrice,
                    Title = book.Title,
                    BookID = book.ID,
                    ListDetailID=book.ListDetailID

                };
                books.Add(bookPriceListViewModel);

            }
            return books;
        }


        public float GetPriceByBook(Guid ID) {

            var query = from PriceList in databaseContext.PriceList.AsQueryable() join
                        ListDetail in databaseContext.ListDetail.AsQueryable() on PriceList.ID equals
                        ListDetail.PriceList.ID where ListDetail.Book.ID == ID && PriceList.ValidityDate <= DateTime.Today
                        orderby PriceList.ValidityDate descending
                        select ListDetail.Price;

            if (query.Any()) {

                return query.First();
            }

            return 0;

                }

    }
    
}
