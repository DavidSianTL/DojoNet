----A.A.	Procedimientos Almacenados (Empleados)(SP):

--1. sp_ingresar_empleado
CREATE PROCEDURE sp_ingresar_empleado
    @Nombre NVARCHAR(100),
    @Puesto NVARCHAR(50),
    @Rendimiento NVARCHAR(50),
    @Salario DECIMAL(10,2),
    @FechaIngreso DATE
AS
BEGIN
    INSERT INTO empleados (Nombre, Puesto, Rendimiento, Salario, FechaIngreso)
    VALUES (@Nombre, @Puesto, @Rendimiento, @Salario, @FechaIngreso);

    INSERT INTO logs (Mensaje)
    VALUES ('Empleado ' + @Nombre + ' ingresado correctamente.');
END;

---prueba de uso.
EXEC sp_ingresar_empleado 'Daniel Roblero', 'Analista', 'Alto', 1000000, '2025-05-01';
Select * From empleados;

---2. sp_aumentar_salario_empleado
CREATE PROCEDURE sp_aumentar_salario_empleado
    @IdEmpleado INT
AS
BEGIN
    DECLARE @Puesto NVARCHAR(50), @Aumento DECIMAL(10,2);

    SELECT @Puesto = Puesto FROM empleados WHERE Id = @IdEmpleado;

    IF @Puesto = 'Analista'
        SET @Aumento = 0.15;
    ELSE IF @Puesto = 'Gerente'
        SET @Aumento = 0.30;
    ELSE IF @Puesto = 'SubGerente'
        SET @Aumento = 0.20;
    ELSE
        SET @Aumento = 0;

    UPDATE empleados
    SET Salario = Salario + (Salario * @Aumento)
    WHERE Id = @IdEmpleado;

    INSERT INTO logs (Mensaje)
    VALUES ('Salario aumentado del empleado ID:  ' + CAST(@IdEmpleado AS NVARCHAR));
END;

--prueba de uso
EXEC sp_aumentar_salario_empleado 1;
Select * From empleados;


---------3. sp_aumentar_salario_rendimiento
CREATE PROCEDURE sp_aumentar_salario_rendimiento
    @IdEmpleado INT
AS
BEGIN
    DECLARE @Rendimiento NVARCHAR(50), @Mensaje NVARCHAR(100), @Aumento DECIMAL(10,2);

    SELECT @Rendimiento = Rendimiento FROM empleados WHERE Id = @IdEmpleado;

    IF @Rendimiento = 'Bueno'
    BEGIN
        SET @Aumento = 0.02;
        SET @Mensaje = 'Bono por rendimiento Bueno';
    END
    ELSE IF @Rendimiento = 'Medio'
    BEGIN
        SET @Aumento = 0.05;
        SET @Mensaje = 'Bono por rendimiento Medio';
    END
    ELSE IF @Rendimiento = 'Alto'
    BEGIN
        SET @Aumento = 0.07;
        SET @Mensaje = 'Bono por rendimiento Alto';
    END
    ELSE
    BEGIN
        SET @Aumento = 0;
        SET @Mensaje = 'Sin bono por rendimiento Bajo';
    END

    UPDATE empleados
    SET Salario = Salario + (Salario * @Aumento)
    WHERE Id = @IdEmpleado;

    INSERT INTO logs (Mensaje)
    VALUES (@Mensaje + ' aplicado al empleado con ID ' + CAST(@IdEmpleado AS NVARCHAR));
END;

---- prueba de uso 
EXEC sp_aumentar_salario_rendimiento 1;
EXEC sp_aumentar_salario_rendimiento 3;
Select * From empleados;


---4. sp_notificacion_rendimiento
CREATE PROCEDURE sp_notificacion_rendimiento
AS
BEGIN
    DECLARE @IdEmpleado INT, @Nombre NVARCHAR(100), @Rendimiento NVARCHAR(50);

    DECLARE cursor_rendimiento CURSOR FOR
        SELECT Id, Nombre, Rendimiento FROM empleados;

    OPEN cursor_rendimiento;

    FETCH NEXT FROM cursor_rendimiento INTO @IdEmpleado, @Nombre, @Rendimiento;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Rendimiento = 'Bajo'
        BEGIN
            INSERT INTO logs (Mensaje)
            VALUES ('Advertencia por bajo rendimiento: ' + @Nombre + ' (ID: ' + CAST(@IdEmpleado AS NVARCHAR) + ')');
        END

        FETCH NEXT FROM cursor_rendimiento INTO @IdEmpleado, @Nombre, @Rendimiento;
    END

    CLOSE cursor_rendimiento;
    DEALLOCATE cursor_rendimiento;
END;

--- prueba de uso:
EXEC sp_notificacion_rendimiento;

SELECT * FROM logs WHERE Mensaje LIKE 'Advertencia por bajo rendimiento%';
	