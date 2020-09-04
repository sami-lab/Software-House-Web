using SoftwareHouseWeb.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        int AddEmployee(RegisterEmployeeViewModel model);
        List<RegisterEmployeeViewModel> ListEmployees();
        List<RegisterEmployeeViewModel> ListInterns();

        RegisterEmployeeViewModel Employee(int Employee_id);
        bool delete(int Employee_id);
    }
}
