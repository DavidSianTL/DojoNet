CREATE PROCEDURE sp_aumento_precio_tipo_producto
	@id INT
AS
BEGIN
	DECLARE 
		@tipo NVARCHAR(50),
		@costo DECIMAL(10,2),
		@nuevoCosto DECIMAL(10,2),
		@nombre NVARCHAR(50),
		@mensaje NVARCHAR(100);

	BEGIN TRY
		SELECT @tipo = tipo, @costo = costo, @nombre = nombre FROM Productos WHERE id = @id;

		
		IF @tipo = 'Importados'
		BEGIN
			SET @nuevoCosto = @costo * 1.44;
			SET @mensaje = 'Aumento del 44% aplicado a producto importado $_$';
		END
		ELSE IF @tipo = 'Nacionales'
		BEGIN
			SET @nuevoCosto = @costo * 1.12;
			SET @mensaje = 'Aumento del 12% aplicado a producto nacional $_$';
		END
		ELSE
		BEGIN
			SET @nuevoCosto = @costo;
			SET @mensaje = 'Sin aumento: tipo de producto desconocido unu';
		END

		--  actualizar sollo si se aumentó
		IF @nuevoCosto <> @costo
			UPDATE Productos SET costo = @nuevoCosto WHERE id = @id;

		-- Logger xd
		INSERT INTO Log_productos(fk_id_producto, nombre_producto, accion)
		VALUES (@id, @nombre, @mensaje);
	END TRY
	BEGIN CATCH
		DECLARE @error NVARCHAR(250) = ERROR_MESSAGE();

		--logger malo >:c
		INSERT INTO Log_productos(fk_id_producto, nombre_producto, accion, mensaje_error)
		VALUES (@id, ISNULL(@nombre, '-UNKNOWN-'), 'AUMENTO_PRECIO_FAIL', @error);
	END CATCH
END;
