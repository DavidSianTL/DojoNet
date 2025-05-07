CREATE PROCEDURE sp_AltaUsuario
    @usuario VARCHAR(50),
    @nom_usuario VARCHAR(100),
    @contrasenia VARCHAR(255),
    @fk_id_estado INT
AS
BEGIN
    DECLARE @hashed_password VARBINARY(64) = HASHBYTES('SHA2_256', CONVERT(VARBINARY, @contrasenia));
    
    INSERT INTO Usuario (usuario, nom_usuario, contrasenia, fk_id_estado)
    VALUES (@usuario, @nom_usuario, @hashed_password, @fk_id_estado);

    DECLARE @id_usuario INT = SCOPE_IDENTITY();

    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, 'Alta de usuario');
END;
GO
