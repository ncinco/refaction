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
            TinyMapper.Bind<Product, Persistence.Product>(config => config.Ignore(x => x.Id));
            TinyMapper.Bind<ProductOption, Persistence.ProductOption>(config =>
            {
                config.Ignore(x => x.Id);
                config.Ignore(x => x.ProductId);
            });

            TinyMapper.Bind<Persistence.Product, Product>();
            TinyMapper.Bind<Persistence.ProductOption, ProductOption>();
        }
    }
}