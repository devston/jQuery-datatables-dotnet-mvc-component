using System.Web.Mvc;

namespace MVCDatatables.Web.Common.DataTables
{
    /// <summary>
    /// The Mvc value provider.
    /// </summary>
    public class MvcValueProvider : IGenericValueProvider
    {
        private IValueProvider _valueProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcValueProvider"/> class.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        public MvcValueProvider(IValueProvider valueProvider)
        {
            _valueProvider = valueProvider;
        }

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            var valueResult = _valueProvider.GetValue(key);
            return (valueResult == null)
                ? default(T)
                : (T)valueResult.ConvertTo(typeof(T));
        }
    }
}