
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


CREATE TRIGGER trg_RegistroCambiosEmpleados
ON Empleados
AFTER UPDATE
AS
BEGIN
    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    SELECT 
        'trg_RegistroCambiosEmpleados',
        CONCAT('Se actualizó el empleado ID: ', i.EmpleadoID, ' - ', i.Nombre, ' ', i.Apellido),
        0
    FROM inserted i;
END;
GO

UPDATE Empleados
SET Puesto = 'Jefe de Área'
WHERE EmpleadoID = 2;
GO

SELECT * FROM Logs
ORDER BY Fecha DESC;