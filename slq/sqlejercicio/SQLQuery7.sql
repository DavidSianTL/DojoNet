ALTER PROCEDURE sp_BajaUsuario
    @id_usuario INT
AS
BEGIN
   
    UPDATE Usuario
    SET fk_id_estado = 2
    WHERE id_usuario = @id_usuario;

    
    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, 'Baja l�gica de usuario');
END;
GO
