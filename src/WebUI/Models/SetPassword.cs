using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class SetPassword
{
    [Required]
    [Display(Name = "userId")]
    public string userId { get; set; }
    [Required]
    [Display(Name = "NewPassword")]
    public string NewPassword { get; set; }
    [Display(Name = "ConfirmNewPassword")]
    [Compare("Password", ErrorMessage = "NewPassword and Confirmation NewPassword do not match")]
    public string ConfirmNewPassword { get; set; }
}
