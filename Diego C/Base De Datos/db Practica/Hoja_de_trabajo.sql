
--Consulta todos los empleados
SELECT *
FROM Empleados
WHERE Activo = 1;
GO
--Muestra los pagos en mayo

SELECT *
FROM Pagos
WHERE MONTH(FechaPago) = 5 AND YEAR(FechaPago) = 2025;
GO

--Muestra el total a cada empleado
SELECT 
    E.EmpleadoID,
    E.Nombre,
    E.Apellido,
    SUM(P.MontoPagado) AS TotalPagado
FROM Empleados E
JOIN Planilla PL ON E.EmpleadoID = PL.EmpleadoID
JOIN Pagos P ON PL.PlanillaID = P.PlanillaID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido;
GO

--Usa fn_CalcularEdad para listar empleados con sus edades.

SELECT 
    EmpleadoID,
    Nombre,
    Apellido,
    FechaNacimiento,
    dbo.fn_CalcularEdad(FechaNacimiento) AS Edad
FROM Empleados;
GO


--Ejecuta sp_RegistrarPago para un empleado que aún no tenga pago.



EXEC sp_RegistrarPago @EmpleadoID = 2, @Mes = 4, @Anio = 2025, @MetodoPago = 'Transferencia';
GO

--Revisa la tabla de logs
SELECT * FROM Logs ORDER BY Fecha DESC;
GO


--Ejecuta sp_RegistrarPago con un mes o año sin planilla registrada (para disparar el error).


EXEC sp_RegistrarPago @EmpleadoID = 1,@Mes = 6,@Anio = 2024,@MetodoPago = 'Transferencia';
GO

--Verifica que el error se registró correctamente en la tabla Logs.

SELECT TOP 5 * 
FROM Logs 
ORDER BY Fecha DESC;
GO


--Crea una vista resumen

CREATE VIEW vw_ResumenPagosEmpleado AS
SELECT 
    E.EmpleadoID,
    CONCAT(E.Nombre, ' ', E.Apellido) AS NombreCompleto,
    E.SalarioBase,
    ISNULL(SUM(Pla.Bonos), 0) AS TotalBonos,
    ISNULL(SUM(Pla.Deducciones), 0) AS TotalDeducciones,
    ISNULL(SUM(Pag.MontoPagado), 0) AS TotalPagado
FROM Empleados E
LEFT JOIN Planilla Pla ON E.EmpleadoID = Pla.EmpleadoID
LEFT JOIN Pagos Pag ON Pla.PlanillaID = Pag.PlanillaID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido, E.SalarioBase;
GO

SELECT * FROM vw_ResumenPagosEmpleado;
GO

--Agregar un trigger que registre cambios
CREATE TRIGGER trg_LogUpdateEmpleados
ON Empleados
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmpleadoID INT;
    DECLARE @Cambios NVARCHAR(MAX) = '';
    DECLARE @MensajeFinal NVARCHAR(MAX);

    SELECT TOP 1 @EmpleadoID = EmpleadoID FROM inserted;

  
    SELECT @Cambios = STRING_AGG(Campo, ', ')
    FROM (
        SELECT 'Nombre' AS Campo FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.Nombre <> d.Nombre
        UNION ALL
        SELECT 'Apellido' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.Apellido <> d.Apellido
        UNION ALL
        SELECT 'FechaNacimiento' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.FechaNacimiento <> d.FechaNacimiento
        UNION ALL
        SELECT 'FechaIngreso' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.FechaIngreso <> d.FechaIngreso
        UNION ALL
        SELECT 'Puesto' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.Puesto <> d.Puesto
        UNION ALL
        SELECT 'SalarioBase' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.SalarioBase <> d.SalarioBase
        UNION ALL
        SELECT 'Activo' FROM inserted i INNER JOIN deleted d ON i.EmpleadoID = d.EmpleadoID WHERE i.Activo <> d.Activo
    ) CambiosDetectados;

  
    SET @MensajeFinal = CONCAT(
        'Empleado actualizado. ID: ', @EmpleadoID,
        '. Cambios detectados en columnas: ', ISNULL(@Cambios, 'Ninguno')
    );

 
    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    VALUES ('trg_LogUpdateEmpleados', @MensajeFinal, 0);
END;

UPDATE Empleados
SET Puesto = 'Gerente', SalarioBase = 2000
WHERE EmpleadoID = 2;
GO

SELECT * FROM Logs ORDER BY LogID DESC;