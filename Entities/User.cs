using System.Text.Json.Serialization;

namespace CRUDNewsApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string? PasswordResetToken { get; set; }
        public ERoles Roles { get; set; }
        public EStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Comments> Comments { get; set; }
        public List<Likes> Likes { get; set; }
        public List<Posts> Posts { get; set; }


        public static string DecodeRoles(ERoles roles)
        {
            var roleDecode = roles switch
            {
                ERoles.Admin => "Admin",
                ERoles.User => "User",
                _ => "Writer"
            };

            return roleDecode;
        }
    }
}
