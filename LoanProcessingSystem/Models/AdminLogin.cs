using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Models
{  
    public class AdminLogin
    {
        [Key]
        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName is required")]
        public string EmailId { get; set; }
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
       // [Range(5, 10, ErrorMessage = "Password should contain 5 to 10 length")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Designation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Designation required")]
        public string Role { get; set; }
    }
}