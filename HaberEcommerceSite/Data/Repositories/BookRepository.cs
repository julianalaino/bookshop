using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Repositories
{
    public class BookRepository : Repository<Book>
    {

        private HaberContext databaseContext;

        private DbSet<Book> databaseSet;

        

        public BookRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<Book>();
        }
        
        public  virtual IQueryable<Book> GetQuerySearch(string searchString,string includeProperties, Expression<Func<Book, bool>> filter = null)
        {

            var query =
            from book in databaseContext.Books.AsQueryable()
             join bookAuthor in databaseContext.BooksAuthors.AsQueryable() on book.ID equals bookAuthor.BookId join author in databaseContext.Authors on bookAuthor.AuthorId equals author.ID
             where book.Editorial.Description.ToUpper().Contains(searchString.ToUpper()) || book.Title.ToUpper().Contains(searchString.ToUpper()) || book.BookCollection.Description.ToUpper().Contains(searchString.ToUpper()) || book.Category.Description.ToUpper().Contains(searchString.ToUpper()) || book.Editorial.Description.Contains(searchString) || book.Subtitle.ToUpper().Contains(searchString.ToUpper()) || book.ISBN.ToUpper().Contains(searchString.ToUpper())
             || author.Name.ToUpper().Contains(searchString.ToUpper()) select book;
           
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;

        }

        public virtual List<Book> GetQueryBooks( string includeProperties, Expression<Func<Book, bool>> filter = null)
        {

            var query =
            from book in databaseContext.Books.AsQueryable()
            join bookAuthor in databaseContext.BooksAuthors.AsQueryable() on book.ID equals bookAuthor.BookId
            join author in databaseContext.Authors on bookAuthor.AuthorId equals author.ID
            orderby book.Editorial.Description,author.Name,book.Title
            select book;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }


            return query.ToList();

        }       

        
    }

    
}
