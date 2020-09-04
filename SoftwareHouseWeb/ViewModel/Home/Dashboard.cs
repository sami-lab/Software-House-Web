using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Home
{
    public class Dashboard
    {
        public int ProjectsCount { get; set; }
        public double NetProfit { get; set; }
        public double NewCustomers { get; set; }
        public double CustomerSatisfication { get; set; }

        public List<Graph> Sales { get; set; }
        public List<Employee> Employees{get; set;}
        //public List<OrderViewModel> Orders { get; set; }
    }
    public class Graph
    {
        public string Day { get; set; }
        public double Amount { get; set; }
    }
    public class Employee
    {
        public string Id { get; set; }
        public string Photo { get; set; }
        public string Position { get; set; }
        public string Name  { get; set; }
    }
}
