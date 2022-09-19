using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Entities
{
    public class Promo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Promo")]
        public IList<BookPromo> BooksPromos { get; set; }

        public String ImagePath { get; set; }
    }
}
