CREATE PROCEDURE sp_notificacion_rendimiento
AS
BEGIN
    INSERT INTO logs (Mensaje, FechaLog)
    SELECT 'Advertencia por bajo rendimiento: ' + Nombre, GETDATE()
    FROM empleados
    WHERE Rendimiento = 'Bajo';
END;
