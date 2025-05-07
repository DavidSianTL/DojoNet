--cambio de contrase�a
use SistemaSeguridad
GO

-- Procedimiento para cambiar la contrase�a de un usuario
CREATE PROCEDURE cambio_contrase�a
    @p_id_usuario INT,
    @p_nueva_contrasenia VARCHAR(255)
AS
BEGIN
	BEGIN TRY
		DECLARE @p_password_encriptada VARCHAR(64);

		-- Generar hash SHA-256 de la nueva contrase�a
		SET @p_password_encriptada = dbo.encriptar_contrase�a(@p_nueva_contrasenia);

		-- Actualizar la contrase�a del usuario
		UPDATE Usuarios
		SET contrasenia = @p_password_encriptada
		WHERE id_usuario = @p_id_usuario;
	END TRY
	BEGIN CATCH
		
		INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (NULL, ERROR_MESSAGE(), ERROR_PROCEDURE());

	END CATCH
END;
GO
