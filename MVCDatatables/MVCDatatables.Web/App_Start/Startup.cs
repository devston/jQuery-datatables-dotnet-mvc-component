using Autofac.Integration.Mvc;
using MVCDatatables.DependencyResolution;
using Owin;
using System.Web.Mvc;

namespace MVCDatatables.Web.App_Start
{
    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration of the app on startup.
        /// </summary>
        /// <param name="app">The app. </param>
        public void Configuration(IAppBuilder app)
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
        }
    }
}