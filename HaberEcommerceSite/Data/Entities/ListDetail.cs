using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Entities
{
    public class ListDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public Book Book { get; set; }

        [Required]
        public float Price { get; set; }

        public PriceList PriceList { get; set; }
    }
}
