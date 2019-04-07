namespace CustomerApi.Contracts.Exceptions
{
    public class NoResultFoundException : ApiException
    {
        public NoResultFoundException(string message)
            : base(message)
        {
            HttpStatusCode = (int)System.Net.HttpStatusCode.NotFound;
        }
    }
}
