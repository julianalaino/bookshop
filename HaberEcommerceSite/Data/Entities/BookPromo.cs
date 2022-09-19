using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Entities
{
    public class BookPromo
    {

        public Guid PromoId { get; set; }
        public Promo Promo { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
