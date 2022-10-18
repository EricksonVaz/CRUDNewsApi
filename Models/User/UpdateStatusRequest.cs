using CRUDNewsApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdateStatusRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public EStatus status { get; set; }
    }
}
