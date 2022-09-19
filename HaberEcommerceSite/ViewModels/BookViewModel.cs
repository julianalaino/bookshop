using HaberEcommerceSite.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.ViewModels
{
    public class BookViewModel
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        
        [Display(Name = "Subtítulo")]
        public string Subtitle { get; set; }

        [Required]
        [Display(Name = "Autor")]
        public List<string> Author { get; set; }

        public MultiSelectList Authors { get; set; }

        [Required]
        [Display(Name = "Editorial")]
        public string Editorial { get; set; }

        public SelectList Editorials { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public string Category { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        [Display(Name = "Subcategoría")]
        public string Subcategory { get; set; }

        public SelectList Subcategories { get; set; }

        
        [Display(Name = "Peso")]
        public Decimal Weight { get; set; }

        
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Colección")]
        public string BookCollection { get; set; }

        public SelectList BookCollections { get; set; }

        
        [Display(Name = "Páginas")]
        public int Pages { get; set; }

        
        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Encuadernación")]
        public string Bookbinding { get; set; }

        [Required]
        [Display(Name = "Recomendado")]
        public bool Recommended { get; set; }

        public SelectList Bookbindings { get; set; }

        [Display(Name = "Carátula")]
        public string ImagePath { get; set; }

        [Display(Name = "Precio Actual")]

        [DisplayFormat(DataFormatString = "{0:#.00}")]
        public float Price { get; set; }
    }
}
