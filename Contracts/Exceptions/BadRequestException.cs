namespace CustomerApi.Contracts.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message)
            : base(message)
        {
            HttpStatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        }
    }
}
