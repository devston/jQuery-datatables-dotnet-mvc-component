using Autofac;
using MVCDatatables.Core.Interfaces;
using MVCDatatables.Core.Repositories;
using MVCDatatables.Data.Data;

namespace MVCDatatables.DependencyResolution.Modules
{
    /// <summary>
    /// The data autofac module.
    /// </summary>
    public class DataModule : Module
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>()
                .InstancePerRequest();

            builder.RegisterType<DemoRepository>()
                .As<IDemoRepository>()
                .InstancePerRequest();
        }
    }
}
