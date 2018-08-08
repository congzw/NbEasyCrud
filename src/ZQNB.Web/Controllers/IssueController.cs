using System.Collections.Generic;
using System.Linq;
using ZQNB.Common;
using ZQNB.Common.Cruds;
using ZQNB.Common.Data;
using ZQNB.Common.Data._Mock;
using ZQNB.Web.Models;

namespace ZQNB.Web.Controllers
{
    public class IssueController : CrudController<IssueViewModel, Issue>
    {
        #region for test mock

        protected override void ProcessMySelectListItems()
        {
            AddSelectListItemsForProperty(x => x.AssignedToUserID, () =>
            {
                var mySelectListItems = new List<MySelectListItem>();
                var repository = ResolveRepository();
                var entities = repository.Query<IssueUser>().ToList();

                mySelectListItems.Add(new MySelectListItem() { Text = "未指定", Value = string.Empty });

                foreach (var entity in entities)
                {
                    mySelectListItems.Add(new MySelectListItem
                    {
                        Text = entity.Name,
                        Value = entity.Id.ToString()
                    });
                }
                return mySelectListItems;
            });


            AddSelectListItemsForProperty(x => x.CategoryId, () =>
            {
                var mySelectListItems = new List<MySelectListItem>();
                var repository = ResolveRepository();
                var issueCategories = repository.Query<IssueCategory>().ToList();

                mySelectListItems.Add(new MySelectListItem() { Text = "未指定", Value = string.Empty });

                foreach (var issueCategory in issueCategories)
                {
                    mySelectListItems.Add(new MySelectListItem
                    {
                        Text = issueCategory.Name,
                        Value = issueCategory.Id.ToString()
                    });
                }
                return mySelectListItems;
            });
        }

        protected override MessageResult CallAddValidate(IssueViewModel model)
        {
            var messageResult = base.CallAddValidate(model);
            if (!messageResult.Success)
            {
                return messageResult;
            }

            if (model.Subject == "sss")
            {
                messageResult.Success = false;
                messageResult.Message = "不能使用关键sss";
                return messageResult;
            }

            return messageResult;
        }

        protected override MessageResult CallEditValidate(IssueViewModel model)
        {
            var messageResult = base.CallEditValidate(model);
            if (!messageResult.Success)
            {
                return messageResult;
            }

            if (model.Subject == "sss")
            {
                messageResult.Success = false;
                messageResult.Message = "不能使用关键sss";
                return messageResult;
            }

            return messageResult;
        }


        private static ISimpleRepository _simpleRepository;
        protected override ISimpleRepository ResolveRepository()
        {
            if (_simpleRepository == null)
            {
                var memeoryRepository = new MemeoryRepository();

                memeoryRepository.InitFor(() =>
                {
                    //return new List<Issue>();
                    var guids = GuidHelper.CreateMockGuidQueue(10);
                    var issueViewModels = new List<Issue>();
                    for (int i = 1; i <= 10; i++)
                    {
                        issueViewModels.Add(new Issue() { Id = guids.Dequeue(), Subject = i.ToString("0000"), Body = "BODY..." });
                    }
                    return issueViewModels;
                });


                memeoryRepository.InitFor(() =>
                {
                    var guids = GuidHelper.CreateMockGuidQueue(3);
                    var issueCategories = new List<IssueCategory>();
                    for (int i = 1; i <= 3; i++)
                    {
                        issueCategories.Add(new IssueCategory() { Id = guids.Dequeue(), Name = "分类" + i.ToString("0000") });
                    }
                    return issueCategories;
                });



                memeoryRepository.InitFor(() =>
                {
                    var guids = GuidHelper.CreateMockGuidQueue(3);
                    var users = new List<IssueUser>();
                    for (int i = 1; i <= 3; i++)
                    {
                        users.Add(new IssueUser() { Id = guids.Dequeue(), Name = "用户" + i.ToString("0000") });
                    }
                    return users;
                });


                _simpleRepository = memeoryRepository;
            }
            return _simpleRepository;
        }

        #endregion
    }
}