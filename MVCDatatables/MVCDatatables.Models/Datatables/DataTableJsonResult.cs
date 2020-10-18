using System.Collections.Generic;

namespace MVCDatatables.Models.Datatables
{
    /// <summary>
    /// The datatable json result.
    /// </summary>
    /// <typeparam name="T">The class.</typeparam>
    public class DataTableJsonResult<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the draw count. Unfortunately this must be lower case due to the way datatable.net library has been set up.
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// Gets or sets the data collection. Unfortunately this must be lower case due to the way datatable.net library has been set up.
        /// </summary>
        public IEnumerable<T> data { get; set; }

        /// <summary>
        /// Gets or sets the filtered record count. Unfortunately this must be lower case due to the way datatable.net library has been set up.
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the record total count. Unfortunately this must be lower case due to the way datatable.net library has been set up.
        /// </summary>
        public int recordsTotal { get; set; }
    }
}
