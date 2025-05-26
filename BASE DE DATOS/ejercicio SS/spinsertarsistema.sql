use SistemaSeguridad
Go

CREATE PROCEDURE sp_InsertarSistema
    @NombreSistema NVARCHAR(100),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Sistemas (nombre_sistema, descripcion)
    VALUES (@NombreSistema, @Descripcion);
END
