using Microsoft.EntityFrameworkCore.Storage;

namespace AjNetCore.Modules.Core.Data
{
    public interface IUnitOfWork
    {
        void Commit();
        void BeginTransaction();
        void Rollback();
        void CompleteTransaction();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private SqlContext _dataContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected SqlContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());

        public void Commit()
        {
            _databaseFactory.Get().SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _databaseFactory.Get().Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void CompleteTransaction()
        {
            _transaction?.Commit();
        }
    }
}