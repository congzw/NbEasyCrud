using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZQNB.Web.Models
{
    [DisplayName("问题")]
    public class IssueViewModel : ICrudViewModel
    {
        [Display(Name = "主键")]
        public Guid Id { get; set; }
        
        [Display(Name = "题目")]
        [Required]
        public string Subject { get; set; }
        
        [Display(Name = "分配给")]
        public string AssignedToUserID { get; set; }


        [Display(Name = "内容")]
        public string Body { get; set; }
    }

    public class Issue : INbEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AssignedToUserID { get; set; }
        public virtual string Body { get; set; } 
    }
}