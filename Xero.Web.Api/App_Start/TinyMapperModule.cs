using Nelibur.ObjectMapper;
using Xero.Web.Api.Models;

namespace Xero.Web.Api
{
    public class TinyMapperModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            TinyMapper.Bind<Product, Xero.Persistence.Product>();
            TinyMapper.Bind<ProductOption, Xero.Persistence.ProductOption>();

            TinyMapper.Bind<Xero.Persistence.Product, Product>();
            TinyMapper.Bind<Xero.Persistence.ProductOption, ProductOption>();
        }
    }
}