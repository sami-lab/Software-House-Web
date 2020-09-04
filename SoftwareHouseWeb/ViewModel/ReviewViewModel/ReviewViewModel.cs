using SoftwareHouseWeb.Data;
using SoftwareHouseWeb.Data.Models.Review;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.ReviewViewModel
{
    public class ReviewViewModel
    {
        [Display(Name ="ID")]
        public int id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string User_Id { get; set; }
        [Display(Name = "Name")]
        public string UserName { get; set; }
        [Display(Name = "User Photo")]
        public string UserPhoto { get; set; }
        [Display(Name = "Time")]
        [DataType(DataType.DateTime)]
        public System.DateTime created_At { get; set; }
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Desc { get; set; }

        public ReviewStatus reviewStatus { get; set; }
        public ApplicationUser User { get; set; }
    }
}
