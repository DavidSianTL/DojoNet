--encriptar contrase�a
use SistemaSeguridad
GO

-- Funci�n para encriptar la contrase�a (hash SHA-256)
CREATE FUNCTION encriptar_contrase�a (@plain_password VARCHAR(255))
RETURNS VARCHAR(64)
AS
BEGIN
    -- Retorna el hash de la contrase�a en formato hexadecimal (SHA-256)
    RETURN CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @plain_password), 2);
END;
GO
