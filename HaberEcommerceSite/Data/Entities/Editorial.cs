using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberEcommerceSite.Data.Entities
{
    public class Editorial
	{
		[Key]
		public Guid ID { get; set; }

		[Required]
        [Display(Name = "Descripción")]
        public String Description { get; set; }

        [NotMapped]
        public ICollection<Book> Books { get; set; }		
	}
}
