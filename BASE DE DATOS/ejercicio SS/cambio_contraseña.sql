use SistemaSeguridad
GO

-- Procedimiento para cambiar la contraseņa de un usuario
CREATE PROCEDURE cambio_contraseņa
    @p_id_usuario INT,
    @p_nueva_contraseņa VARCHAR(255)
AS
BEGIN
    DECLARE @p_password_encriptada VARCHAR(64);

    -- Generar hash SHA-256 de la nueva contraseņa
    SET @p_password_encriptada = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @p_nueva_contraseņa), 2);

    -- Actualizar la contraseņa del usuario
    UPDATE Usuarios
    SET password = @p_password_encriptada
    WHERE id_usuario = @p_id_usuario;
END;
GO

