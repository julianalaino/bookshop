using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data.Entities
{
    public class PriceList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]        
        public DateTime ValidityDate { get; set; }

        [Required]
        public string Observation { get; set; }

        [Required]
        public Editorial Editorial { get; set; }


    }
}
