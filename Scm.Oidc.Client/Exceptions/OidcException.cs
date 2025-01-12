using System;

namespace Com.Scm.Exceptions
{
    public class OidcException : Exception
    {
        public OidcException() { }

        public OidcException(string message) : base(message)
        {
        }
    }
}
