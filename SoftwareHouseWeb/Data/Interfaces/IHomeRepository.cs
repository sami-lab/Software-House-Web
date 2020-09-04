using SoftwareHouseWeb.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IHomeRepository
    {
        Dashboard Dashboard();
        CombineData combineDataofSErvice(int Ser_Id);
        IndexViewModel IndexData();
        int CustomQuote(CustomQuoteViewModel model);
        List<CustomQuoteViewModel> GetAllCustomQuotes();
    }
}
