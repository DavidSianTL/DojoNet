
use SistemaSeguridad
GO
-- Procedimiento para cerrar sesión de un usuario
CREATE PROCEDURE cerrar_sesion
    @p_id_usuario INT
AS
BEGIN
    -- Insertar en la bitácora la acción de cierre de sesión
    INSERT INTO Bitacora (id_usuario, accion)
    VALUES (@p_id_usuario, 'Cierre de sesión');
    
    -- Retornar un mensaje de éxito
    SELECT 'Sesión cerrada exitosamente' AS mensaje;
END;
GO
