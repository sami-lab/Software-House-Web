using SoftwareHouseWeb.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.OrderViewModel
{
    public class UpdateOrderStatus
    {
        [Display(Name = "Order ID")]
        public int order_id { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus orderStatus { get; set; }
    }
}
