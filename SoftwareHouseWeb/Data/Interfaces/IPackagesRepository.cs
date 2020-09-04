using SoftwareHouseWeb.ViewModel.PackagesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IPackagesRepository
    {
        List<PackagesViewModel> GetDetails();
        List<PackagesViewModel> GetPackagesOfService(int Ser_Id);
        PackagesViewModel GetDetail(string id);
        string addPackage(PackagesViewModel c);
        string Update(string id, PackagesViewModel emp);
        bool delete(string id);
    }
}
