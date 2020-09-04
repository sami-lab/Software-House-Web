using Microsoft.AspNetCore.Mvc;
using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.ViewModel.PackagesViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.OrderViewModel
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderPackages =new  List<OrderPackagesViewModel>();
            OrderTeam = new List<OrderTeamViewModel>();
            ServicesPackages = new List<GroupByServices>(); 
        }
        [Display(Name = "Order ID")]
        public int order_id { get; set; }

        public string Encryptedorder_id { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime Date { get; set; }

        public int countOrder { get; set; }

        #region User's Data
        public int cus_id { get; set; }
        [Display(Name = "Customer Name")]
        [Required]
        public string cus_name { get; set; }
        [Display(Name = "Country Name")]
        public string Country { get; set; }
        [Display(Name = "Customer Phone")]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Must be Phone No")]
        [Required]
        public string cus_phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstPreferences { get; set; }
        [Required]
        public string SecondPreferences { get; set; }

        [Required]
        public TimeSpan FirstPreferedTimeStart { get; set; }
        [Required]
        public TimeSpan FirstPreferedTimeEnd { get; set; }

        public TimeSpan SecondPreferedTimeStart { get; set; }
        public TimeSpan SecondPreferedTimeEnd { get; set; } 
        #endregion

        [Display(Name = "Payment Method")]
        [Required]
        public PaymentMethods PaymentMethod { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        public string Message { get; set; }
        public List<OrderPackagesViewModel> OrderPackages { get; set; }


        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Expected End Date")]
        public DateTime EndDate { get; set; }

        public List<OrderTeamViewModel> OrderTeam { get; set; }
        public double TotalAmount { get; set; }
        public string ChargeID { get; set; }

        public List<GroupByServices> ServicesPackages { get; set; }
        //[Remote(action: "CheckPromo", controller: "Promo")]
        public string PromoCode { get; set; }
        public string HoldPackage { get; set; }
    }
    public class OrderPackagesViewModel
    {
        public int id { get; set; }
        public int Ser_Id { get; set; }
        public int pkg_Id { get; set; }
        [Display(Name = "Package Name")]
        public string PkgName { get; set; }
        [Display(Name = "Price")]
        public double price { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public int order_id { get; set; }
        //public bool isSelected { get; set; }
    }

    public class OrderTeamViewModel
    {
      
        public int id { get; set; }

        public int order_id { get; set; }
        public int emp_id { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }

        //public Employee.Employee Employee { get; set; }
    }
}
