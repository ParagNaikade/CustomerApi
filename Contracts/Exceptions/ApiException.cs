using System;

namespace CustomerApi.Contracts.Exceptions
{
    public class ApiException : Exception
    {
        public int HttpStatusCode { get; set; }

        public ApiException(string message)
            : base(message)
        {
            HttpStatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        }
    }
}
