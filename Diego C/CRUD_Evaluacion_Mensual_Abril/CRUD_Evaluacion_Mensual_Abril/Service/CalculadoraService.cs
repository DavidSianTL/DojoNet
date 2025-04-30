using Calculadora;  
using System;
using System.Threading.Tasks;

namespace CRUD_Evaluacion_Mensual_Abril.Services
{

    public interface ICalculadoraService
    {
        Task<int> Sumar(int num1, int num2);
        Task<int> Restar(int num1, int num2);
        Task<int> Multiplicar(int num1, int num2);
        Task<int> Dividir(int num1, int num2);
    }
    public class CalculadoraService : ICalculadoraService
    {
        // Método para sumar dos números
        public async Task<int> Sumar(int num1, int num2)
        {
            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.AddAsync(num1, num2);
                await client.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consumir el servicio SOAP: {ex.Message}");
            }
        }

        // Método para restar dos números
        public async Task<int> Restar(int num1, int num2)
        {
            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.SubtractAsync(num1, num2);
                await client.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consumir el servicio SOAP: {ex.Message}");
            }
        }

        // Método para multiplicar dos números
        public async Task<int> Multiplicar(int num1, int num2)
        {
            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.MultiplyAsync(num1, num2);
                await client.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consumir el servicio SOAP: {ex.Message}");
            }
        }

        // Método para dividir dos números
        public async Task<int> Dividir(int num1, int num2)
        {
            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.DivideAsync(num1, num2);
                await client.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consumir el servicio SOAP: {ex.Message}");
            }
        }
    }
}
