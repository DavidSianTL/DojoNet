public class ApiResponse
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public object? Data { get; set; }

    public ApiResponse(string code, string message, object? data = null)
    {
        ResponseCode = code;
        ResponseMessage = message;
        Data = data;
    }
}
