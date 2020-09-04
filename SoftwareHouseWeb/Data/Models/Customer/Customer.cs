using SoftwareHouseWeb.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Customer
{
    public class Customer
    {
        [Key]
        public int cus_id { get; set; }
        [Required]
        public string cus_name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNo { get; set; }

        public string User_Id { get; set; }

        [ForeignKey("User_Id")]
        public ApplicationUser User_Model { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
