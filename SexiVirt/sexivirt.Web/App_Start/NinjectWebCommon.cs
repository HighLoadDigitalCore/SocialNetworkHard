using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using sexivirt.Web.Global.Config;
using sexivirt.Model;
using sexivirt.Web.Global.Auth;

[assembly: WebActivator.PreApplicationStartMethod(typeof(sexivirt.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(sexivirt.Web.App_Start.NinjectWebCommon), "Stop")]

namespace sexivirt.Web.App_Start
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IConfig>().To<Config>();
            kernel.Bind<sexivirtDbDataContext>().ToMethod(c => new sexivirtDbDataContext(kernel.Get<IConfig>().ConnectionStrings("ConnectionString")));
            kernel.Bind<IRepository>().To<SqlRepository>().InRequestScope();
            kernel.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
        }        
    }
}
