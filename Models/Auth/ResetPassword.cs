using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.Auth
{
    public class ResetPassword
    {
        [Required]
        public string PasswordResetToken { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
