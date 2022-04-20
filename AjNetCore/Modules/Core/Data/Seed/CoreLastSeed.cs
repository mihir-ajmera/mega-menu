namespace AjNetCore.Modules.Core.Data.Seed
{
    public class CoreLastSeed : BaseSeed
    {
        public CoreLastSeed(SqlContext context) : base(context)
        {
            OrderId = 9999;
        }

        public override void Seed()
        {
            Context.BuildTree("AdminPermission");
        }
    }
}