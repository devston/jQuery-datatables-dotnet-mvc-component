using System.Collections.Generic;

namespace MVCDatatables.Web.ViewModels.Manage
{
    /// <summary>
    /// The configure two factor view model - this is placed here temporarily to get around namespace issues.
    /// Move this once the presentation layer is using .net 5.
    /// </summary>
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}