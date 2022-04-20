using System;
using System.IO;

namespace AjNetCore.Modules.Core.Data.Seed
{
    public abstract class BaseSeed : IComparable<BaseSeed>
    {
        protected readonly SqlContext Context;
        public int OrderId { get; set; }

        public abstract void Seed();

        protected BaseSeed(SqlContext context)
        {
            Context = context;
        }

        public int CompareTo(BaseSeed other)
        {
            return OrderId.CompareTo(other.OrderId);
        }

        protected static string ReadFile(string moduleName, string fileName)
        {
            return File.ReadAllText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules", moduleName, "Data", "Seed", "Templates",
                    fileName));
        }
    }
}