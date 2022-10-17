namespace CRUDNewsApi.Helpers.EmailsTemplates
{
    public class RegisteredUser
    {
        public static string composeHTML()
        {
            return $"<h1>Activate email</h1>" +
                $"<h2>User registered successfully to activate click on the link below</h2>" +
                $"<a href=''>";
        }
    }
}
