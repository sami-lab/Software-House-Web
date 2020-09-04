using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Models.Employee;
using SoftwareHouseWeb.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Administration
{
    public class RegisterEmployeeViewModel
    {

        public int Employee_id { get; set; }

        [Required]
        [Display(Name = "Name*")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
     
        public string Email { get; set; }

        [Display(Name = "Phone*")]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Must be Valid Phone No")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Position { get; set; }
        [Required]
        public Time Time { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public string Skill1 { get; set; }
        public string Skill2 { get; set; }
        public string Skill3 { get; set; }
        public string Skill4 { get; set; }
        public string Skills { get; set; }
        [Required]
        public double Salary { get; set; }
        [Display(Name = "Age")]
        public int age { get; set; }

    }
}
