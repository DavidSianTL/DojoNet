-- a) Función para calcular salario anual con 13 pagos
CREATE FUNCTION fn_CalcularSalarioAnual(@SalarioMensual DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * 13;
END;

-- Aplicación de la función
SELECT Nombre, dbo.fn_CalcularSalarioAnual(SalarioMensual) AS Salario_Anual FROM Empleados;

-- b) Modificación para aceptar número de pagos
ALTER FUNCTION fn_CalcularSalarioAnual(@SalarioMensual DECIMAL(10,2), @Pagos INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * @Pagos;
END;

-- c) Uso de la función con 14 pagos
SELECT Nombre, dbo.fn_CalcularSalarioAnual(SalarioMensual, 14) AS Salario_Anual FROM Empleados;

-- d) Modificación para calcular salario anual con prestaciones
ALTER FUNCTION fn_CalcularSalarioAnual(@SalarioMensual DECIMAL(10,2), @Pagos INT, @Prestaciones DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (@SalarioMensual * @Pagos) + @Prestaciones;
END;
