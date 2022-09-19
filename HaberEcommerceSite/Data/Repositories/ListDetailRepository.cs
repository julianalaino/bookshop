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
    public class ListDetailRepository : Repository<ListDetail>
    {

        private HaberContext databaseContext;

        private DbSet<ListDetail> databaseSet;

        

        public ListDetailRepository(HaberContext context) : base(context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<ListDetail>();
        }
       

        public virtual float GetQueryBooks(Guid bookID)
        {

            var query =
            from ListDetail listDetail in databaseContext.ListDetail.AsQueryable()
            join PriceList priceList in databaseContext.PriceList.AsQueryable() on listDetail.PriceList.ID equals priceList.ID
            where listDetail.Book.ID==bookID
            orderby priceList.ValidityDate descending
            select  listDetail.Price;

            if (query.Any())
            {
                return query.First();
            }
            else return 0;

        }

        public virtual List<ListDetail> FindListByPriceListId( Guid priceListID)
        {

            var query = from PriceList in databaseContext.PriceList.AsQueryable()
                        join listDetail in databaseContext.ListDetail.AsQueryable() on PriceList.ID equals listDetail.PriceList.ID
                        where PriceList.ID == priceListID
                        select listDetail;         
            return query.ToList();

        }



    }

    
}
