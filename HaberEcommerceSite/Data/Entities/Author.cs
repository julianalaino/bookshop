using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberEcommerceSite.Data.Entities
{
	public class Author
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public IList<BookAuthor> BooksAuthors { get; set; }
    }
}
