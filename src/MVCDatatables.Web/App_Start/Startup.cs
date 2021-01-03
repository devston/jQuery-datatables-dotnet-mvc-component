using MVCDatatables.DependencyResolution;
using Owin;

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
            // Set the dependency resolver to be autofac.
            var container = IoCBootstrapper.GetIoCContainer();
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }
    }
}