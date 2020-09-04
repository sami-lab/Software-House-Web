using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Portfolio
{
    public class PortfolioViewModel
    {
        
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Service Name")]
        public int Ser_Id { get; set; }
        [Display(Name = "Service Name")]
        public string Ser_Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        [DataType(DataType.DateTime)]
        public System.DateTime created_At { get; set; }

        [Display(Name = "Package Image")]
        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }

        [Display(Name = "Heading(optional)")]
        public string Heading { get; set; }
        [Display(Name = "Description(optional)")]
        public string Desc { get; set; }
    }
}
