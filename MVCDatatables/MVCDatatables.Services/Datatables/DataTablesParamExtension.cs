using System.Collections.Generic;
using Mvc.JQuery.DataTables;
using MVCDatatables.Models.Datatables;

namespace MVCDatatables.Services.Datatables
{
    /// <summary>
    /// The datatables param extension.
    /// </summary>
    public static class DataTablesParamExtension
    {
        /// <summary>
        /// Get the search filter string.
        /// </summary>
        /// <param name="param">The datatables param.</param>
        /// <returns>The search filter string.</returns>
        public static string GetSearchFilterString(this DataTablesParam param)
        {
            return param.sSearch == null ? string.Empty : param.sSearch.ToLower();
        }

        /// <summary>
        /// Gets the empty datatables object.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <returns>An empty datatables object.</returns>
        public static DataTableJsonResult<T> GetEmptyDataTableObject<T>(this DataTablesParam param)
            where T : class
        {
            return new DataTableJsonResult<T>() { recordsTotal = 0, recordsFiltered = 0, data = new List<T>() };
        }
    }
}
