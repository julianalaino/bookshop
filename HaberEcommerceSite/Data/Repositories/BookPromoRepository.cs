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
    public class BookPromoRepository : Repository<BookPromo>
    {

        private HaberContext databaseContext;

        private DbSet<Book> databaseSet;

        

        public BookPromoRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;
            this.databaseSet = context.Set<Book>();
        }
        
      

       public virtual IQueryable<BookPromo> FindListById(string includeProperties, Expression<Func<BookPromo, bool>> filter = null)
        {

            var query =
            from book in databaseContext.Books.AsQueryable()
            join bookPromo in databaseContext.BooksPromos.AsQueryable() on book.ID equals bookPromo.BookId
            join Promo in databaseContext.Promos on bookPromo.PromoId equals Promo.ID 
            select bookPromo;

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

        public virtual List<Book> GetBooksById(Guid ID)
        {


            var query =  from bookPromo in databaseContext.BooksPromos.AsQueryable() 
            join Book in databaseContext.Books on bookPromo.BookId equals Book.ID
            where bookPromo.PromoId==ID
            select Book;        
         
            return query.ToList();

        }
    }

    
}
