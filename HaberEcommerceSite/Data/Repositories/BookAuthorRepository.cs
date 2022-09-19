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
    public class BookAuthorRepository : Repository<BookAuthor>
    {

        private HaberContext databaseContext;

        private DbSet<Book> databaseSet;

        

        public BookAuthorRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<Book>();
        }
        
      

       public virtual IQueryable<BookAuthor> FindListById(string includeProperties, Expression<Func<BookAuthor, bool>> filter = null)
        {

            var query =
            from book in databaseContext.Books.AsQueryable()
            join bookAuthor in databaseContext.BooksAuthors.AsQueryable() on book.ID equals bookAuthor.BookId
            join author in databaseContext.Authors on bookAuthor.AuthorId equals author.ID 
            select bookAuthor;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;

        }

        public virtual List<Author> GetAuthorsById(Guid ID)
        {


            var query =  from bookAuthor in databaseContext.BooksAuthors.AsQueryable() 
            join author in databaseContext.Authors on bookAuthor.AuthorId equals author.ID
            where bookAuthor.BookId==ID
            select author;        
         
            return query.ToList();

        }
    }

    
}
