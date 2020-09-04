using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Promos
{
    public class Promo
    {
        public int id { get; set; }
        [Required]
        public int Ser_id { get; set; }
        [Required]
        public string PromoCode { get; set; }
        [Required]
        [Range(1, 99)]
        public double PromoDiscount { get; set; }

        [ForeignKey("Ser_id")]
        public virtual Services.OurServices Services{ get; set;}
    }
}
