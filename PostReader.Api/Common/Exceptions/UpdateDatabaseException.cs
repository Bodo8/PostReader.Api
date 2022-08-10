using System;

namespace PostReader.Api.Common.Exceptions
{
    class UpdateDatabaseException : Exception
    {
        public UpdateDatabaseException() : base() { }

        public UpdateDatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
