﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        #region MVC Logic

        public ActionResult Index()
        {
            var models = CallGetAll();
            return View("_CrudIndex", models);
        }

        public ActionResult Add()
        {
            var viewModel = new TViewModel();
            return View("_CrudAdd", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("_CrudAdd", viewModel);
            }

            var messageResult = CallAdd(viewModel);
            if (!messageResult.Success)
            {
                return View("_CrudAdd", viewModel).WithError(messageResult.Message);
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
            return View("_CrudEdit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("_CrudEdit", viewModel);
            }

            var messageResult = CallEdit(viewModel);
            if (!messageResult.Success)
            {
                return View("_CrudEdit", viewModel).WithError(messageResult.Message);
            }

            return RedirectToAction("Index").WithSuccess(messageResult.Message);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ProcessMySelectListItems();
            base.OnActionExecuting(filterContext);
        }

        #endregion
        
        #region default simple crud impls

        protected virtual IList<TViewModel> CallGetAll()
        {
            var simpleRepository = ResolveRepository();
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
            var simpleRepository = ResolveRepository();
            var entity = simpleRepository.Get<TEntity>(id);
            if (entity == null)
            {
                return default(TViewModel);
            }
            var viewModel = new TViewModel();
            entity.TryCopyTo(viewModel);
            return viewModel;
        }

        protected virtual MessageResult CallAddValidate(TViewModel model)
        {
            var validateResult = MessageResult.ValidateResult();
            if (model == null)
            {
                validateResult.Message = "对象不能为空";
                return validateResult;
            }

            //override to add validate logic
            validateResult.Success = true;
            validateResult.Message = "OK";
            return validateResult;
        }

        protected virtual MessageResult CallAdd(TViewModel model)
        {
            var callAddValidate = CallAddValidate(model);
            if (!callAddValidate.Success)
            {
                return callAddValidate;
            }

            var messageResult = new MessageResult();
            var entity = CreateEntity();
            model.TryCopyTo(entity);

            var simpleRepository = ResolveRepository();
            simpleRepository.Add(entity);

            messageResult.Success = true;
            messageResult.Message = "添加成功";
            messageResult.Data = entity.Id;
            return messageResult;
        }

        protected virtual MessageResult CallDeleteValidate(Guid id)
        {
            var validateResult = MessageResult.ValidateResult();
            if (id == Guid.Empty)
            {
                validateResult.Message = "Id不能为空";
                return validateResult;
            }

            //override to add validate logic
            validateResult.Success = true;
            validateResult.Message = "OK";
            return validateResult;
        }


        protected virtual MessageResult CallDelete(Guid id)
        {
            var callDeleteValidate = CallDeleteValidate(id);
            if (!callDeleteValidate.Success)
            {
                return callDeleteValidate;
            }

            var messageResult = new MessageResult();
            var simpleRepository = ResolveRepository();
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

        protected virtual MessageResult CallEditValidate(TViewModel model)
        {
            var validateResult = MessageResult.ValidateResult();
            if (model == null)
            {
                validateResult.Message = "对象不能为空";
                return validateResult;
            }

            //override to add validate logic
            validateResult.Success = true;
            validateResult.Message = "OK";
            return validateResult;
        }


        protected virtual MessageResult CallEdit(TViewModel model)
        {
            var callEditValidate = CallEditValidate(model);
            if (!callEditValidate.Success)
            {
                return callEditValidate;
            }

            var messageResult = new MessageResult();

            var simpleRepository = ResolveRepository();
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

        /// <summary>
        /// 当需要为多个属性设置参照集合时，通过property来区分不同的集合
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="getItems"></param>
        protected void AddSelectListItemsForProperty<TProp>(Expression<Func<TViewModel, TProp>> expression, Func<List<MySelectListItem>> getItems)
        {
            //var memberExpression = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;
            MemberExpression memberExpression = (MemberExpression)expression.Body;
            var property = memberExpression.Member.Name;
            var items = getItems();
            MySelectListItem.TrySetMySelectListItems(items, property);
        }

        protected virtual void ProcessMySelectListItems()
        {
            //todo
        }

        protected virtual string GetControllerName()
        {
            return this.GetType().Name.Replace("Controller", "");
        }

        private ISimpleRepository _simpleRepository = null;
        protected virtual ISimpleRepository ResolveRepository()
        {
            if (_simpleRepository == null)
            {
                //_simpleRepository = CoreServiceProvider.LocateService<ISimpleRepository>;
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
