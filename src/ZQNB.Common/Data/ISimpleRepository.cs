using System;
using System.Linq;

namespace ZQNB.Common.Data
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
}
