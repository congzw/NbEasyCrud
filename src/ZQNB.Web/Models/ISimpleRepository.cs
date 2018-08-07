using System;
using System.Linq;

namespace ZQNB.Web.Models
{
    /// <summary>
    /// 简化的通用仓储的接口声明
    /// </summary>
    /// <typeparam name="TPk"></typeparam>
    public interface ISimpleRepository<TPk>
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>() where T : INbEntity<TPk>;

        /// <summary>
        /// 获取某个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get<T>(Guid id) where T : INbEntity<TPk>;

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void Add<T>(params T[] entities) where T : INbEntity<TPk>;

        /// <summary>
        /// 新增批量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        void Add<T>(T entity, Guid? id = null) where T : INbEntity<TPk>;

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void Update<T>(params T[] entities) where T : INbEntity<TPk>;

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void SaveOrUpdate<T>(params T[] entities) where T : INbEntity<TPk>;

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void Delete<T>(params T[] entities) where T : INbEntity<TPk>;

        /// <summary>
        /// 提交数据
        /// </summary>
        void Flush();
    }

    /// <summary>
    /// 简化的仓储
    /// </summary>
    public interface ISimpleRepository : ISimpleRepository<Guid>
    {
    }

    public class MockSimpleRepository : ISimpleRepository
    {
        private static MockData data = new MockData();

        public IQueryable<T> Query<T>() where T : INbEntity<Guid>
        {
            return (IQueryable<T>) data.Issues.AsQueryable();
        }

        public T Get<T>(Guid id) where T : INbEntity<Guid>
        {
            var entity = (object)data.Issues.SingleOrDefault(x => x.Id == id);
            return entity is T ? (T) entity : default(T);
        }

        public void Add<T>(params T[] entities) where T : INbEntity<Guid>
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public void Add<T>(T entity, Guid? id = null) where T : INbEntity<Guid>
        {
            if (id != null)
            {
                entity.Id = id.Value;
            }

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            data.Issues.Add((IssueViewModel)(object)entity);
        }

        public void Update<T>(params T[] entities) where T : INbEntity<Guid>
        {
            foreach (var entity in entities)
            {
                var theOne = data.Issues.SingleOrDefault(x => x.Id == entity.Id);
                if (theOne != null)
                {
                    entity.TryCopyTo(theOne);
                }
            }
        }

        public void SaveOrUpdate<T>(params T[] entities) where T : INbEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(params T[] entities) where T : INbEntity<Guid>
        {
            foreach (var entity in entities)
            {
                var theOne = data.Issues.SingleOrDefault(x => x.Id == entity.Id);
                if (theOne != null)
                {
                    data.Issues.Remove(theOne);
                }
            }
        }

        public void Flush()
        {
        }
    }
}
