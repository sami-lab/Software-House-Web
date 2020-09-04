using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Home
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            IndexView = new List<IndexView>();
            Reviews = new List<ReviewViewModel.ReviewViewModel>();
        }
        public List<IndexView> IndexView { get; set; }
        public List<ReviewViewModel.ReviewViewModel> Reviews { get; set; }

    }
    public class IndexView
    {
        public ServicesViewModel.ServicesViewModel Services { get; set; }
        public PackagesViewModel.PackagesViewModel Packages { get; set; }
        public Portfolio.PortfolioViewModel Portfolios { get; set; }
    }
}
