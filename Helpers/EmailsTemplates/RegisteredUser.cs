using CRUDNewsApi.Entities;

namespace CRUDNewsApi.Helpers.EmailsTemplates
{
    public class RegisteredUser
    {
        public static string composeHTML(User user, IHttpContextAccessor context)
        {
            return $"<h1>Activate email</h1>" +
                $"<h2>Hi {user.FirstName} account registered successfully to activate click on the link below</h2>" +
                $"<a href=\"{context.HttpContext.Request.Scheme}://localhost/crud-news/activate.html?id={user.Uuid}\">Active</a>\r\n";
            //$"<a href=\"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}/api/v1/active\">Active</a>\r\n";
                //"<a href=\""+context.HttpContext.Request.Scheme+":"+context.HttpContext.Request.Host+"/api/v1/active\">Activate Account</a>";
        }
    }
}
