DECLARE @nombre VARCHAR(100),
        @stock DECIMAL(10,2);

DECLARE cursor_stock_bajo CURSOR FOR
SELECT Nombre, Stock FROM Productos WHERE Stock < 10;

OPEN cursor_stock_bajo;

FETCH NEXT FROM cursor_stock_bajo INTO @nombre, @stock;

WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC SP_Notificar_Stock_Bajo
        @p_nombre = @nombre,
        @p_stock = @stock;

    FETCH NEXT FROM cursor_stock_bajo INTO @nombre, @stock;
END

CLOSE cursor_stock_bajo;
DEALLOCATE cursor_stock_bajo;
