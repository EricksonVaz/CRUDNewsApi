using CRUDNewsApi.Entities;

namespace CRUDNewsApi.Helpers.EmailsTemplates
{
    public class RecoverPassword
    {
        public static string composeHTML(User user, IHttpContextAccessor context)
        {
            return $"<h1>Recover Password</h1>" +
                $"<h2>Hi {user.FirstName}to change your password click on the link below</h2>" +
                $"<a href=\"{context.HttpContext.Request.Scheme}://localhost/crud-news/reset-password.html?id={user.PasswordResetToken}\">Active</a>\r\n";
        }
    }
}
