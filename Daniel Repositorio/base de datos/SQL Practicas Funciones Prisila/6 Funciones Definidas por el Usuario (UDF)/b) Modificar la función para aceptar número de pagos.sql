--b) Modificar la funci�n para aceptar n�mero de pagos

CREATE FUNCTION dbo.fn_SalarioAnual (@SalarioMensual DECIMAL(10,2), @Pagos INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * @Pagos;
END;
GO

-- c) Usar la funci�n con 14 pagos
SELECT ID, Nombre, Salario,
       dbo.fn_SalarioAnual(Salario, 14) AS Salario_Anual_14_Pagos
FROM Empleados;
