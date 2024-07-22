using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Vi_EnegryMeter.ViewModels.EM_Master
{
    public class EM_Master_ViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Energy Meter Name")]
        [Display(Name = "Name")]
        [Remote(action: "IsNameExists", controller: "EM_Master", AdditionalFields = "Id")]
        public string EM_Name { get; set; }
        [Required(ErrorMessage = "Enter Energy Meter Make")]
        [Display(Name = "Make")]
        public string EM_Make { get; set; }
        [Required(ErrorMessage = "Enter Energy Meter Model")]
        [Display(Name = "Model")]
        public string EM_Model { get; set; }
        [Required(ErrorMessage = "Enter Energy Supplier Name")]
        [Display(Name = "Supplier Name")]
        public string EM_Supplier { get; set; }

        [Required(ErrorMessage = "Enter Energy Serial Number")]
        [Display(Name = "Serial Number")]
        public string EM_SN { get; set; }

        [Required(ErrorMessage = "Enter Energy Meter IP Address")]
        [RegularExpression("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", ErrorMessage = "Enter Valid IP Address")]
        [Remote(action: "IsIpExists", controller: "EM_Master", AdditionalFields = "Id")]
        public string IP { get; set; }
        public DateTime DOP { get; set; }
        public bool ISactive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CalibrationDate { get; set; } 
        
    }
}
