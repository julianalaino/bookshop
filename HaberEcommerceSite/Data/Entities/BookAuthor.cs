using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Entities
{
    public class BookAuthor
    {
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
