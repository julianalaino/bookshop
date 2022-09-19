using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.ViewModels
{
    public class BookPriceListViewModel
    {




        public Guid BookID { get; set; }

        public Guid ListDetailID { get; set; }

        [Display(Name = "Título")]
        public string Title { get; set; }


        [Display(Name = "Subtítulo")]
        public string Subtitle { get; set; }


        [Display(Name = "Autor")]
        public string Author { get; set; }


        [Display(Name = "Editorial")]
        public string Editorial { get; set; }

        [Display(Name = "Precio Actual")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public float currentPrice { get; set; }


    }
}
