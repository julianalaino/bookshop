using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaberEcommerceSite.Data.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Display(Name = "Subtítulo")]
        public string Subtitle { get; set; }

        [Required]
        [Display(Name = "Autor/es")]
        public IList<BookAuthor> BooksAuthors { get; set; }
        // public ICollection<BookAuthor> BooksAuthors { get; }= new List<BookAuthor>();

        
        [Display(Name = "Promo")]
        public IList<BookPromo> BooksPromos { get; set; }

        [Required]
        public Editorial Editorial { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Subcategoría")]
        public Subcategory Subcategory { get; set; }
        
        [Required]
        [Display(Name = "Peso")]
        public Decimal Weight { get; set; }

       
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Collección")]
        public BookCollection BookCollection { get; set; }

        [Required]
        [Display(Name = "Páginas")]
        public int Pages { get; set; }

        
        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Encuadernación")]
        public Bookbinding Bookbinding { get; set; }
       
        [Display(Name = "Recomendado")]
        public bool Recommended { get; set; }

        public string ImagePath { get; set; }
    }
}
