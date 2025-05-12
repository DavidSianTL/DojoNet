-- Insertar 1000 transacciones de forma aleatoria
DECLARE @i INT = 1012;
WHILE @i <= 11000
BEGIN
    INSERT INTO Transacciones (TransaccionID, CuentaID, Fecha, Monto, TipoTransaccion)
    VALUES
    (@i, (@i % 100) + 201, DATEADD(DAY, -(RAND() * 30), GETDATE()), ROUND(RAND() * 1000, 2), 
    CASE WHEN @i % 2 = 0 THEN 'Depósito' ELSE 'Retiro' END);
    
    SET @i = @i + 1;
END;
GO
