using CRUDNewsApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace CRUDNewsApi.Models.User
{
    public class UpdateRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public ERoles Roles { get; set; }
    }
}
