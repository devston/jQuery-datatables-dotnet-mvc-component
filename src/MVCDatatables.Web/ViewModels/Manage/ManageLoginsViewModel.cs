using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace MVCDatatables.Web.ViewModels.Manage
{
    /// <summary>
    /// The manage logins view model - this is placed here temporarily to get around namespace issues.
    /// Move this once the presentation layer is using .net 5.
    /// </summary>
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}