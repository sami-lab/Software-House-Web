using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel
{
    public class PromoViewModel
    {
        public int id { get; set; }
        [Required]
        public int Ser_id { get; set; }
        public string ServiceName { get; set; }
        [Required]
        [Display(Name ="Promo Code")]
        public string PromoCode { get; set; }
        [Required]
        [Display(Name = "Promo Discount(Number)")]
        [Range(1,99)]
        public double PromoDiscount { get; set; }
    }
}
