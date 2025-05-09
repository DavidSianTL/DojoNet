use SistemaSeguridad
GO

-- Procedimiento para dar de baja un usuario
CREATE PROCEDURE baja_usuario
    @p_id_usuario INT
AS
BEGIN
	BEGIN TRY
		UPDATE Usuarios
		SET fk_id_estado = 4
		WHERE id_usuario = @p_id_usuario;
	END TRY
	BEGIN CATCH
		DECLARE @nombre_procedimiento NVARCHAR(128);
        SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
		INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (@p_id_usuario, ERROR_MESSAGE(),@nombre_procedimiento);
	END CATCH

END;
GO
