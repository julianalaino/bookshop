using HaberEcommerceSite.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.ViewModels
{
    public class PriceListViewModel
    {
        public Guid ID { get; set; }    

        [Display(Name = "Observación")]
        public string Observation { get; set; }       

        public List<BookPriceListViewModel> Books { get; set; }

        [Required]
        [Display(Name = "Fecha de Vigencia")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime ValidityDate { get; set; }

        [Required]
        public string Editorial { get; set; }

        public SelectList Editorials { get; set; }
    }
}
