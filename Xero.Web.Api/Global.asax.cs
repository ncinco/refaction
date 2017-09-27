using System;
using System.Collections;
using System.Web;
using System.Web.Http;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Xero.Persistence;

namespace Xero.Web.Api
{
    public class WebApiApplication : NinjectHttpApplication
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
                new PersistenceModule(),
                new TinyMapperModule()
            };

            var kernel = new StandardKernel();

            kernel.Load(modules);

            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            // register the dependency resolver passing the kernel container
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return new StandardKernel();
        }
    }
}