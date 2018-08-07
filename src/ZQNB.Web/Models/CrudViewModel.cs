using System;
using System.Collections.Generic;
using System.Linq;

namespace ZQNB.Web.Models
{
    public interface ICrudViewModel : IGuidPk
    {
        //void CopyFrom<TEntity>(TEntity entity) where TEntity : INbEntity<Guid>;
        //void CopyTo<TEntity>(TEntity entity) where TEntity : INbEntity<Guid>;
        //TEntity Create<TEntity>() where TEntity : INbEntity<Guid>; 

        //TEntity Create<TEntity>() where TEntity : INbEntity<Guid>; 
    }

    //public interface ICrudViewModelRepository
    //{
    //    IList<dynamic> GetAll();

    //    dynamic Get(Guid id);

    //    MessageResult Add(dynamic model);

    //    MessageResult Delete(Guid id);

    //    MessageResult Update(dynamic model);
    //}

    //public interface ICrudViewModelRepository<TViewModel> : ICrudViewModelRepository
    //    where TViewModel : ICrudViewModel, new()
    //{
    //    IList<TViewModel> GetAllVos();

    //    TViewModel GetVo(Guid id);

    //    MessageResult AddVo(TViewModel vo);

    //    MessageResult UpdateVo(TViewModel vo);
    //}

    //public class CrudViewModelRepository<TViewModel, TEntity> : ICrudViewModelRepository<TViewModel>
    //    where TViewModel : ICrudViewModel, new()
    //    where TEntity : INbEntity<Guid>
    //{
    //    private readonly ISimpleRepository SimpleRepository;

    //    protected CrudViewModelRepository(ISimpleRepository simpleRepository)
    //    {
    //        SimpleRepository = simpleRepository;
    //    }

    //    public IList<dynamic> GetAll()
    //    {
    //        var datas = GetAllVos();
    //        var objects = CastToObjects(datas);
    //        return objects;
    //    }

    //    public dynamic Get(Guid id)
    //    {
    //        var data = GetVo(id);
    //        return data;
    //    }

    //    public MessageResult Add(dynamic model)
    //    {
    //        var result = AddVo((TViewModel)model);
    //        return result;
    //    }

    //    public MessageResult Delete(Guid id)
    //    {
    //        var messageResult = new MessageResult();
    //        if (id == Guid.Empty)
    //        {
    //            messageResult.Message = "Id不能为空";
    //            return messageResult;
    //        }

    //        var entity = SimpleRepository.Get<TEntity>(id);
    //        if (entity == null)
    //        {
    //            messageResult.Message = "没有找到记录：" + id;
    //            return messageResult;
    //        }
            
    //        SimpleRepository.Delete(entity);

    //        messageResult.Message = "删除成功";
    //        messageResult.Success = true;
    //        messageResult.Data = entity.Id;
    //        return messageResult;
    //    }

    //    public MessageResult Update(dynamic model)
    //    {
    //        var result = UpdateVo((TViewModel)model);
    //        return result;
    //    }

    //    public IList<TViewModel> GetAllVos()
    //    {
    //        var entities = SimpleRepository.Query<TEntity>().ToList();
    //        var vos = new List<TViewModel>();
    //        foreach (var entity in entities)
    //        {
    //            var viewModel = new TViewModel();
    //            viewModel.CopyFrom(entity);
    //            vos.Add(viewModel);
    //        }
    //        return vos;
    //    }

    //    public TViewModel GetVo(Guid id)
    //    {
    //        var entity = SimpleRepository.Get<TEntity>(id);
    //        if (entity == null)
    //        {
    //            return default(TViewModel);
    //        }
    //        var viewModel = new TViewModel();
    //        viewModel.CopyFrom(entity);
    //        return viewModel;
    //    }

    //    public MessageResult AddVo(TViewModel model)
    //    {
    //        var messageResult = new MessageResult();
    //        if (model == null)
    //        {
    //            messageResult.Message = "对象不能为空";
    //            messageResult.Success = false;
    //            return messageResult;
    //        }

    //        var entity = model.Create<TEntity>();
    //        model.CopyTo(entity);
    //        SimpleRepository.Add(entity);

    //        messageResult.Success = true;
    //        messageResult.Message = "添加成功";
    //        messageResult.Data = entity.Id;
    //        return messageResult;
    //    }

    //    public MessageResult UpdateVo(TViewModel model)
    //    {
    //        //todo
    //        var messageResult = new MessageResult();
    //        if (model == null)
    //        {
    //            messageResult.Message = "对象不能为空";
    //            messageResult.Success = false;
    //            return messageResult;
    //        }

    //        var theOne = SimpleRepository.Get<TEntity>(model.Id);
    //        if (theOne == null)
    //        {
    //            messageResult.Message = "没有找到记录：" + model.Id;
    //            messageResult.Success = false;
    //            return messageResult;
    //        }

    //        model.CopyTo(theOne);
    //        messageResult.Message = "修改成功";
    //        messageResult.Success = true;
    //        messageResult.Data = model.Id;
    //        return messageResult;
    //    }

    //    private IList<dynamic> CastToObjects(IList<TViewModel> list)
    //    {
    //        var objects = list.Cast<dynamic>().ToList();
    //        return objects;
    //    }
    //}
}
