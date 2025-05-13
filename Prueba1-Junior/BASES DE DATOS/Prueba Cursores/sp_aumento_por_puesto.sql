CREATE PROCEDURE sp_aumento_por_puesto
	@id INT
AS
BEGIN
	DECLARE 
		@puesto NVARCHAR(50),
		@salario DECIMAL(10,2),
		@nuevoSalario DECIMAL(10,2),
		@nombre NVARCHAR(50),
		@error NVARCHAR(250);

	BEGIN TRY
		SELECT @puesto = puesto, @salario = salario, @nombre = nombre
		FROM Empleados WHERE id = @id;

		
		IF @puesto = 'Analista'
			SET @nuevoSalario = @salario * 1.15;
		ELSE IF @puesto = 'Gerente'
			SET @nuevoSalario = @salario * 1.30;
		ELSE IF @puesto = 'Subgerente'
			SET @nuevoSalario = @salario * 1.20;
		ELSE
			SET @nuevoSalario = @salario;

		
		UPDATE Empleados SET salario = @nuevoSalario WHERE id = @id;

		
		INSERT INTO Log_empleados(fk_id_empleado, nombre_empleado, accion)
		VALUES (@id, @nombre, CONCAT('AUMENTO_PUESTO_', @puesto));
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();

		
		INSERT INTO Log_empleados(fk_id_empleado, nombre_empleado, accion, mensaje_error)
		VALUES (@id, @nombre, 'AUMENTO_PUESTO_FAIL', @error);
	END CATCH
END;
