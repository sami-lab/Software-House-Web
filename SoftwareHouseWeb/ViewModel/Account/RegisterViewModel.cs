using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Account
{
    public class RegisterViewModel
    {
        public string id { get; set; }
        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailexist", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [Remote(action: "IsUsernameExist", controller: "Account")]
        public string Username { get; set; }

        //[Display(Name = "Alternative Email: (Optional)")]
        //public string Alternative_Email { get; set; }

        [Display(Name = "Phone*")]
        public string PhoneNumber { get; set; }

        //[Display(Name = "Alternative Phone: (Optional)")]
        //public string Alternative_Phone { get; set; }

        [Display(Name = "Gender")]
        public Gender gender { get; set; }
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile Photo { get; set; }
        public string Photopath { get; set; }
    }
}
