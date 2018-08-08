using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZQNB.Common;
using ZQNB.Common.Cruds;
using ZQNB.Common.Data;
using ZQNB.Web.Models;
using ZQNB.Web.Models._Mock;

namespace ZQNB.Web.Controllers
{
    public abstract class CrudController<TViewModel, TEntity> : Controller
        where TViewModel : ICrudViewModel, new()
        where TEntity : INbEntity<Guid>
    {
        public ActionResult Index()
        {
            var models = CallGetAll();
            return View("_Crud/Index", models);
        }

        public ActionResult Add()
        {
            var viewModel = new TViewModel();
            return View("_Crud/Add", viewModel);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("_Crud/Add", viewModel);
            }

            var messageResult = CallAdd(viewModel);
            if (!messageResult.Success)
            {
                return View("_Crud/Add", viewModel).WithError(messageResult.Message);
            }

            return RedirectToAction("Index").WithSuccess(messageResult.Message);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            var messageResult = CallDelete(id);
            return RedirectToAction("Index").WithMessageResult(messageResult);
        }
        
        public ActionResult Edit(Guid id)
        {
            var viewModel = CallGet(id);
            if (viewModel == null)
            {
                return RedirectToAction("Index").WithError("没有找到记录：" + id);
            }
            return View("_Crud/Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("_Crud/Edit", viewModel);
            }

            var messageResult = CallEdit(viewModel);
            if (!messageResult.Success)
            {
                return View("_Crud/Edit", viewModel).WithError(messageResult.Message);
            }

            return RedirectToAction("Index").WithSuccess(messageResult.Message);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.CrudViewModelMeta = GetCrudViewModelMeta();
            base.OnActionExecuting(filterContext);
        }

        #region default impls

        private CrudViewModelMeta _meta;
        protected virtual CrudViewModelMeta GetCrudViewModelMeta()
        {
            return _meta ?? (_meta = CrudViewModelMeta.Create(typeof(TViewModel), this.GetType()));
        }

        protected virtual IList<TViewModel> CallGetAll()
        {
            var simpleRepository = GetRepository();
            var entities = simpleRepository.Query<TEntity>().ToList();
            var vos = new List<TViewModel>();
            foreach (var entity in entities)
            {
                var viewModel = new TViewModel();
                entity.TryCopyTo(viewModel);
                vos.Add(viewModel);
            }
            return vos;
        }

        protected virtual TViewModel CallGet(Guid id)
        {
            var simpleRepository = GetRepository();
            var entity = simpleRepository.Get<TEntity>(id);
            if (entity == null)
            {
                return default(TViewModel);
            }

            var viewModel = new TViewModel();
            entity.TryCopyTo(viewModel);
            return viewModel;
        }
        protected virtual MessageResult CallAdd(TViewModel model)
        {
            var messageResult = new MessageResult();
            if (model == null)
            {
                messageResult.Message = "对象不能为空";
                messageResult.Success = false;
                return messageResult;
            }

            ////mock errors
            //var vo = model as IssueViewModel;
            //if (vo != null && vo.Subject == "sss")
            //{
            //    messageResult.Message = "题目不能是: "+ vo.Subject;
            //    messageResult.Success = false;
            //    return messageResult;
            //}

            var entity = CreateEntity();
            model.TryCopyTo(entity);

            var simpleRepository = GetRepository();
            simpleRepository.Add(entity);

            messageResult.Success = true;
            messageResult.Message = "添加成功";
            messageResult.Data = entity.Id;
            return messageResult;
        }

        protected virtual MessageResult CallDelete(Guid id)
        {
            var messageResult = new MessageResult();
            if (id == Guid.Empty)
            {
                messageResult.Message = "Id不能为空";
                return messageResult;
            }

            var simpleRepository = GetRepository();
            var entity = simpleRepository.Get<TEntity>(id);
            if (entity == null)
            {
                messageResult.Message = "没有找到记录：" + id;
                return messageResult;
            }

            simpleRepository.Delete(entity);

            messageResult.Message = "删除成功";
            messageResult.Success = true;
            messageResult.Data = entity.Id;
            return messageResult;
        }

        protected virtual MessageResult CallEdit(TViewModel model)
        {
            var messageResult = new MessageResult();
            if (model == null)
            {
                messageResult.Message = "对象不能为空";
                messageResult.Success = false;
                return messageResult;
            }

            //mock errors
            var vo = model as IssueViewModel;
            if (vo != null && vo.Subject == "sss")
            {
                messageResult.Message = "题目不能是: " + vo.Subject;
                messageResult.Success = false;
                return messageResult;
            }

            var simpleRepository = GetRepository();
            var theOne = simpleRepository.Get<TEntity>(model.Id);
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
        }

        protected virtual string GetControllerName()
        {
            return this.GetType().Name.Replace("Controller", "");
        }

        private static ISimpleRepository _simpleRepository;
        private ISimpleRepository GetRepository()
        {
            if (_simpleRepository == null)
            {
                var memeoryRepository = new MemeoryRepository();
                
                var guids = GuidHelper.CreateMockGuidQueue(10);
                var issueViewModels = new List<IssueViewModel>();
                for (int i = 0; i < 10; i++)
                {
                    issueViewModels.Add(new IssueViewModel() { Id = guids.Dequeue(), Subject = i.ToString("0000"), Body = "BODY..."});
                }
                memeoryRepository.InitFor(issueViewModels);
                _simpleRepository = memeoryRepository;
            }
            return _simpleRepository;
        }
        private TEntity CreateEntity()
        {
            return Activator.CreateInstance<TEntity>();
        }

        #endregion
    }
}
