CREATE PROCEDURE sp_ingresar_empleado
    @Nombre NVARCHAR(100),
    @Puesto NVARCHAR(50),
    @Rendimiento NVARCHAR(20),
    @Salario DECIMAL(10,2),
    @FechaIngreso DATE
AS
BEGIN
    INSERT INTO empleados (Nombre, Puesto, Rendimiento, Salario, FechaIngreso)
    VALUES (@Nombre, @Puesto, @Rendimiento, @Salario, @FechaIngreso);

    INSERT INTO logs (Mensaje, FechaLog)
    VALUES ('Empleado ingresado: ' + @Nombre, GETDATE());
END;
