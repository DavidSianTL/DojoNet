use SistemaSeguridad
GO

-- Procedimiento para cambiar la contraseña de un usuario
CREATE PROCEDURE cambio_contraseña
    @p_id_usuario INT,
    @p_nueva_contraseña VARCHAR(255)
AS
BEGIN
    DECLARE @p_password_encriptada VARCHAR(64);

    -- Generar hash SHA-256 de la nueva contraseña
    SET @p_password_encriptada = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @p_nueva_contraseña), 2);

    -- Actualizar la contraseña del usuario
    UPDATE Usuarios
    SET password = @p_password_encriptada
    WHERE id_usuario = @p_id_usuario;
END;
GO

