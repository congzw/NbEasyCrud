using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZQNB.Web.Models
{
    public class IssueViewModel : ICrudViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Subject { get; set; }

        //[Required]
        //public IssueType IssueType { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedToUserID { get; set; }

        public string Body { get; set; }
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