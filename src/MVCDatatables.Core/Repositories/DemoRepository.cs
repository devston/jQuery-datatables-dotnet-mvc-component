using Mvc.JQuery.DataTables;
using MVCDatatables.Core.Interfaces;
using MVCDatatables.Data.Data;
using MVCDatatables.Models.Datatables;
using MVCDatatables.Models.Demo;
using MVCDatatables.Services.Datatables;
using System.Linq;

namespace MVCDatatables.Core.Repositories
{
    /// <summary>
    /// The demo repository.
    /// </summary>
    public class DemoRepository : IDemoRepository
    {
        /// <summary>
        /// The db context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The datatable service.
        /// </summary>
        private readonly IDataTableService _dataTableService;

        public DemoRepository(
            ApplicationDbContext context,
            IDataTableService dataTableService)
        {
            _context = context;
            _dataTableService = dataTableService;
        }

        /// <summary>
        /// Get the demo data as a datatable json result.
        /// </summary>
        /// <param name="param">The datatables param.</param>
        /// <returns>The datatables json result.</returns>
        public DataTableJsonResult<DemoDm> GetDemoDataTable(
            DataTablesParam param)
        {
            string searchString = param.GetSearchFilterString();
            return _dataTableService.GetFilteredResultOrDefault(
                param,
                _context.DummyData
                    .Select(x => new DemoDm()
                    {
                        Id = x.DummyKey,
                        Data = x.RandomData
                    }),
                x => x.Data.ToLower().Contains(searchString));
        }
    }
}
