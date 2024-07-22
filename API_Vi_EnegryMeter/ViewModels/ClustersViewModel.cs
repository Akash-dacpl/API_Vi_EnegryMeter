using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.ViewModels
{
    public class ClustersViewModel
    {
        public int CL_Id { get; set; }
        public string cluster { get; set; }
        public int count { get; set; }
        public int EM_Id { get; set; }
        public string EM_Name { get; set; }
        public decimal Current { get; set; }
        public decimal AverageCurrent { get; set; }
        public decimal PhaseCurrents { get; set; }
        public decimal UnbalanceCurrent { get; set; }
        public decimal Voltage { get; set; }
        public decimal AverageVoltage { get; set; }
        public decimal UnbalanceVoltage { get; set; }
        public decimal Frequency { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal ActivePower { get; set; }
        public decimal ActiveEnergy { get; set; }
        public decimal Percentage_of_load { get; set; }
        public string TimeStamp { get; set; }
        public IEnumerable<ClustersViewModel> getALl { get; set; }
    }
}
