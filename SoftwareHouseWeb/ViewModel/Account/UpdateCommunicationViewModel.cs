using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareHouseWeb.ViewModel.Account
{
    public class UpdateCommunicationViewModel
    {
        public int id { get; set; }
        [Required]
        public string FirstPreferences { get; set; }
        [Required]
        public string SecondPreferences { get; set; }

        [Required]
        public TimeSpan FirstPreferedTimeStart { get; set; }
        [Required]
        public TimeSpan FirstPreferedTimeEnd { get; set; }


        public TimeSpan SecondPreferedTimeStart { get; set; }
        public TimeSpan SecondPreferedTimeEnd { get; set; }
    }
}
