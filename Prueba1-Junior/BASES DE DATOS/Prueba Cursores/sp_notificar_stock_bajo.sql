CREATE PROCEDURE sp_notificar_stock_bajo
	@id INT
AS
BEGIN
	DECLARE 
		@stock INT,
		@nombre NVARCHAR(50);

	BEGIN TRY
		SELECT @stock = stock, @nombre = nombre FROM Productos WHERE id = @id;

		IF @stock <= 5
		BEGIN
			-- Logger de advertenciaaa
			INSERT INTO Log_productos(fk_id_producto, nombre_producto, accion)
			VALUES (@id, @nombre, 'Advertencia: cuidao Stock bajo');
		END
	END TRY
	BEGIN CATCH
		DECLARE @error NVARCHAR(250) = ERROR_MESSAGE();

		INSERT INTO Log_productos(fk_id_producto, nombre_producto, accion, mensaje_error)
		VALUES (@id, ISNULL(@nombre, '-UNKNOWN-'), 'STOCK_CHECK_FAIL', @error);
	END CATCH
END;

