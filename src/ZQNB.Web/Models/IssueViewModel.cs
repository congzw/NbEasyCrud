using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZQNB.Common.Cruds;
using ZQNB.Common.Data;

namespace ZQNB.Web.Models
{
    [DisplayName("问题")]
    public class IssueViewModel : ICrudViewModel
    {
        [HiddenInput]
        [Display(Name = "主键")]
        public Guid Id { get; set; }

        [Display(Name = "题目")]
        [Required]
        public string Subject { get; set; }

        [UIHint("MySelectListItem")]
        [AdditionalMetadata("Tooltip", "The title of the item")]
        [Display(Name = "分配给")]
        public string AssignedToUserID { get; set; }
        
        [UIHint("MySelectListItem")]
        [Display(Name = "分类")]
        public Guid CategoryId { get; set; }
        
        [Display(Name = "内容")]
        public string Body { get; set; }
    }

    public class Issue : INbEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string Subject { get; set; }
        public virtual string AssignedToUserID { get; set; }

        public virtual Guid CategoryId { get; set; }
        public virtual string Body { get; set; }
    }

    public class IssueCategory : INbEntity<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}