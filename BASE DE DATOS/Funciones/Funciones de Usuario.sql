
use FuncionesSQLDemo
GO
-- Crear la funci�n
CREATE FUNCTION dbo.CalcularSalarioAnual(@SalarioMensual DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * 13; -- incluye bono adicional
END;
GO

-- Usar la funci�n
SELECT 
    Nombre,
    Salario,
    dbo.CalcularSalarioAnual(Salario) AS SalarioAnual
FROM Empleados;
