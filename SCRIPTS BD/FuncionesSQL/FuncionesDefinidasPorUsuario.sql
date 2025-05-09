USE FuncionesSQLDemo

--FUNCIONES DEFINIDAS POR EL USUARIO
--Crea una funci�n que reciba un salario mensual y devuelva el salario anual (13 pagos). Luego:

CREATE FUNCTION dbo.DevolverSalarioAnual (
    @SalarioMensual DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SalarioAnual DECIMAL(10,2);
    SET @SalarioAnual = @SalarioMensual * 13; -- Incluye bono adicional
    RETURN @SalarioAnual;
END;
GO

--a) Aplica la funci�n a la tabla Empleados.
SELECT 
    Nombre,
    Salario,
    dbo.DevolverSalarioAnual(Salario) AS SalarioAnual
FROM Empleados;

--b) Modifica la funci�n para aceptar un n�mero de pagos como par�metro.

DROP FUNCTION IF EXISTS dbo.DevolverSalarioAnual;

CREATE FUNCTION dbo.DevolverSalarioAnual (
    @SalarioMensual DECIMAL(10,2),
    @NumeroPagos INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * @NumeroPagos;
END;
GO

--c) Usa la funci�n en una consulta que calcule el salario anual con 14 pagos.
SELECT 
    Nombre,
    Salario,
    dbo.DevolverSalarioAnual(Salario, 14) AS SalarioAnual14Pagos
FROM Empleados;


--d) Modifica la funci�n para que calcule el salario anual con prestaciones.

DROP FUNCTION IF EXISTS dbo.DevolverSalarioAnual;

CREATE FUNCTION dbo.DevolverSalarioAnual (
    @SalarioMensual DECIMAL(10,2),
    @NumeroPagos INT,
    @PorcentajePrestacion DECIMAL(5,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SalarioAnual DECIMAL(10,2);
    SET @SalarioAnual = @SalarioMensual * @NumeroPagos;
    RETURN @SalarioAnual + (@SalarioAnual * @PorcentajePrestacion);
END;
GO
