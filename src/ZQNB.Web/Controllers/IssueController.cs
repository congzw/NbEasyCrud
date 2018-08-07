using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZQNB.Web.Models;

namespace ZQNB.Web.Controllers
{
    public class IssueController : BaseController
    {
        public ActionResult Index()
        {
            var models = GetAll();
            return View(models);
        }
        
        public ActionResult Add()
        {
            var viewModel = new IssueViewModel();
            return View(viewModel);
        }

        public ActionResult Edit(Guid id)
        {
            var viewModel = Get(id);
            if (viewModel == null)
            {
                return RedirectToAction<IssueController>(c => c.Index())
                    .WithError("没有找到记录：" + id);
            }
            return View(viewModel);
        }

        #region post


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(IssueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var messageResult = SaveOrUpdate(viewModel);
            if (!messageResult.Success)
            {
                ModelState.AddModelError("_MessageResult", messageResult.Message);
                return View(viewModel);
            }

            return RedirectToAction<IssueController>(c => c.Index())
                .WithSuccess(messageResult.Message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            var viewModel = Get(id);

            if (viewModel == null)
            {
                return RedirectToAction<IssueController>(c => c.Index())
                    .WithError("没有找到记录：" + id);
            }

            var messageResult = Delete(viewModel);
            return RedirectToAction<IssueController>(c => c.Index())
                .WithMessageResult(messageResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(IssueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var messageResult = SaveOrUpdate(viewModel);
            if (!messageResult.Success)
            {
                ModelState.AddModelError("_MessageResult", messageResult.Message);
                return View(viewModel);
            }

            return RedirectToAction<IssueController>(c => c.Index())
                .WithSuccess(messageResult.Message);
        }

        //ajax 
        //[HttpPost]
        //public ActionResult Edit(IssueViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return JsonValidationError();
        //    }

        //    var messageResult = SaveOrUpdate(viewModel);
        //    if (!messageResult.Success)
        //    {
        //        return JsonError(messageResult.Message);
        //    }

        //    var model = Get(viewModel.Id);
        //    return JsonSuccess(model);
        //}

        #endregion

        #region helpers

        protected MessageResult SaveOrUpdate(IssueViewModel model)
        {
            //todo
            var messageResult = new MessageResult();
            if (model == null)
            {
                messageResult.Message = "对象不能为空";
                messageResult.Success = false;
                return messageResult;
            }

            if (model.Id == Guid.Empty)
            {
                //Add New
                model.Id = Guid.NewGuid();
                mockData.Issues.Add(model);
                messageResult.Success = true;
                messageResult.Message = "添加成功";
                messageResult.Data = model.Id;
                return messageResult;
            }

            var theOne = mockData.Issues.SingleOrDefault(x => x.Id == model.Id);
            if (theOne == null)
            {
                messageResult.Message = "没有找到记录：" + model.Id;
                messageResult.Success = false;
                return messageResult;
            }

            model.TryCopyTo(theOne);
            messageResult.Message = "修改成功";
            messageResult.Success = true;
            messageResult.Data = model.Id;
            return messageResult;
            return new MessageResult();
        }

        protected IssueViewModel Get(Guid id)
        {
            //todo
            return mockData.Issues.SingleOrDefault(x => x.Id == id);
        }

        protected IList<IssueViewModel> GetAll()
        {
            //todo
            var issueViewModels = mockData.Issues;
            return issueViewModels;
        }

        protected MessageResult Delete(IGuidPk model)
        {
            //todo
            var theOne = mockData.Issues.SingleOrDefault(x => x.Id == model.Id);
            if (theOne != null)
            {
                mockData.Issues.Remove(theOne);
            }

            var messageResult = new MessageResult();
            messageResult.Message = "删除成功";
            messageResult.Success = true;
            messageResult.Data = model.Id;
            return messageResult;
        }

        private static readonly MockData mockData = new MockData();

        #endregion
    }
}