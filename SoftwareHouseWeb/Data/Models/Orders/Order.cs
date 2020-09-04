using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Orders
{
    public class Order
    {
        [Key]
        public int order_id { get; set; }

        [Required]
       
        [DataType(DataType.DateTime)]
        public System.DateTime Date { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public int cus_id { get; set; }
        [ForeignKey("cus_id")]
        public Customer.Customer Cus_model { get; set; }

        public double TotalAmount { get; set; }
        public string ChargeID { get; set; }

        public string Message { get; set; }

        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime EndDate { get; set; }

        public ICollection<OrderPackages> OrderPackages { get; set; }
        public ICollection<OrderTeam> OrderTeam { get; set; }

    }
    public class OrderPackages
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int pkg_Id { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public int Quantity { get; set; }

        public int order_id { get; set; }
        [ForeignKey("pkg_Id")]
        public Packages.Packages Packages { get; set; }
        [ForeignKey("order_id")]
        public Order Order { get; set; }
    }
    public class OrderTeam
    {
        [Key]
        public int id { get; set; }
       
        public int order_id { get; set; }
        public int emp_id { get; set; }

        [ForeignKey("order_id")]
        public Order Order { get; set; }

        [ForeignKey("emp_id")]
        public Employee.Employee Employee { get; set; }
    }
    public enum OrderStatus
    {
        Rejected,
        Cancelled,
        Pending,
        Processing,
        Completed
    }
    public enum PaymentStatus
    {
       Paid,
        [Description("Partially Paid")]
        PartiallyPaid,
       Unpaid
    }
    public enum PaymentMethods
    {
       Cash,
       Stripe,
       Others
    }
}
