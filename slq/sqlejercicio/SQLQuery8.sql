CREATE PROCEDURE sp_ModificarUsuario
    @id_usuario INT,
    @nom_usuario VARCHAR(100),
    @contrasenia VARCHAR(255),
    @fk_id_estado INT
AS
BEGIN
    DECLARE @hashed_password VARBINARY(64) = HASHBYTES('SHA2_256', CONVERT(VARBINARY, @contrasenia));

    UPDATE Usuario
    SET nom_usuario = @nom_usuario,
        contrasenia = @hashed_password,
        fk_id_estado = @fk_id_estado
    WHERE id_usuario = @id_usuario;

    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, 'Modificación de usuario');
END;
GO
