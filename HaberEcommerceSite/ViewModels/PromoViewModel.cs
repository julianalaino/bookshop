using HaberEcommerceSite.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.ViewModels
{
    public class PromoViewModel
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:#.00}")]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Libros")]
        public List<string> Book { get; set; }

        public MultiSelectList Books { get; set; }       

        [Display(Name = "Carátula")]
        public string ImagePath { get; set; }
    }
}
