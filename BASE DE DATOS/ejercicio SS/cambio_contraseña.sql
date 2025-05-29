use SistemaSeguridad
GO

-- Procedimiento para cambiar la contrase�a de un usuario
CREATE PROCEDURE cambio_contrase�a
    @p_id_usuario INT,
    @p_nueva_contrase�a VARCHAR(255)
AS
BEGIN
    DECLARE @p_password_encriptada VARCHAR(64);

    -- Generar hash SHA-256 de la nueva contrase�a
    SET @p_password_encriptada = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @p_nueva_contrase�a), 2);

    -- Actualizar la contrase�a del usuario
    UPDATE Usuarios
    SET password = @p_password_encriptada
    WHERE id_usuario = @p_id_usuario;
END;
GO

