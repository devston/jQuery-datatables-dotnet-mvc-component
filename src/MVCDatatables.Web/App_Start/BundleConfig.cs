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

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                "~/Content/Plugins/DataTables/datatables-bs4.css",
                "~/Content/Plugins/DataTables/buttons.bootstrap4.css"));
        }
    }
}
