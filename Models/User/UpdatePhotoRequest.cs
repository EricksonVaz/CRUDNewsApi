using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdatePhotoRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
