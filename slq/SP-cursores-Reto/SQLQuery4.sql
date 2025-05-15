CREATE PROCEDURE sp_aumentar_salario_rendimiento
AS
BEGIN
    DECLARE @Id INT, @Rendimiento NVARCHAR(20), @Salario DECIMAL(10,2)
    DECLARE empleado_cursor CURSOR FOR
        SELECT Id, Rendimiento, Salario FROM empleados;

    OPEN empleado_cursor;
    FETCH NEXT FROM empleado_cursor INTO @Id, @Rendimiento, @Salario;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Rendimiento = 'Bueno'
            UPDATE empleados SET Salario = @Salario * 1.02 WHERE Id = @Id;

        ELSE IF @Rendimiento = 'Medio'
            UPDATE empleados SET Salario = @Salario * 1.05 WHERE Id = @Id;

        ELSE IF @Rendimiento = 'Alto'
            UPDATE empleados SET Salario = @Salario * 1.07 WHERE Id = @Id;

        INSERT INTO logs (Mensaje, FechaLog)
        VALUES (
            'Bono por rendimiento ' + 
            CASE 
                WHEN @Rendimiento = 'Alto' THEN 'Alto'
                WHEN @Rendimiento = 'Medio' THEN 'Medio'
                WHEN @Rendimiento = 'Bueno' THEN 'Bueno'
                ELSE 'Sin bono por rendimiento bajo'
            END, GETDATE());

        FETCH NEXT FROM empleado_cursor INTO @Id, @Rendimiento, @Salario;
    END

    CLOSE empleado_cursor;
    DEALLOCATE empleado_cursor;
END;
