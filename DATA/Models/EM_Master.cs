using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATA.Models
{
    [Table("tbl_EM_Master")]
    public class EM_Master
    {
        [Key]
        public int Id { get; set; }

        public string EM_Name { get; set; }
        public string EM_Make { get; set; }
        public string EM_Model { get; set; }
        public string EM_Supplier { get; set; }
        public string EM_SN { get; set; }
        public string IP { get; set; }
        public DateTime DOP { get; set; }
        public bool ISactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public bool IsAlive { get; set; }
        public DateTime? CalibrationDate { get; set; }
    }
}
