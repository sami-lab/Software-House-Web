using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Home
{
    public class CombineData
    {
        public List<PackagesViewModel.PackagesViewModel> Packages { get; set; }
        public ServicesViewModel.ServicesViewModel Services { get; set; }
        public List<Portfolio.PortfolioViewModel> Portfolios { get; set; }
        public List<ReviewViewModel.ReviewViewModel> Reviews { get; set; } 
    }
}
