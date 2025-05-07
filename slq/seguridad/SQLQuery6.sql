--cerrarsesion

use SistemaSeguridad
GO
-- Procedimiento para cerrar sesión de un usuario
CREATE PROCEDURE cerrar_sesion
    @p_id_usuario INT
AS
BEGIN
	BEGIN TRY
		-- Insertar en la bitácora la acción de cierre de sesión
		INSERT INTO Bitacora (fk_id_usuario, accion)
		VALUES (@p_id_usuario, 'Cierre de sesión');
    
		-- Retornar un mensaje de éxito
		SELECT 'Sesión cerrada exitosamente' AS mensaje;
	END TRY
	BEGIN CATCH
		DECLARE @nombre_procedimiento NVARCHAR(128);
        SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
		INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (@p_id_usuario, ERROR_MESSAGE(),@nombre_procedimiento);

	END CATCH
END;
GO

