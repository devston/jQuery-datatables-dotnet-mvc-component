using Mvc.JQuery.DataTables;
using MVCDatatables.Models.Datatables;
using MVCDatatables.Models.Demo;

namespace MVCDatatables.Core.Interfaces
{
    /// <summary>
    /// The interface for the demo repository.
    /// </summary>
    public interface IDemoRepository
    {
        /// <summary>
        /// Get the demo data as a datatable json result.
        /// </summary>
        /// <param name="param">The datatables param.</param>
        /// <returns>The datatables json result.</returns>
        DataTableJsonResult<DemoDm> GetDemoDataTable(DataTablesParam param);
    }
}
