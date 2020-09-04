using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.PackagesViewModel
{
    public class GroupByServices
    {
        [Display(Name = "Service Name")]
        public int? Ser_Id { get; set; }
        [Display(Name = "Service Name")]
        public string Ser_Name { get; set; }
        public List<PackagesViewModel> Packages { get; set; }
    }
}
