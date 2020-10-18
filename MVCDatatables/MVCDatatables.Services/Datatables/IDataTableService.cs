using System;
using System.Linq;
using System.Linq.Expressions;
using Mvc.JQuery.DataTables;
using MVCDatatables.Models.Datatables;

namespace MVCDatatables.Services.Datatables
{
    /// <summary>
    /// The interface for the datatable service.
    /// </summary>
    public interface IDataTableService
    {
        /// <summary>
        /// Get filtered results for a datatable.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="query">The data collection.</param>
        /// <returns>A datatables json result.</returns>
        DataTableJsonResult<T> GetFilteredResultOrDefault<T>(DataTablesParam param, IQueryable<T> query)
            where T : class;

        /// <summary>
        /// Get filtered results for a datatable.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="query">The data collection.</param>
        /// <param name="filterClause">The filter clause.</param>
        /// <returns>A datatables json result.</returns>
        DataTableJsonResult<T> GetFilteredResultOrDefault<T>(DataTablesParam param, IQueryable<T> query, Expression<Func<T, bool>> filterClause)
            where T : class;
    }
}
