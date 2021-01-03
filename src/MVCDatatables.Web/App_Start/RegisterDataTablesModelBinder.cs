using Mvc.JQuery.DataTables;
using System.Web;
using System.Web.Mvc;

[assembly: PreApplicationStartMethod(typeof(MVCDatatables.Web.App_Start.RegisterDataTablesModelBinder), "Start")]
namespace MVCDatatables.Web.App_Start
{
    public class RegisterDataTablesModelBinder
    {
        /// <summary>
        /// Start.
        /// </summary>
        public static void Start()
        {
            if (!ModelBinders.Binders.ContainsKey(typeof(DataTablesParam)))
            {
                ModelBinders.Binders.Add(typeof(DataTablesParam), new DataTablesModelBinder());
            }
        }
    }
}