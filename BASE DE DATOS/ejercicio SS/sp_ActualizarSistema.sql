USE SistemaSeguridad
GO

CREATE PROCEDURE sp_ActualizarSistema
    @IdSistema INT,
    @NombreSistema NVARCHAR(100),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Sistemas
    SET nombre_sistema = @NombreSistema,
        descripcion = @Descripcion
    WHERE id_sistema = @IdSistema;
END
