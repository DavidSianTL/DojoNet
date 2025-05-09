use SistemaSeguridad
GO

-- Procedimiento para validar acceso de un usuario
CREATE PROCEDURE validar_acceso
    @p_usuario VARCHAR(50),
    @p_contrasenia VARCHAR(255)
AS
BEGIN
	BEGIN TRY
		DECLARE @p_contrasenia_encriptada VARCHAR(64);
		DECLARE @id_usuario INT;

		-- Encriptar la contrase�a ingresada usando la funci�n de encriptar_contrase�a
		SET @p_contrasenia_encriptada = dbo.encriptar_contrase�a(@p_contrasenia);

		-- Verificar si el usuario existe y tiene la contrase�a correcta
		IF EXISTS (SELECT 1 FROM Usuarios WHERE usuario = @p_usuario AND contrasenia = @p_contrasenia_encriptada AND fk_id_estado = 1)
		BEGIN
			-- Obtener el id del usuario
			SELECT @id_usuario = id_usuario FROM Usuarios WHERE usuario = @p_usuario;

			-- Insertar en la bit�cora la acci�n de acceso exitoso
			INSERT INTO Bitacora (fk_id_usuario, accion)
			VALUES (@id_usuario, 'Acceso exitoso');

			-- Retornar el mensaje de acceso exitoso
			SELECT 'Acceso exitoso' AS mensaje;
		END
		ELSE
		BEGIN
			-- Insertar en la bit�cora la acci�n de acceso exitoso
			SELECT @id_usuario = id_usuario FROM Usuarios WHERE usuario = @p_usuario;
			INSERT INTO Bitacora (fk_id_usuario, accion)
			VALUES (@id_usuario, 'Acceso denegado');
			-- Si no se encuentra el usuario o la contrase�a es incorrecta
			SELECT 'Acceso denegado' AS mensaje;
		
			
		END
	END TRY
	BEGIN CATCH
		DECLARE @nombre_procedimiento NVARCHAR(128);
        SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
		INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
        VALUES (NULL, ERROR_MESSAGE(),@nombre_procedimiento);
	END CATCH
END;
GO

