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

        //public TEntity Create<TEntity>() where TEntity : INbEntity<Guid>
        //{
        //    var issue = new Issue();
        //    return (TEntity)(object)issue;
        //}

        [Display(Name = "题目")]
        [Required]
        public string Subject { get; set; }

        //[Required]
        //public IssueType IssueType { get; set; }

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

    public class MockData
    {
        public MockData()
        {
            Issues = new List<IssueViewModel>();
            InitIssues();
        }

        private void InitIssues()
        {
            var guids = GuidHelper.CreateMockGuidQueue(10);
            var issueViewModels = new List<IssueViewModel>();
            for (int i = 0; i < 10; i++)
            {
                issueViewModels.Add(new IssueViewModel() { Id = guids.Dequeue(), Subject = i.ToString("0000"), Body = "BODY..."});
            }
            Issues = issueViewModels;
        }

        public IList<IssueViewModel> Issues { get; set; }
    
    }
}