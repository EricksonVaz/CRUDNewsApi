using CRUDNewsApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdateStatusRequest
    {
        [Required]
        public EStatus status { get; set; }
    }
}
