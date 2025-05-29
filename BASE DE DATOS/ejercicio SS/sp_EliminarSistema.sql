use SistemaSeguridad
Go

CREATE PROCEDURE sp_EliminarSistema
    @IdSistema INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Sistemas
    WHERE id_sistema = @IdSistema;
END
