using Autofac;
using MVCDatatables.Services.Datatables;

namespace MVCDatatables.DependencyResolution.Modules
{
    /// <summary>
    /// The services autofac module.
    /// </summary>
    public class ServicesModule : Module
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataTableService>()
                .As<IDataTableService>()
                .InstancePerRequest();
        }
    }
}
