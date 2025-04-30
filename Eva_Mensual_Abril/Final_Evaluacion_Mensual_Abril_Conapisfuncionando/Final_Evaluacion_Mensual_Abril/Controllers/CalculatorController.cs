using Microsoft.AspNetCore.Mvc;
using System.ServiceModel; 
using CalculatorService; 
public class CalculatorController : Controller
{
    private readonly CalculatorSoap _client;

    public CalculatorController()
    {
        
        var binding = new BasicHttpBinding();
        var endpoint = new EndpointAddress("http://www.dneonline.com/calculator.asmx");
        _client = new CalculatorSoapClient(binding, endpoint);
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Calculate(int num1, int num2, string operation)
    {
        try
        {
            int result = 0;

            switch (operation)
            {
                case "add":
                    result = await _client.AddAsync(num1, num2);
                    break;
                case "subtract":
                    result = await _client.SubtractAsync(num1, num2);
                    break;
                case "multiply":
                    result = await _client.MultiplyAsync(num1, num2);
                    break;
                case "divide":
                    result = await _client.DivideAsync(num1, num2);
                    break;
                default:
                    return BadRequest("Operación no válida");
            }

            return Json(new { success = true, result = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}