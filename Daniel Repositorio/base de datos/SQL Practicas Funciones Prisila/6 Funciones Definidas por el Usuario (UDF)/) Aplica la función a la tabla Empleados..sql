--6: Funciones Definidas por el Usuario (UDF)
-- Crear función básica para salario anual con 13 pagos
CREATE FUNCTION dbo.fn_SalarioAnual13 (@SalarioMensual DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * 13;
END;
GO

-- a) Aplicar función a la tabla Empleados
SELECT ID, Nombre, Salario,
       dbo.fn_SalarioAnual13(Salario) AS Salario_Anual_13_Pagos
FROM Empleados;
