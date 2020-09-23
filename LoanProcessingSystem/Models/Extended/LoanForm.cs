using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Models
{
    [MetadataType(typeof(LoanFormMetadata))]
    public partial class LoanForm
    {
    }
    public class LoanFormMetadata
    {
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Require")]
        public string Name { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id require")]
        [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "Not in Valid Format")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Phone No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Phone No.")]
        [DataType(DataType.PhoneNumber)]
        public decimal PhoneNo { get; set; }

        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Display(Name = "Salary(CTC)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Salary")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Property Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select your Preference")]
        public string PropertyType { get; set; }
        [Display(Name = "DOB")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter DateOfBith")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Display(Name = "Photo(Small)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a file")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

    }
}