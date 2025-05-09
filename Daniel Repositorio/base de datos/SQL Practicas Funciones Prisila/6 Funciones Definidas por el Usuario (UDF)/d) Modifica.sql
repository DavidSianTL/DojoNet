---d) Modifica la funciòn para que calcule el salario anual con prestaciones.

CREATE FUNCTION dbo.fn_SalarioAnualGuatemala (
    @SalarioMensual DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SalarioBase DECIMAL(10,2) = @SalarioMensual * 12;      -- 12 meses normales
    DECLARE @Aguinaldo DECIMAL(10,2) = @SalarioMensual;             -- 1 mes extra
    DECLARE @Bono14 DECIMAL(10,2) = @SalarioMensual;                -- 1 mes extra
    DECLARE @Vacaciones DECIMAL(10,2) = @SalarioMensual / 2;        -- medio mes (15 días)

    RETURN @SalarioBase + @Aguinaldo + @Bono14 + @Vacaciones;
END;
GO
----Aplicar funcion en tabla empleados
SELECT ID, Nombre, Salario,
       dbo.fn_SalarioAnualGuatemala(Salario) AS Salario_Anual_Con_Prestaciones_GT
FROM Empleados;
