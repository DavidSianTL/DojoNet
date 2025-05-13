CREATE OR ALTER PROCEDURE sp_bono_por_rendimiento
	@id INT
AS
BEGIN
	DECLARE 
		@rendimiento NVARCHAR(50),
		@salario DECIMAL(10,2),
		@nuevoSalario DECIMAL(10,2),
		@nombre NVARCHAR(50),
		@mensaje NVARCHAR(100),
		@error NVARCHAR(250);

	BEGIN TRY
		SELECT @rendimiento = rendimiento, @salario = salario, @nombre = nombre
		FROM Empleados WHERE id = @id;

		SET @nuevoSalario = @salario;
		SET @mensaje = 'Sin bono por rendimiento bajo';

		
		IF @rendimiento = 'Bueno'
		BEGIN
			SET @nuevoSalario = @salario * 1.02;
			SET @mensaje = 'bono por rendimiento Bueno';
		END
		ELSE IF @rendimiento = 'Medio'
		BEGIN
			SET @nuevoSalario = @salario * 1.05;
			SET @mensaje = 'bono por rendimiento Medio';
		END
		ELSE IF @rendimiento = 'Alto'
		BEGIN
			SET @nuevoSalario = @salario * 1.07;
			SET @mensaje = 'bono por rendimiento Alto';
		END
		ELSE IF @rendimiento = 'Bajo'
		BEGIN
			SET @mensaje = 'Advertencia por bajo rendimiento';
		END

		
		IF @nuevoSalario <> @salario
			UPDATE Empleados SET salario = @nuevoSalario WHERE id = @id;

		
		INSERT INTO Log_empleados(fk_id_empleado, nombre_empleado, accion)
		VALUES (@id, @nombre, @mensaje);
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();

		-- Log de error
		INSERT INTO Log_empleados(fk_id_empleado, nombre_empleado, accion, mensaje_error)
		VALUES (@id, @nombre, 'BONO_RENDIMIENTO_FAIL', @error);
	END CATCH
END;
