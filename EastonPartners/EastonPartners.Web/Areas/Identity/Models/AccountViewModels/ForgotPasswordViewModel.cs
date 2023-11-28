using System.ComponentModel.DataAnnotations;

namespace EastonPartners.Web.Areas.Identity.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}