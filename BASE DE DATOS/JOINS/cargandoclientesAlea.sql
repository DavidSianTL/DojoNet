-- Insertar 100 clientes de forma aleatoria
DECLARE @i INT = 15;
WHILE @i <= 115
BEGIN
    INSERT INTO Clientes (ClienteID, Nombre, Email)
    VALUES
    (@i, 'Cliente' + CAST(@i AS NVARCHAR(3)), 'cliente' + CAST(@i AS NVARCHAR(3)) + '@email.com');
    
    SET @i = @i + 1;
END;
GO
