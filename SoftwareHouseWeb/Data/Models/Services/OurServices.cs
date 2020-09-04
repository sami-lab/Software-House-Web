using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Models.Services
{
    public class OurServices
    {
        [Key]
        public int id { get; set; }
        public string ServiceName { get; set; }
        public bool isActive { get; set; }

        public string IconPath { get; set; }


        public ICollection<Packages.Packages> Packages { get; set; }
        public ICollection<Portfolio.Portfolio> Portfolios { get; set; }
        public ICollection<Promos.Promo> Promos { get; set; }
    }
}
