namespace HojadeTrabajoAPI_REST.Models.Responses
{
    public class ApiResponse<T>
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }

        public ApiResponse(int code, string message, T data = default)
        {
            ResponseCode = code;
            ResponseMessage = message;
            Data = data;
        }
    }
}


