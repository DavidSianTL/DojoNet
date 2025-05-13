
DECLARE @id INT;


DECLARE empleados_cursor CURSOR FOR
	SELECT id FROM Empleados;

BEGIN TRY
	
	OPEN empleados_cursor;

	
	FETCH NEXT FROM empleados_cursor INTO @id;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		EXEC dbo.sp_aumento_por_puesto @id;
		EXEC dbo.sp_bono_por_rendimiento @id;

		
		FETCH NEXT FROM empleados_cursor INTO @id;
	END

	
	CLOSE empleados_cursor;
	DEALLOCATE empleados_cursor;
END TRY
BEGIN CATCH
	DECLARE @error NVARCHAR(250) = ERROR_MESSAGE();

	
	INSERT INTO Log_empleados(nombre_empleado, accion, mensaje_error)
	VALUES ('-CURSOR-', 'PROCESAR_EMPLEADOS_FAIL', @error);

	
	IF CURSOR_STATUS('global', 'empleados_cursor') >= 0
	BEGIN
		CLOSE empleados_cursor;
		DEALLOCATE empleados_cursor;
	END
END CATCH;
