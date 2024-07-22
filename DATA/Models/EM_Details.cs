using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATA.Models
{
    [Table("tbl_EM_Details")]
    public class EM_Details
    {
        [Key]
        public int Id { get; set; }
        public int EM_Id { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Sys_Voltage { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Sys_Curr { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal VL1_L2 { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal VL2_L3 { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal VL3_L1 { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Frequency { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal ActivePower { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PowerFactor { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Act_Imp_Energy { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal NoOfInttruption { get; set; }
        public DateTime TS { get; set; }
        public DateTime TS_hours { get; set; }

        [ForeignKey("EM_Id")]
        public EM_Master EM_Master { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal KVah { get; set; }
    }
}
