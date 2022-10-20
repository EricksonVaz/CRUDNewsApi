using CRUDNewsApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.Auth
{
    public class UpdateRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
