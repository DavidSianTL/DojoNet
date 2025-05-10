---Parte D: Bonus
---9. Crear una vista de resumen de pagos por empleado:
CREATE VIEW vw_ResumenPagosEmpleado AS
SELECT 
    E.EmpleadoID,
    E.Nombre + ' ' + E.Apellido AS NombreCompleto,
    E.SalarioBase,
    SUM(P.Bonos) AS TotalBonos,
    SUM(P.Deducciones) AS TotalDeducciones,
    SUM(Pa.MontoPagado) AS TotalPagado
FROM Empleados E
JOIN Planilla P ON E.EmpleadoID = P.EmpleadoID
LEFT JOIN Pagos Pa ON P.PlanillaID = Pa.PlanillaID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido, E.SalarioBase;

---usar la vista
SELECT * FROM vw_ResumenPagosEmpleado;


---10. Crear un trigger para registrar cambios en Empleados:
CREATE TRIGGER trg_Empleados_Update
ON Empleados
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EmpleadoID INT;
    SELECT TOP 1 @EmpleadoID = EmpleadoID FROM inserted;

    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    VALUES (
        'trg_Empleados_Update',
        CONCAT('Empleado actualizado con ID: ', @EmpleadoID),
        0
    );
END;

-- probando trigger
UPDATE Empleados SET Puesto = 'Gerente' WHERE EmpleadoID = 1;
SELECT * FROM Logs ORDER BY Fecha DESC;

