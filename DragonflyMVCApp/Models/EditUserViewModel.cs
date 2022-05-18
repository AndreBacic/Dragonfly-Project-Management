using System.ComponentModel.DataAnnotations;

namespace DragonflyMVCApp.Models
{
    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        
        public ChangePasswordViewModel ChangePasswordView { get; set; } = null;
    }
}