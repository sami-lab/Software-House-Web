using SoftwareHouseWeb.ViewModel.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IServicesRepository
    {
        List<ServicesViewModel> GetDetails();
        ServicesViewModel GetDetail(int id);
        int addService(ServicesViewModel c);
        int Update(int id,ServicesViewModel emp);
        bool delete(int id);
    }
}
