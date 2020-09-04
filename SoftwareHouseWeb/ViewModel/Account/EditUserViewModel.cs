using Microsoft.AspNetCore.Http;
using SoftwareHouseWeb.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Account
{
    public class EditUserViewModel
    {
        public string id { get; set; }
        [Required]
        [Display(Name = "Full Name*")]
        public string FullName { get; set; }

        [Display(Name = "Phone*")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender gender { get; set; }

        [Required]
        public string Country { get; set; }

        [Display(Name = "Profile Photo")]
        public IFormFile Photo { get; set; }
        public string Photopath { get; set; }
    }
}
