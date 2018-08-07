using System;
using System.ComponentModel.DataAnnotations;

namespace ZQNB.Web.Models
{
    public interface ICrudViewModel
    {
        Guid Id { get; set; }
    }


    public class IssueViewModel : ICrudViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Subject { get; set; }

        //[Required]
        //public IssueType IssueType { get; set; }

        [Required, Display(Name = "Assigned To")]
        public string AssignedToUserID { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
