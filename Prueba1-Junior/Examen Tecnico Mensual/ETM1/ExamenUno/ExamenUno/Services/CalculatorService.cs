using calculatorService;

namespace ExamenUno.Services
{
    public interface ICalculatorService
    {
        public Task<int> Sumar(int a, int b);
        public Task<int> Restar(int a, int b);
        public Task<int> Multiplicar(int a, int b);
        public Task<int> Dividir(int a, int b);
    }


    public class CalculatorService : ICalculatorService
    {
        private readonly CalculatorSoapClient _calculatoClient;

        public CalculatorService(CalculatorSoapClient calculatoClient) { _calculatoClient = calculatoClient; }

        public async Task<int> Sumar(int a, int b)
        {
            try
            {
                var response = await _calculatoClient.AddAsync(a, b);

                return response;

            }catch (Exception ex)
            {
                LoggerService.LogError(ex);
                return 0;
            }
        }


        public async Task<int> Restar(int a, int b)
        {
            try
            {
                var response = await _calculatoClient.SubtractAsync(a, b);

                return response;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
                return 0;
            }
        }

        public async Task<int> Multiplicar(int a, int b)
        {
            try
            {
                var response = await _calculatoClient.MultiplyAsync(a, b);
                return response;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
                return 0;
            }

        }


        public async Task<int> Dividir(int a, int b)
        {
            try
            {
                var response = await _calculatoClient.DivideAsync(a, b);
                return response;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex);
                return 0;
            }
        }



    }
}
