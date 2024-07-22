using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.ViewModels.Account
{
    public class SignUpViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]
        [Remote(action: "UserAlreadyExist", controller: "Account")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter User Email")]
        [Display(Name = "User Email")]
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Enter Valid email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter User Mobile(+91)")]
        [Display(Name = "User Mobile")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Enter valid Mobile no.(+91)")]

        public long? Mobile { get; set; }

        [Required(ErrorMessage = "Enter User Password")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Enter User Confirm Password")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm Password Can't match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Active")]
        public bool Isactive { get; set; }
    }
}
