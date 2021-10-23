using System.Web.Optimization;

namespace MVCDatatables.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/Plugins/DataTables/datatables-bs4.js",
                "~/Scripts/Plugins/DataTables/Buttons-1.5.1/js/dataTables.buttons.js",
                "~/Scripts/Plugins/DataTables/Buttons-1.5.1/js/buttons.bootstrap4.js",
                "~/Scripts/Plugins/DataTables/JSZip-2.5.0/jszip.min.js",
                "~/Scripts/Plugins/DataTables/pdfmake-0.1.36/pdfmake.min.js",
                "~/Scripts/Plugins/DataTables/pdfmake-0.1.36/vfs_fonts.js",
                "~/Scripts/Plugins/DataTables/Buttons-1.5.1/js/buttons.html5.min.js",
                "~/Scripts/Plugins/DataTables/Buttons-1.5.1/js/buttons.print.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/Plugins/moment.js"));

            // Components.
            bundles.Add(new ScriptBundle("~/bundles/site-loader").Include("~/Scripts/Compiled/Component/site-loader.js"));
            bundles.Add(new ScriptBundle("~/bundles/site-alert").Include("~/Scripts/Compiled/Component/site-alert.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable-service").Include("~/Scripts/Compiled/Component/datatable-service.js"));

            // Pages.
            bundles.Add(new ScriptBundle("~/bundles/demo-scripts").Include("~/Scripts/Compiled/Page/datatables-demo.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Scripts/Plugins/DataTables/datatables-bs4.css",
                "~/Scripts/Plugins/DataTables/Buttons-1.5.1/css/buttons.bootstrap4.css"));
        }
    }
}
