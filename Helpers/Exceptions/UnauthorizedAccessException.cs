using System.Globalization;

namespace CRUDNewsApi.Helpers.Exceptions
{
    public class UnauthorizedAccessException :Exception
    {
        public UnauthorizedAccessException() : base() { }

        public UnauthorizedAccessException(string message) : base(message) { }

        public UnauthorizedAccessException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
