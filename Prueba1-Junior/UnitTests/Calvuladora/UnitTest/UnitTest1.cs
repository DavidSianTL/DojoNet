namespace UnitTest
{
    public class TestCalculadora
    {
        [Fact(DisplayName = "La suma debe devolver la cantidad correcta")]
        public void TestSumar()
        {
            // Arrange
            var logger = new ConsoleLogger();
            var calculadora = new calculadoraBasica(logger);
            // Act
            double result = calculadora.Sumar(2, 3);
            // Assert
            Assert.Equal(5, result);
            Console.WriteLine($"Resultado de Sumar: {result}");
        }





    }
}
