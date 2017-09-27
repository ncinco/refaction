using Ninject.Modules;
using Xero.Persistence;

namespace Xero.Web.Api
{
    public class PersistenceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IProductsContext>().To<ProductsContext>().InSingletonScope();
        }
    }
}