using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.OrderViewModel
{
    public class SetupTeam
    {
        public SetupTeam()
        {
            Members = new List<Team>();
        }
        public int order_id { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public List<Team> Members { get; set; }
    }
    public class Team
    {
        public int order_id { get; set; }
        public int Employee_id { get; set; }

        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public bool IsSelected { get; set; }
    }
}
