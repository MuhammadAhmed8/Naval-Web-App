using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [UIHint("Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool Rememberme { get; set; }
    }
}
