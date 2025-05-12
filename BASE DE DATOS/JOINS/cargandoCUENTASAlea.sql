-- Insertar 100 cuentas de forma aleatoria
DECLARE @i INT = 12;
WHILE @i <= 112
BEGIN
    INSERT INTO Cuentas (CuentaID, ClienteID, TipoCuenta, Saldo)
    VALUES
    (@i + 200, @i, CASE WHEN @i % 2 = 0 THEN 'Ahorro' ELSE 'Monetario' END, ROUND(RAND() * 5000 + 1000, 2));
    
    SET @i = @i + 1;
END;
GO
