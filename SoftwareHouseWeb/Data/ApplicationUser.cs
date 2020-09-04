using Microsoft.AspNetCore.Identity;
using SoftwareHouseWeb.Data.Models.Customer;
using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.Data.Models.Review;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        //[Display(Name = "Alternative Email: (Optional)")]
        //public string Alternative_Email { get; set; }
        //[Display(Name = "Alternative Phone: (Optional)")]
       // public string Alternative_Phone { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "Gender")]
        public Gender gender { get; set; }
        public string Country { get; set; }


        [Display(Name = "User Photo")]
        public string Photopath { get; set; }
       
        public bool InitialPromoUsed { get; set; }
        public Nullable<int> user_communication { get; set; }
        [ForeignKey("user_communication")]
        public UsersCommunication User_Communication { get; set; }

    
       
        public Customer  Customer{ get; set; }
       // public ICollection<Order> User_Orders { get; set; }
        public virtual Review Review_Model { get; set; }

    }

    public class UsersCommunication
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string FirstPreferences { get; set; }
        [Required]
        public string SecondPreferences { get; set; }


        [Required]
        public TimeSpan FirstPreferedTimeStart { get; set; }
        [Required]
        public TimeSpan FirstPreferedTimeEnd{ get; set; }


        public TimeSpan SecondPreferedTimeStart { get; set; }
        public TimeSpan SecondPreferedTimeEnd { get; set; }

        public virtual ApplicationUser User_Model { get; set; }

    }

  
    public enum Gender
    {
        Male,
        Female
    }
   
}
