using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Review
{
    public class Review
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string User_Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public System.DateTime created_At { get; set; }
        [Required]
        [Range(0,5)]
        public double Rating { get; set; }
        [Required]
        public string Desc { get; set; }

        public ReviewStatus ReviewStaus { get; set; }
        [ForeignKey("User_Id")]
        public  ApplicationUser User { get; set; }
    }
    public enum ReviewStatus
    {
        Approve,
        WaitingForApproval
    }
}
