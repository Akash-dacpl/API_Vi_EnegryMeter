using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.ViewModels
{
    public class EM_DetailsViewModel
    {
        public int srno { get; set; }
        public int EM_Id { get; set; }
        public string EM_Name { get; set; }
        public decimal Sys_Voltage { get; set; }
        public decimal Sys_Curr { get; set; }
        public decimal VL1_L2 { get; set; }
        public decimal VL2_L3 { get; set; }
        public decimal VL3_L1 { get; set; }
        public decimal Frequency { get; set; }
        public decimal ActivePower { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal Act_Imp_Energy { get; set; }
        public decimal NoOfInttruption { get; set; }
        public DateTime TS { get; set; }
        public DateTime TS_hours { get; set; }
        public bool IsAlive { get; set; }
        public DateTime? LastUpdate { get; set; }

        public decimal Kvah { get; set; }

        public decimal FirstReading { get; set; }
        public decimal LastReading { get; set; }
        public decimal MinActivePower { get; set; }
        public decimal MaxActivePower { get; set; }
        //  public List<EM_DetailsViewModel> EM_lst { get; set; }
    }
}
 