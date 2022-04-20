using System;

namespace AjNetCore.Modules.Core.Data
{
    public interface IDatabaseFactory : IDisposable
    {
        SqlContext Get();
    }

    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private readonly SqlContext _sqlContext;
        private SqlContext _dataContext;

        public DatabaseFactory(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public SqlContext Get()
        {
            return _dataContext ??= _sqlContext;
        }

        protected override void DisposeCore()
        {
            _dataContext?.Dispose();
            base.DisposeCore();
        }
    }
}