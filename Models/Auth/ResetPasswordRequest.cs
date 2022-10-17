using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
