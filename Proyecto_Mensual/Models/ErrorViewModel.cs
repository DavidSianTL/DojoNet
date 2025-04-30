namespace Crud.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public string? Mensaje { get; set; } 
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
