using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.ViewModel.OrderViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.Data.Interfaces
{
    public interface IOrderRepository
    {
        List<OrderViewModel> Details();
        List<OrderViewModel> UserOrders(string userId, bool findstatus);
        List<OrderViewModel> OrdersWithStatus(OrderStatus status);
        Task<int>  Create(OrderViewModel model);
        bool UpdateStatus(int order_id, OrderStatus status);
        OrderViewModel Detail(int order_id);
        OrderViewModel OrderTeam(int order_id);
        bool UpdateTeam(SetupTeam model);
        int Charge(PaymentGatwayViewModel model);

        bool CancelOrder(int id);
        bool Delete(int id);
        int Update(OrderViewModel model);
    }
}
