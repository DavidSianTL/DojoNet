use SistemaSeguridad
go

CREATE PROCEDURE sp_ValidarToken
    @Token NVARCHAR(MAX)
AS
BEGIN
    SELECT COUNT(*) AS TokenValido
    FROM Tokens
    WHERE Token = @Token
      AND Estado = 1
      AND FechaExpiracion > GETDATE();
END
