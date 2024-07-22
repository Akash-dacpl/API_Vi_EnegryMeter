using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DATA.Models
{
    [Table("tbl_Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "User Name")]
        [Required]
        //[Remote(action: "UserAlreadyExist", controller: "Account")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public long? Mobile { get; set; }
        [Required]
        public string Password { get; set; }

        public bool Isactive { get; set; }
    }
}
