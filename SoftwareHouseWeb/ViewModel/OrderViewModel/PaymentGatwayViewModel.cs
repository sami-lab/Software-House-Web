using SoftwareHouseWeb.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.OrderViewModel
{
    public class PaymentGatwayViewModel
    {
        [Display(Name = "Order ID")]
        public int order_id { get; set; }

        [Display(Name = "Customer Name")]
        [Required]
        public string cus_name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Customer Phone")]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Must be Phone No")]
        [Required]
        public string cus_phone { get; set; }

        public string Address { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }

        public string CardName { get; set; }
        public string CardNum { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM}")]
        public DateTime ExpiryMonth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy}")]
        public DateTime Year { get; set; }
        public int cvv { get; set; }

        public List<OrderPackagesViewModel> OrderPackages { get; set; }
        public double TotalAmount { get; set; }

        public string StripeToken { get; set; }
        [Display(Name = "Payment Method")]
        [Required]
        public PaymentMethods PaymentMethod { get; set; }


    }
}
