--1. Consulta todos los empleados activos
SELECT * FROM Empleados
WHERE Activo = 1;


--2. Muestra los pagos realizados en mayo de 2025
SELECT Pa.*, E.Nombre, E.Apellido
FROM Pagos Pa
JOIN Planilla P ON Pa.PlanillaID = P.PlanillaID
JOIN Empleados E ON P.EmpleadoID = E.EmpleadoID
WHERE MONTH(Pa.FechaPago) = 5 AND YEAR(Pa.FechaPago) = 2025;


--3. Muestra el total pagado a cada empleado
SELECT 
    E.EmpleadoID,
    E.Nombre + ' ' + E.Apellido AS Empleado,
    SUM(Pa.MontoPagado) AS TotalPagado
FROM Pagos Pa
JOIN Planilla P ON Pa.PlanillaID = P.PlanillaID
JOIN Empleados E ON P.EmpleadoID = E.EmpleadoID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido;





--Funciones y procedimientos



--1. Usa fn_CalcularEdad para listar empleados con sus edades

SELECT 
    EmpleadoID,
    Nombre,
    Apellido,
    dbo.fn_CalcularEdad(FechaNacimiento) AS Edad
FROM Empleados;



--2. Ejecuta sp_RegistrarPago para un empleado que aún no tenga pago
-- Insertar nueva planilla sin pago
INSERT INTO Planilla (EmpleadoID, Mes, Anio, HorasTrabajadas, Bonos, Deducciones)
VALUES (1, 5, 2025, 160, 120.00, 40.00);

-- Ejecutar el SP
EXEC sp_RegistrarPago @EmpleadoID = 1, @Mes = 5, @Anio = 2025, @MetodoPago = 'Transferencia';


SELECT * FROM Logs ORDER BY Fecha DESC;



--Parte C: Manejo de errores y logs

-- Esto debe causar un error porque no existe planilla para junio de 2025
EXEC sp_RegistrarPago @EmpleadoID = 1, @Mes = 6, @Anio = 2025, @MetodoPago = 'Transferencia';




SELECT * 
FROM Logs 
WHERE Error = 1
ORDER BY Fecha DESC;






--Parte D: Bonus
CREATE VIEW vw_ResumenEmpleado AS
SELECT 
    E.EmpleadoID,
    E.Nombre + ' ' + E.Apellido AS Empleado,
    E.SalarioBase,
    SUM(P.Bonos) AS TotalBonos,
    SUM(P.Deducciones) AS TotalDeducciones,
    SUM(Pg.MontoPagado) AS TotalPagado
FROM Empleados E
JOIN Planilla P ON E.EmpleadoID = P.EmpleadoID
LEFT JOIN Pagos Pg ON P.PlanillaID = Pg.PlanillaID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido, E.SalarioBase;




-- Ver la vista
SELECT * FROM vw_ResumenEmpleado;




CREATE TRIGGER trg_LogCambiosEmpleados
ON Empleados
AFTER UPDATE
AS
BEGIN
    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    SELECT 
        'trg_LogCambiosEmpleados',
        CONCAT('Empleado ID ', i.EmpleadoID, ' modificado.'),
        0
    FROM inserted i;
END;



-- Prueba el trigger con una actualización
UPDATE Empleados
SET Puesto = 'Jefe de Área'
WHERE EmpleadoID = 1;




SELECT TOP 5 *
FROM Logs
WHERE Procedimiento = 'trg_LogCambiosEmpleados'
ORDER BY Fecha DESC;
