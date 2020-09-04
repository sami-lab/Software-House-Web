using SoftwareHouseWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IPromoRepository
    {
        int AddPromo(PromoViewModel model);
        List<PromoViewModel> ListPromo();

        PromoViewModel GetPromo(int id);
        List<PromoViewModel> GetPromosOfService(int Ser_id);
        PromoViewModel GetInitialPromo(int Ser_id);
        bool GetPromoByCode(string Code);
        int Update(int id,PromoViewModel model);
        bool delete(int id);
    }
}
