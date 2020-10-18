using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mvc.JQuery.DataTables;
using Mvc.JQuery.DataTables.Models;
using MVCDatatables.Models.Datatables;

namespace MVCDatatables.Services.Datatables
{
    /// <summary>
    /// The datatables service.
    /// </summary>
    public class DataTableService : IDataTableService
    {
        /// <summary>
        /// Get filtered results for a datatable.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="query">The data collection.</param>
        /// <returns>A datatables json result.</returns>
        public DataTableJsonResult<T> GetFilteredResultOrDefault<T>(DataTablesParam param, IQueryable<T> query)
            where T : class
        {
            var result = new DataTableJsonResult<T>();
            var listOfContainsClauses = GetContainsClausesOrDefault(param, query);
            var totals = GetTotal(query, listOfContainsClauses);
            var eventInfo = GetFilterClause(query, param);
            result.recordsFiltered = totals.FilteredCount;
            result.recordsTotal = totals.TotalCount;
            var sortedandPagedTasks = GetPagedEntities(param, GetOrderByColumnName(param), eventInfo);

            if (result.recordsFiltered <= 0)
            {
                return null;
            }

            result.data = sortedandPagedTasks?.ToList();
            return result;
        }

        /// <summary>
        /// Get filtered results for a datatable.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="query">The data collection.</param>
        /// <param name="filterClause">The filter clause.</param>
        /// <returns>A datatables json result.</returns>
        public DataTableJsonResult<T> GetFilteredResultOrDefault<T>(DataTablesParam param, IQueryable<T> query, Expression<Func<T, bool>> filterClause)
            where T : class
        {
            var result = new DataTableJsonResult<T>();
            var totals = GetTotal(query, filterClause);
            var eventInfo = query;

            if (filterClause != null && !string.IsNullOrEmpty(param.GetSearchFilterString()))
            {
                eventInfo = eventInfo.Where(filterClause);
            }

            result.recordsFiltered = totals.FilteredCount;
            result.recordsTotal = totals.TotalCount;
            var sortedandPagedTasks = GetPagedEntities(param, GetOrderByColumnName(param), eventInfo);

            if (result.recordsFiltered <= 0)
            {
                return null;
            }

            result.data = sortedandPagedTasks?.ToList();
            return result;
        }

        /// <summary>
        /// Get the order by column name.
        /// </summary>
        /// <param name="dataTableParam">The datatable param.</param>
        /// <returns>The sort direction.</returns>
        private static Dictionary<string, SortDirection> GetOrderByColumnName(DataTablesParam dataTableParam)
        {
            var c = new Dictionary<string, SortDirection>();

            for (int v = 0; v < dataTableParam.iSortCol.Count; v++)
            {
                c.Add(dataTableParam.sColumnNames[dataTableParam.iSortCol[v]], IsDataTableSortAscending(dataTableParam, v));
            }

            return c;
        }

        /// <summary>
        /// Get the sort direction.
        /// </summary>
        /// <param name="dataTableParam">The datatables param.</param>
        /// <param name="index">The index of the column.</param>
        /// <returns>The sort direction.</returns>
        private static SortDirection IsDataTableSortAscending(DataTablesParam dataTableParam, int index)
        {
            return dataTableParam.sSortDir[index] == "asc" ? SortDirection.Ascending : SortDirection.Descending;
        }

        /// <summary>
        /// Get the contains clauses in the query.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="query">The dataset.</param>
        /// <returns>The filtered query.</returns>
        private IQueryable<T> GetContainsClausesOrDefault<T>(DataTablesParam param, IQueryable<T> query)
            where T : class
        {
            var columnNames = new List<string>();
            string searchTerm = param.GetSearchFilterString();

            // If there are no searchable columns then return null.
            if (!param.bSearchable.Any(a => a))
            {
                return null;
            }

            for (int i = 0; i < param.bSearchable.Count; i++)
            {
                if (param.bSearchable[i])
                {
                    columnNames.Add(param.sColumnNames[i]);
                }
            }

            return query.GenericOrContains(columnNames.ToArray(), searchTerm);
        }

        /// <summary>
        /// Get the filtered query.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="param">The datatables param.</param>
        /// <returns>The filtered query.</returns>
        private IQueryable<T> GetFilterClause<T>(IQueryable<T> query, DataTablesParam param)
            where T : class
        {
            string searchTerm = param.GetSearchFilterString();
            var colNames = new List<string>();

            // If there are no searchable columns then return null.
            if (!param.bSearchable.Any(a => a))
            {
                return query;
            }

            for (int i = 0; i < param.bSearchable.Count; i++)
            {
                if (param.bSearchable[i])
                {
                    colNames.Add(param.sColumnNames[i]);
                }
            }

            query = query.GenericOrContains(colNames.ToArray(), searchTerm);
            return query;
        }

        /// <summary>
        /// Get the paged data.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="param">The datatables param.</param>
        /// <param name="sortUsingProperty">The sort direction.</param>
        /// <param name="query">The dataset.</param>
        /// <returns>The paged dataset.</returns>
        private IEnumerable<T> GetPagedEntities<T>(DataTablesParam param, Dictionary<string, SortDirection> sortUsingProperty, IQueryable<T> query)
        {
            var sortedQuery = query;

            foreach (var item in sortUsingProperty)
            {
                if (item.Value == SortDirection.Ascending)
                {
                    sortedQuery = sortedQuery.GenericOrderBy(item.Key).Skip(param.iDisplayStart).Take(param.iDisplayLength);
                }
                else
                {
                    sortedQuery = sortedQuery.GenericOrderByDesc(item.Key).Skip(param.iDisplayStart).Take(param.iDisplayLength);
                }
            }

            return sortedQuery;
        }

        /// <summary>
        /// Get the total count.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="query">The dataset.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The total count.</returns>
        private DataTablesRecordCount GetTotal<T>(IQueryable<T> query, Expression<Func<T, bool>> filter)
            where T : class
        {
            var recordCount = new DataTablesRecordCount();

            if (filter != null)
            {
                recordCount.FilteredCount = query.Where(filter).Count();
                recordCount.TotalCount = query.Count();
                return recordCount;
            }

            recordCount.FilteredCount = query.Count();
            recordCount.TotalCount = recordCount.FilteredCount;
            return recordCount;
        }

        /// <summary>
        /// Get the filtered result count.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="query">The dataset.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The count.</returns>
        private DataTablesRecordCount GetTotalNonContext<T>(IQueryable<T> query, Expression<Func<T, bool>> filter)
            where T : class
        {
            var recordCount = new DataTablesRecordCount();
            try
            {
                recordCount.FilteredCount = query.Where(filter).Count();
            }
            catch (Exception)
            {
                recordCount.FilteredCount = query.Count();
            }

            recordCount.TotalCount = query.Count();
            return recordCount;
        }

        /// <summary>
        /// Get the total count.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="query">The dataset.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The count.</returns>
        private DataTablesRecordCount GetTotal<T>(IQueryable<T> query, IQueryable<T> filter)
            where T : class
        {
            var recordCount = new DataTablesRecordCount();

            if (filter != null)
            {
                recordCount.FilteredCount = filter.Count();
                recordCount.TotalCount = query.Count();
                return recordCount;
            }

            recordCount.FilteredCount = query.Count();
            recordCount.TotalCount = recordCount.FilteredCount;
            return recordCount;
        }
    }
}
