using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.ViewModel.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.PackagesViewModel
{
    public class PackagesViewModel
    {
        public PackagesViewModel()
        {
            Description = new string[] {};
        }
        [Display(Name ="ID")]
        public int id { get; set; }
        public string Encrypted_id { get; set; }
        [Required]
        [Display(Name = "Package Name")]
        public string PkgName { get; set; }
        [Required]
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        [Display(Name = "Service Name")]
        public int Ser_Id { get; set; }
        [Display(Name = "Service Name")]
        public string Ser_Name { get; set; }
        public string Heading { get; set; }
        [Display(Name = "Description")]
        public string Desc { get; set; }
        public string[] Description { get; set; }

        [Display(Name = "Package Image")]
        public string PhotoPath { get; set; }
        public IFormFile Photo { get; set; }
        [Range(0, 100)]
        [Display(Name = "Package Discount(0-100)%")]
        public double DiscountPercent { get; set; }

      
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public System.DateTime LaunchDate { get; set; }
        public bool isActive { get; set; }


      //public ServicesViewModel.ServicesViewModel Services { get; set; }


        //Just for Displaying Packages on Order page
        [Display(Name = "Quantity")]
        [Remote(action: "quantityCheck", controller: "Order")]
        public int Quantity { get; set; }
        
        [Display(Name = "Select")]
        public bool is_selected { get; set; }
    }
}
