using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.Data.Entities
{
    public class BookCollection
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
        [Display(Name = "Descripción")]
        public String Description { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}
