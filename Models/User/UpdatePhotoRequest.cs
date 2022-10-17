using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdatePhotoRequest
    {
        [Required]
        public string Photo { get; set; }
    }
}
