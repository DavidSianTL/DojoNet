use SistemaSeguridad
GO
-- Procedimiento para dar de alta un usuario
CREATE PROCEDURE sp_mantenientoUsr
    @p_tipo_op VARCHAR(2),
	@p_tipo_oper VARCHAR(2),
	@s_debug  VARCHAR(2),
    @i_usuario VARCHAR(50),
	@i_nom_usuario VARCHAR(50),
	@i_id_usuario INT,
    @i_contrasenia VARCHAR(255)
AS
BEGIN
	DECLARE @nombre_procedimiento NVARCHAR(128);
	IF (@p_tipo_op = 'I') 
	BEGIN
		IF (@s_debug = 'S')
		BEGIN
			
			SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
			PRINT 'Ejecuciòn del procedimiento' + @nombre_procedimiento;
			PRINT 'PARAMETROS ENVIADOS @p_tipo_op=' + @p_tipo_op + ', @s_debug= ' + @s_debug +  CHAR(13) + CHAR(10)+'@i_usuario= ' + @i_usuario +', @i_nom_usuario='+ @i_nom_usuario +  CHAR(13) + CHAR(10)+', @i_contrasenia'+@i_contrasenia;
		END 
		BEGIN TRY

			DECLARE @p_password_encriptada VARCHAR(64);

			-- Encriptar la contraseña usando HASHBYTES (SHA-256)
			SET @p_password_encriptada = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @i_contrasenia), 2);

			-- Insertar el nuevo usuario
			INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
			VALUES (@i_usuario, @i_nom_usuario, @p_password_encriptada, 1);
		END TRY
		BEGIN CATCH
			INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
			VALUES (NULL, ERROR_MESSAGE(),'alta_usuario');

		END CATCH


	END
	ELSE
		IF (@p_tipo_op = 'D') 
		BEGIN
			IF (@s_debug = 'S')
			BEGIN
				
				SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
				PRINT 'Ejecuciòn del procedimiento' + @nombre_procedimiento;
				PRINT 'PARAMETROS ENVIADOS @p_tipo_op=' + @p_tipo_op + ', @s_debug= ' + @s_debug +  CHAR(13) + CHAR(10)+'@i_usuario= ' + @i_usuario +', @i_nom_usuario='+ @i_nom_usuario +  CHAR(13) + CHAR(10)+', @i_contrasenia'+@i_contrasenia;
				PRINT '@i_id_usuario= ' + @i_id_usuario;
			END 

			BEGIN TRY
				UPDATE Usuarios
				SET fk_id_estado = 4
				WHERE id_usuario = @i_id_usuario;
			END TRY
			BEGIN CATCH
				
				SET @nombre_procedimiento = OBJECT_NAME(@@PROCID);
				INSERT INTO LogErrores (fk_id_usuario, mensaje_error, procedimiento)
				VALUES (NULL, ERROR_MESSAGE(),@nombre_procedimiento);
			END CATCH

		END
	
END;
GO

