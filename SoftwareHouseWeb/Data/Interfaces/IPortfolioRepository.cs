using SoftwareHouseWeb.ViewModel.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IPortfolioRepository
    {
        List<PortfolioViewModel> GetDetails();
        List<PortfolioViewModel> ServicePortfolio(int Ser_Id);
        PortfolioViewModel GetDetail(int id);
        int addPortfolio(PortfolioViewModel c);
        int Update(int id, PortfolioViewModel emp);
        bool delete(int id);
    }
}
