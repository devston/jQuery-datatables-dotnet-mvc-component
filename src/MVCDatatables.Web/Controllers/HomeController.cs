using Mvc.JQuery.DataTables;
using MVCDatatables.Core.Interfaces;
using MVCDatatables.Models.Demo;
using MVCDatatables.Presentation.Models.Datatables;
using MVCDatatables.Services.Datatables;
using System.Web.Mvc;

namespace MVCDatatables.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDemoRepository _demoRepository;

        public HomeController(IDemoRepository demoRepository)
        {
            _demoRepository = demoRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Demo()
        {
            return View();
        }

        /// <summary>
        /// Get the demo list data.
        /// </summary>
        /// <param name="param">The datatables param.</param>
        /// <returns>A json containing the demo list data.</returns>
        [HttpGet]
        public JsonResult GetDemoListData(DataTablesParam param)
        {
            var data = _demoRepository.GetDemoDataTable(param);

            if (data == null)
            {
                return Json(param.GetEmptyDataTableObject<DemoDm>(), JsonRequestBehavior.AllowGet);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the demo table.
        /// </summary>
        /// <returns>A json containing the demo table.</returns>
        [HttpGet]
        public JsonResult DemoListTable()
        {
            var vm = new DataTableViewModel("Data");
            vm.AddColumn("Id", "Id").IsHidden();
            vm.AddColumn("RandomData", "Data").IsSortable(true).AllowSearch();
            vm.WithBackendURL(Url.Action(nameof(GetDemoListData), "Home", null));
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the demo table.
        /// </summary>
        /// <returns>A json containing the demo table.</returns>
        [HttpGet]
        public JsonResult ChildDemoListTable()
        {
            var vm = new DataTableViewModel("Data");
            vm.AddColumn("Id", "Id").IsSortable(true).AllowSearch();
            vm.AddColumn("RandomData", "Data").IsSortable().AllowSearch().IsInsideChildRow();
            vm.WithBackendURL(Url.Action(nameof(GetDemoListData), "Home", null));
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}