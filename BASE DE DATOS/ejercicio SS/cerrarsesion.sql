
use SistemaSeguridad
GO
-- Procedimiento para cerrar sesi�n de un usuario
CREATE PROCEDURE cerrar_sesion
    @p_id_usuario INT
AS
BEGIN
    -- Insertar en la bit�cora la acci�n de cierre de sesi�n
    INSERT INTO Bitacora (id_usuario, accion)
    VALUES (@p_id_usuario, 'Cierre de sesi�n');
    
    -- Retornar un mensaje de �xito
    SELECT 'Sesi�n cerrada exitosamente' AS mensaje;
END;
GO
