using CRUDNewsApi.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdatePasswordRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
