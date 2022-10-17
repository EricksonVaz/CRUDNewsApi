using System.Globalization;

namespace CRUDNewsApi.Helpers.Exceptions
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException() : base() { }

        public NotImplementedException(string message) : base(message) { }

        public NotImplementedException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
