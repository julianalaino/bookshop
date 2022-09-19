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
    public class PromoRepository : Repository<Promo>
    {

        private HaberContext databaseContext;

        private DbSet<Promo> databaseSet;

        

        public PromoRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<Promo>();
        }
        
        public  virtual IQueryable<Promo> GetQuerySearch(string searchString,string includeProperties, Expression<Func<Promo, bool>> filter = null)
        {

            var query =
            from promo in databaseContext.Promos.AsQueryable()
             join bookPromo in databaseContext.BooksPromos.AsQueryable() on promo.ID equals bookPromo.PromoId join Book in databaseContext.Books on bookPromo.BookId equals Book.ID
             where Book.Title.Contains(searchString) || Book.BookCollection.Description.Contains(searchString) || Book.Category.Description.Contains(searchString) || Book.Editorial.Description.Contains(searchString) || Book.Subtitle.Contains(searchString) || Book.ISBN.ToString().Contains(searchString)
             || promo.Title.Contains(searchString) select promo;
           
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
         

            return query;

        }
    }

    
}
