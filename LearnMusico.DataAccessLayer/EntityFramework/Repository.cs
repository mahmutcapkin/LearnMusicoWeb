using LearnMusico.Common;
using LearnMusico.Core;
using LearnMusico.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearnMusico.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }
        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }


        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            //obj nesnesi bir MyEntityBase ise demek
            if (obj is MyEntityBase)
            {
                MyEntityBase my = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                my.CreatedOn = now;
                my.ModifiedOn = now;
                my.ModifiedUsername = App.Common.GetCurrentUsername();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase my = obj as MyEntityBase;

                my.ModifiedOn = DateTime.Now;
                my.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase my = obj as MyEntityBase;

            //    my.ModifiedOn = DateTime.Now;
            //    my.ModifiedUsername = "system"; //TODO: işlem yapan kullanıcı adı yazılmaı
            //}
            _objectSet.Remove(obj);

            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        //public IQueryable<T> List(Expression<Func<T,bool>> where)
        //{
        //    //IQueryable yapısı dönünce orderby, ifadeler, 10 kaydı ver gibi sorgular eklenebilir.
        //    //kullanıcı ne zaman cagırdıysa buna ToList() eklendiğinde SQL E sorgu atar
        //    return _objectSet.Where(where);
        //}
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
