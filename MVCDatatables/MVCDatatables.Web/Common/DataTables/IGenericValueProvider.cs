namespace MVCDatatables.Web.Common.DataTables
{
    /// <summary>
    /// The interface for the generic value provider.
    /// </summary>
    public interface IGenericValueProvider
    {
        /// <summary>
        /// Get a value.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="v">The property.</param>
        /// <returns>The class type.</returns>
        T GetValue<T>(string v);
    }
}
