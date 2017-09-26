using System.Web.Http;
using Ninject;
using Ninject.Modules;

namespace Xero.Web.Api
{
    public class WebApiApplication : Ninject.Web.Common.NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected override IKernel CreateKernel()
        {
            var modules = new INinjectModule[]
            {
                new TinyMapperModule()
            };

            return new StandardKernel(modules);
        }
    }
}