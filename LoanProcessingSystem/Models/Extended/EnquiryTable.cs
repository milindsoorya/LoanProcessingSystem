using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Models
{
    [MetadataType(typeof(EnquiryTableMetadata))]
    public partial class EnquiryTable
    {
    }
    public class EnquiryTableMetadata
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id require")]
        [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "Not in Valid Format")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Contact Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Phone No.")]
        [DataType(DataType.PhoneNumber)]
        public decimal PhoneNo { get; set; }

        [Display(Name = "Property Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select your Preference")]
        public string PropertyType { get; set; }
        [Display(Name = "Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter DateOfBith")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Message")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter your Query")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

    }
}
