using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AjNetCore.Modules.Core.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (!(entry.Entity is ITrackable trackable)) continue;

                var now = DateTime.Now;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        trackable.UpdatedAt = now;
                        break;

                    case EntityState.Added:
                        trackable.CreatedAt = now;
                        trackable.UpdatedAt = now;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Model Configuration
            Assembly.GetAssembly(typeof(SqlContext)).GetTypes()
                .Where(x => x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType &&
                            y.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .Select(type => (dynamic)Activator.CreateInstance(type))
                .ForEach(seedClass => modelBuilder.ApplyConfiguration(seedClass));

            // Seed
            Assembly.GetAssembly(typeof(SqlContext)).GetTypes()
                .Where(myType => myType.IsClass)
                .Where(w => w.GetInterfaces().Contains(typeof(ISeedConfiguration)))
                .Select(type => (ISeedConfiguration)Activator.CreateInstance(type))
                .ForEach(seedClass => seedClass.Map(modelBuilder));
        }
    }
}
