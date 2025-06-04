use SistemaSeguridad
go

CREATE PROCEDURE sp_RevocarToken
    @Token NVARCHAR(MAX)
AS
BEGIN
    UPDATE Tokens
    SET Estado = 0
    WHERE Token = @Token;
END
