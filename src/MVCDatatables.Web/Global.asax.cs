using Autofac.Integration.Mvc;
using MVCDatatables.DependencyResolution;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCDatatables.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The main application start method.
        /// </summary>
        protected void Application_Start()
        {
            // Set up DI.
            var builder = IoCBootstrapper.SetUpBuilder();

            // Register the MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register any model binders.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // Set the dependency resolver to be autofac.
            var container = builder.Build();
            IoCBootstrapper.SetIoCContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// The application shut down method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            // Dispose of the IoC container.
            IoCBootstrapper.DisposeContainer();
        }
    }
}
