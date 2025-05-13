CREATE PROCEDURE sp_ingresar_producto
	@nombre NVARCHAR(50),
	@costo DECIMAL(10,2),
	@tipo NVARCHAR(50),
	@stock INT
AS
BEGIN
	DECLARE @fechaRegistro DATETIME = GETDATE();
	DECLARE @id INT;

	BEGIN TRY
		
		INSERT INTO Productos(nombre, costo, tipo, stock, fechaRegistro)
		VALUES (@nombre, @costo, @tipo, @stock, @fechaRegistro);

		SET @id = SCOPE_IDENTITY();

		-- Loggear
		INSERT INTO Log_productos(fk_id_producto, nombre_producto, accion, fecha_registro)
		VALUES (@id, @nombre, 'INSERT', @fechaRegistro);

		PRINT 'Producto ingresado correctamente :D';
	END TRY
	BEGIN CATCH
		DECLARE @error NVARCHAR(250) = ERROR_MESSAGE();

		-- Loggear pero error
		INSERT INTO Log_productos(nombre_producto, accion, mensaje_error)
		VALUES (@nombre, 'INSERT_FAIL', @error);

		PRINT 'Error al ingresar producto :c';
	END CATCH
END;
