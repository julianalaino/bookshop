using System;
using System.ComponentModel.DataAnnotations;

namespace HaberEcommerceSite.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
