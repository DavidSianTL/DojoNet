--encriptar contraseña
use SistemaSeguridad
GO

-- Función para encriptar la contraseña (hash SHA-256)
CREATE FUNCTION encriptar_contraseña (@plain_password VARCHAR(255))
RETURNS VARCHAR(64)
AS
BEGIN
    -- Retorna el hash de la contraseña en formato hexadecimal (SHA-256)
    RETURN CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @plain_password), 2);
END;
GO
