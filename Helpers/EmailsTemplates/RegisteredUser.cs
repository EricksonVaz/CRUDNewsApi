namespace CRUDNewsApi.Helpers.EmailsTemplates
{
    public class RegisteredUser
    {
        public static string composeHTML(string username, IHttpContextAccessor context)
        {
            return $"<h1>Activate email</h1>" +
                $"<h2>Hi {username} account registered successfully to activate click on the link below</h2>" +
                $"<a href=\"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}/api/v1/active\">Active</a>\r\n";
                //"<a href=\""+context.HttpContext.Request.Scheme+":"+context.HttpContext.Request.Host+"/api/v1/active\">Activate Account</a>";
        }
    }
}
