using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.ServicesViewModel
{
    public class ServicesViewModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }

        [Display(Name = "Icon Path")]
        public string IconPath { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
