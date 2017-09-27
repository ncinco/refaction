using Nelibur.ObjectMapper;
using Ninject.Modules;
using Product = Xero.Web.Api.Models.Product;
using ProductOption = Xero.Web.Api.Models.ProductOption;

namespace Xero.Web.Api
{
    public class TinyMapperModule : NinjectModule
    {
        public override void Load()
        {
            TinyMapper.Bind<Product, Persistence.Product>();
            TinyMapper.Bind<ProductOption, Persistence.ProductOption>();

            TinyMapper.Bind<Persistence.Product, Product>();
            TinyMapper.Bind<Persistence.ProductOption, ProductOption>();
        }
    }
}