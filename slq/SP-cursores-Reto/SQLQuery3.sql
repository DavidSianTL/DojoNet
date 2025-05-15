CREATE PROCEDURE sp_aumentar_salario_empleado
AS
BEGIN
    UPDATE empleados
    SET Salario = 
        CASE Puesto
            WHEN 'Analista' THEN Salario * 1.15
            WHEN 'Gerente' THEN Salario * 1.30
            WHEN 'SubGerente' THEN Salario * 1.20
            ELSE Salario
        END;

    INSERT INTO logs (Mensaje, FechaLog)
    VALUES ('Salarios actualizados según puesto', GETDATE());
END;
