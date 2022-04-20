using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AjNetCore.Modules.Core.Helpers
{
    public static class DbSetHelper
    {
        public static TEntity AddOrUpdate<TEntity>(this DbSet<TEntity> dbSet, DbContext context, Func<TEntity, object> identifier, TEntity entity) where TEntity : class
        {
            var result = dbSet.Find(identifier.Invoke(entity));
            
            if (result != null)
            {
                context.Entry(result).CurrentValues.SetValues(entity);
                dbSet.Update(result);
                return result;
            }

            dbSet.Add(entity);
            return entity;
        }

    }
}