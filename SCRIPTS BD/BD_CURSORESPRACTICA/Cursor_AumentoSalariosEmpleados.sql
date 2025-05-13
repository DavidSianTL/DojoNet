DECLARE @id INT, @rendimiento VARCHAR(40);

DECLARE cursor_empleados CURSOR FOR
SELECT Id_Empleado, Rendimiento FROM Empleados;

OPEN cursor_empleados;

FETCH NEXT FROM cursor_empleados INTO @id, @rendimiento;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @rendimiento = 'Alto'
        EXEC SP_Aumentar_Salario_Rendimiento_Alto @e_id_empleado = @id;
    ELSE IF @rendimiento = 'Medio'
        EXEC SP_Aumentar_Salario_Rendimiento_Medio @e_id_empleado = @id;
    ELSE IF @rendimiento = 'Bueno'
        EXEC SP_Aumentar_Salario_Rendimiento_Bueno @e_id_empleado = @id;
    ELSE IF @rendimiento = 'Bajo'
        EXEC SP_Salario_Rendimiento_Bajo @e_id_empleado = @id;

    EXEC SP_Aumentar_Salario_Empleado @e_id_empleado = @id;

    FETCH NEXT FROM cursor_empleados INTO @id, @rendimiento;
END

CLOSE cursor_empleados;
DEALLOCATE cursor_empleados;
