using System.Collections.Generic;

namespace MVCDatatables.Web.ViewModels.Account
{
    /// <summary>
    /// The send code view model - this is placed here temporarily to get around namespace issues.
    /// Move this once the presentation layer is using .net 5.
    /// </summary>
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}