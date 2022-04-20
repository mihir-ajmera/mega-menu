using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using AjNetCore.Modules.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AjNetCore.Modules.Core.Data
{
    public interface IRepository<T> where T : class
    {
        void Insert(T entity);
        //void InsertOrUpdate(T entity);
        //void InsertOrUpdate(IEnumerable<T> entity);
        void Update(T entity);
        void Update(IList<T> entities);
        void Delete(T entity);
        void Delete(IList<T> entities);

        IEnumerable<T> All();
        T Find(int id);
        
        IQueryable<T> Table { get; }
        IQueryable<T> AsNoTracking { get; }

        string GenerateUniqueSlug(string phrase, int? id = null, string slugFieldName = "Slug");

        DbConnection GetDbConnection();
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private SqlContext _dataContext;
        private readonly DbSet<T> _dbSet;

        protected IDatabaseFactory DatabaseFactory { get; }
        protected SqlContext DataContext => _dataContext ??= DatabaseFactory.Get();

        public Repository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbSet = databaseFactory.Get().Set<T>();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
            DataContext.Entry(entity).State = EntityState.Added;
        }

        //public void InsertOrUpdate(T entity)
        //{
        //    DataContext.Set<T>().AddOrUpdate(entity);
        //}

        //public void InsertOrUpdate(IEnumerable<T> entities)
        //{
        //    foreach (var entity in entities)
        //        DataContext.Set<T>().AddOrUpdate(entity);    
        //}

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Set<T>().Attach(entity);
                var entry = DataContext.Entry(entity);
                entry.State = EntityState.Modified;
            }
        }

        public void Delete(T entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            DataContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
                DataContext.Entry(entity).State = EntityState.Deleted;
            }
        }

        public IEnumerable<T> All()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public string GenerateUniqueSlug(string phrase, int? id = null, string slugFieldName = "Slug")
        {
            int? loop = null;
            var slug = phrase.GenerateSlug();

            var where = $"{slugFieldName} = @0";
            if (id != null)
                where += " AND Id <> @1";

            while (AsNoTracking.Where(@where, slug, id).Any())
            {
                loop = loop == null ? 1 : loop + 1;
                slug = phrase.GenerateSlug() + ("-" + loop);
            }

            return slug;
        }

        public DbConnection GetDbConnection()
        {
            return DataContext.Database.GetDbConnection();
        }

        public IQueryable<T> Table => _dbSet;
        public IQueryable<T> AsNoTracking => _dbSet.AsNoTracking();
    }
}