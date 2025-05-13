CREATE OR ALTER PROCEDURE sp_ingresar_empleado
	@nombre NVARCHAR(50),
	@puesto NVARCHAR(50),
	@rendimiento NVARCHAR(50),
	@salario DECIMAL(10,2)
AS 
BEGIN
	DECLARE @fecha_ingreso DATETIME = GETDATE();

	BEGIN TRY
		INSERT INTO Empleados(nombre, puesto, rendimiento, salario, fechaIngreso)
		VALUES (@nombre, @puesto, @rendimiento, @salario, @fecha_ingreso);

		DECLARE @id INT = SCOPE_IDENTITY();

		INSERT INTO Log_empleados(fk_id_empleado, nombre_empleado, accion)
		VALUES (@id, @nombre, 'INSERT');

		PRINT 'Empleado registrado con éxito.';
	END TRY
	BEGIN CATCH
		DECLARE @error NVARCHAR(200) = ERROR_MESSAGE();

		INSERT INTO Log_empleados(nombre_empleado, accion, mensaje_error)
		VALUES (@nombre, 'INSERT_FAIL', @error);

		PRINT 'Error al ingresar el empleado';
	END CATCH
END;
