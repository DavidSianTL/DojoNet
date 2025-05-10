--10.	Agrega un trigger para registrar cambios en la tabla Empleados.

CREATE TRIGGER tr_RegistrarCambiosEmpleados
ON Empleados
AFTER UPDATE
AS
BEGIN
    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    SELECT 
        'UPDATE', CONCAT('Se actualizó el empleado con ID: ', i.EmpleadoID),0
    FROM inserted i;
END;
UPDATE Empleados
SET Puesto = 'Jefe de Contabilidad'
WHERE EmpleadoID = 1;  
