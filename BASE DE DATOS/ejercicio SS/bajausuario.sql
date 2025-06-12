use SistemaSeguridad
GO

-- Procedimiento para dar de baja un usuario
CREATE PROCEDURE baja_usuario
    @p_id_usuario INT
AS
BEGIN
    UPDATE Usuarios
    SET estado = 'Inactivo'
    WHERE id_usuario = @p_id_usuario;
END;
GO
