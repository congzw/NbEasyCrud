using System.Collections.Generic;
using ZQNB.Common;
using ZQNB.Common.Data;
using ZQNB.Common.Data._Mock;
using ZQNB.Web.Models;

namespace ZQNB.Web.Controllers
{
    public class IssueController : CrudController<IssueViewModel, Issue>
    {
        #region for test mock
        
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

                var guids = GuidHelper.CreateMockGuidQueue(10);
                var issueViewModels = new List<Issue>();
                for (int i = 0; i < 10; i++)
                {
                    issueViewModels.Add(new Issue() { Id = guids.Dequeue(), Subject = i.ToString("0000"), Body = "BODY..." });
                }
                memeoryRepository.InitFor(issueViewModels);
                _simpleRepository = memeoryRepository;
            }
            return _simpleRepository;
        }

        #endregion
    }
}