using System.ComponentModel.DataAnnotations;

namespace MVCDatatables.Presentation.Models.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
