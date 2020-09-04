using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Promo
{
    public class PromoEmailViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string PromoCode { get; set; }
        public double PromoDiscount { get; set; }
    }
}
