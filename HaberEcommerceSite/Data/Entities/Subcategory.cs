using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.Data.Entities
{
    public class Subcategory
	{
		[Key]
		public Guid ID { get; set; }

		public Category Category { get; set; }

		[Required]
        [Display(Name = "Descripción")]
        public String Description { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}
