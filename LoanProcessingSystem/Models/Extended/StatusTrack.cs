using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Models
{
    [MetadataType(typeof(StatusTrackMetadata))]
    public partial class StatusTrack
    {
    }
    public class StatusTrackMetadata
    {
        [Display(Name = "Applicant Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select a Applicant Name")]
        public Nullable<int> ApplicationId { get; set; }
        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select User Name")]
        public Nullable<int> UserId { get; set; }
        [Display(Name = "Inspector Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Inspector Name")]
        public Nullable<int> AuthorityId { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        public string Status { get; set; }


    }
}