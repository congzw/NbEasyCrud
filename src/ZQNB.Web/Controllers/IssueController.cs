using System;
using System.Web.Mvc;
using ZQNB.Web.Models;

namespace ZQNB.Web.Controllers
{
    public class IssueController : BaseController
    {
        public ActionResult Index()
        {
            return View();
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

        [HttpPost]
        public ActionResult Edit(IssueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var messageResult = SaveOrUpdate(viewModel);
            if (!messageResult.Success)
            {
                return JsonError(messageResult.Message);
            }

            var model = Get(viewModel.Id);
            return JsonSuccess(model);
        }

        #endregion

        #region helpers

        protected MessageResult SaveOrUpdate(object model)
        {
            //todo
            return new MessageResult();
        }

        protected object Get(Guid id)
        {
            //todo
            return null;
        }

        protected MessageResult Delete(object model)
        {
            //todo
            return new MessageResult();
        }

        #endregion
    }
}