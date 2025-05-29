use SistemaSeguridad
GO
-- Procedimiento para dar de alta un usuario
CREATE PROCEDURE alta_usuario
    @p_usuario VARCHAR(50),
	@p_nom_usuario VARCHAR(50),
    @p_contrasenia VARCHAR(255)
AS
BEGIN
	BEGIN TRY

		DECLARE @p_password_encriptada VARCHAR(64);

		-- Encriptar la contraseña usando HASHBYTES (SHA-256)
		SET @p_password_encriptada = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @p_contrasenia), 2);

		-- Insertar el nuevo usuario
		INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, id_estado)
		VALUES (@p_usuario, @p_nom_usuario, @p_password_encriptada, 1);
	END TRY
	BEGIN CATCH
		INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (NULL, ERROR_MESSAGE(),'alta_usuario');

	END CATCH
END;
GO

