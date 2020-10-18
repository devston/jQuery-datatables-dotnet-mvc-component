using System;

namespace MVCDatatables.Presentation.Models.Datatables
{
    /// <summary>
    /// The query string parameter for datatables.
    /// </summary>
    public class QueryStringParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStringParameter"/> class.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public QueryStringParameter(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name must have a value");
            }

            ParameterName = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
    }
}
