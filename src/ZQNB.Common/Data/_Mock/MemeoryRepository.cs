using System;
using System.Collections.Generic;
using System.Linq;

namespace ZQNB.Common.Data._Mock
{
    public class MemeoryRepository : ISimpleRepository
    {
        public MemeoryRepository()
        {
            Datas = new Dictionary<Type, IList<object>>();
        }

        public Dictionary<Type, IList<object>> Datas { get; set; }

        public MemeoryRepository InitFor<T>(IList<T> list)
        {
            var type = typeof (T);
            Datas[type] = list.Cast<object>().ToList();
            return this;
        }

        private IList<T> GetList<T>()
        {
            var type = typeof(T);
            if (!Datas.ContainsKey(type))
            {
                return Enumerable.Empty<T>().ToList();
            }
            return Datas[type].Cast<T>().ToList();
        }


        public IQueryable<T> Query<T>() where T : INbEntity<Guid>
        {
            return GetList<T>().AsQueryable();
        }

        public T Get<T>(Guid id) where T : INbEntity<Guid>
        {
            var entity = Query<T>().SingleOrDefault(x => x.Id == id);
            return entity;
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

            var list = GetList<T>();
            list.Add(entity);
            var type = typeof(T);
            Datas[type] = list.Cast<object>().ToList();
        }

        public void Update<T>(params T[] entities) where T : INbEntity<Guid>
        {
            var list = GetList<T>();
            foreach (var entity in entities)
            {
                var theOne = list.SingleOrDefault(x => x.Id == entity.Id);
                if (theOne != null)
                {
                    entity.TryCopyTo(theOne);
                }
            }
        }

        public void SaveOrUpdate<T>(params T[] entities) where T : INbEntity<Guid>
        {
            foreach (var entity in entities)
            {
                if (entity.Id == Guid.Empty)
                {
                    Add(entity);
                }
                else
                {
                    Update(entity);
                }
            }
        }

        public void Delete<T>(params T[] entities) where T : INbEntity<Guid>
        {
            var list = GetList<T>();
            foreach (var entity in entities)
            {
                var theOne = list.SingleOrDefault(x => x.Id == entity.Id);
                if (theOne != null)
                {
                    list.Remove(theOne);
                    var type = typeof(T);
                    Datas[type] = list.Cast<object>().ToList();
                }
            }
        }

        public void Flush()
        {
        }
    }
}