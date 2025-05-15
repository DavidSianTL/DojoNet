CREATE PROCEDURE sp_notificar_stock_bajo
AS
BEGIN
    INSERT INTO logs (Mensaje, FechaLog)
    SELECT 'Stock bajo para producto: ' + Nombre, GETDATE()
    FROM productos
    WHERE Stock <= 5;
END;
