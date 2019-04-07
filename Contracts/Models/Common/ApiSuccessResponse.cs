namespace CustomerApi.Contracts.Models.Common
{
    public class ApiSuccessResponse<T> : ApiResponse
    {
        public ApiSuccessResponse(T value)
        {
            Data = value;
            Status = Status.Success;
        }

        public T Data { get; set; }
    }
}
