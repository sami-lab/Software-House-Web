using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Portfolio
{
    public class Portfolio
    {
        [Key]
        public int id { get; set; }
        public int Ser_Id { get; set; }
        [Required]
        //[Column(TypeName = "Date")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.DateTime)]
        public System.DateTime created_At { get; set; }
        public string PhotoPath { get; set; }
        public string Heading { get; set; }
        public string Desc { get; set; }

        [ForeignKey("Ser_Id")]
        public Services.OurServices Service_Model { get; set; }
    }
}
