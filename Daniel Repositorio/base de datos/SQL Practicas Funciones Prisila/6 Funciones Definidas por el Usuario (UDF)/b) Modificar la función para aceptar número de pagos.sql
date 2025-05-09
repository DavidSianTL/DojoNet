--b) Modificar la función para aceptar número de pagos

CREATE FUNCTION dbo.fn_SalarioAnual (@SalarioMensual DECIMAL(10,2), @Pagos INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * @Pagos;
END;
GO

-- c) Usar la función con 14 pagos
SELECT ID, Nombre, Salario,
       dbo.fn_SalarioAnual(Salario, 14) AS Salario_Anual_14_Pagos
FROM Empleados;
