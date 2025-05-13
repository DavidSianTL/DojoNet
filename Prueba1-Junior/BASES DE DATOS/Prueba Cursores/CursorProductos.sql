DECLARE @id INT;

DECLARE productos_cursor CURSOR FOR
	SELECT id FROM Productos;

BEGIN TRY
	OPEN productos_cursor;

	FETCH NEXT FROM productos_cursor INTO @id;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC sp_notificar_stock_bajo @id;
		EXEC sp_aumento_precio_tipo_producto @id;

		FETCH NEXT FROM productos_cursor INTO @id;
	END

	CLOSE productos_cursor;
	DEALLOCATE productos_cursor;
END TRY
BEGIN CATCH
	DECLARE @error NVARCHAR(250) = ERROR_MESSAGE();

	INSERT INTO Log_productos(nombre_producto, accion, mensaje_error)
	VALUES ('-CURSOR-', 'PROCESAR_PRODUCTOS_FAIL', @error);

	IF CURSOR_STATUS('global', 'productos_cursor') >= 0
	BEGIN
		CLOSE productos_cursor;
		DEALLOCATE productos_cursor;
	END
END CATCH;
