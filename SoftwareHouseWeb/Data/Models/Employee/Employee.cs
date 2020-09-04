using SoftwareHouseWeb.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Employee
{
    public class Employee
    {
        [Key]
        public int Employee_id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Phone*")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Position { get; set; }
        [Required]
        public Time Time { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> EndDate { get; set; }

        public string Skills { get; set; }
        [Required]
        public double Salary { get; set; }
        [Display(Name = "Age")]
        public int age { get; set; }

        public bool is_active { get; set; }

        public ICollection<OrderTeam> OrderTeams { get; set; }
    }
    public enum Time
    {
        Intern,
        PartTime,
        FullTime
    }
}
