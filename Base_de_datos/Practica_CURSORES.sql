-- CREACIÓN DE TABLAS
CREATE TABLE empleados (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100),
    Puesto VARCHAR(50),
    Rendimiento INT,
    Salario DECIMAL(10,2),
    FechaIngreso DATE
);

CREATE TABLE productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100),
    Costo DECIMAL(10,2),
    Tipo VARCHAR(50),
    Stock INT,
    FechaRegistro DATE
);

CREATE TABLE logs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TablaAfectada VARCHAR(50),
    Operacion VARCHAR(100),
    FechaOperacion DATETIME DEFAULT GETDATE()
);

-- SP: INGRESAR EMPLEADO
CREATE PROCEDURE sp_ingresar_empleado
    @Nombre VARCHAR(100),
    @Puesto VARCHAR(50),
    @Rendimiento INT,
    @Salario DECIMAL(10,2),
    @FechaIngreso DATE
AS
BEGIN
    INSERT INTO empleados (Nombre, Puesto, Rendimiento, Salario, FechaIngreso)
    VALUES (@Nombre, @Puesto, @Rendimiento, @Salario, @FechaIngreso);

    INSERT INTO logs (TablaAfectada, Operacion)
    VALUES ('empleados', 'Se ingresó un nuevo empleado: ' + @Nombre);
END;

-- SP: AUMENTO SEGÚN PUESTO
CREATE PROCEDURE sp_aumentar_salario_empleado
    @EmpleadoId INT
AS
BEGIN
    DECLARE @Puesto VARCHAR(50);
    DECLARE @Aumento DECIMAL(5,2);
    DECLARE @Nombre VARCHAR(100);

    SELECT @Puesto = Puesto, @Nombre = Nombre
    FROM empleados
    WHERE Id = @EmpleadoId;

    IF @Puesto = 'Analista'
        SET @Aumento = 0.15;
    ELSE IF @Puesto = 'Gerente'
        SET @Aumento = 0.30;
    ELSE IF @Puesto = 'SubGerente'
        SET @Aumento = 0.20;
    ELSE
        SET @Aumento = 0.00;

    UPDATE empleados
    SET Salario = Salario + (Salario * @Aumento)
    WHERE Id = @EmpleadoId;

    INSERT INTO logs (TablaAfectada, Operacion)
    VALUES ('empleados', 'Se aumentó el salario del empleado: ' + @Nombre + ' con puesto: ' + @Puesto);
END;

-- SP: AUMENTO SEGÚN RENDIMIENTO
CREATE PROCEDURE sp_aumentar_salario_rendimiento
    @EmpleadoId INT
AS
BEGIN
    DECLARE @Rendimiento INT;
    DECLARE @Bono DECIMAL(5,2);
    DECLARE @Nombre VARCHAR(100);
    DECLARE @Mensaje VARCHAR(100);

    SELECT @Rendimiento = Rendimiento, @Nombre = Nombre
    FROM empleados
    WHERE Id = @EmpleadoId;

    IF @Rendimiento >= 90
    BEGIN
        SET @Bono = 0.07;
        SET @Mensaje = 'bono por rendimiento Alto';
    END
    ELSE IF @Rendimiento >= 75
    BEGIN
        SET @Bono = 0.05;
        SET @Mensaje = 'bono por rendimiento Medio';
    END
    ELSE IF @Rendimiento >= 60
    BEGIN
        SET @Bono = 0.02;
        SET @Mensaje = 'bono por rendimiento Bueno';
    END
    ELSE
    BEGIN
        SET @Bono = 0.00;
        SET @Mensaje = 'Sin bono por rendimiento bajo';
    END

    UPDATE empleados
    SET Salario = Salario + (Salario * @Bono)
    WHERE Id = @EmpleadoId;

    INSERT INTO logs (TablaAfectada, Operacion)
    VALUES ('empleados', 'Empleado: ' + @Nombre + ' - ' + @Mensaje);
END;

-- SP: NOTIFICACIÓN DE RENDIMIENTO BAJO
CREATE PROCEDURE sp_notificacion_rendimiento
AS
BEGIN
    DECLARE @Id INT, @Nombre VARCHAR(100), @Rendimiento INT;

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Rendimiento FROM empleados;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Rendimiento < 60
        BEGIN
            INSERT INTO logs (TablaAfectada, Operacion)
            VALUES ('empleados', 'Advertencia por bajo rendimiento: ' + @Nombre);
        END

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;

-- SP: INGRESAR PRODUCTO
CREATE PROCEDURE sp_ingresar_producto
    @Nombre VARCHAR(100),
    @Costo DECIMAL(10,2),
    @Tipo VARCHAR(50),
    @Stock INT,
    @FechaRegistro DATE
AS
BEGIN
    INSERT INTO productos (Nombre, Costo, Tipo, Stock, FechaRegistro)
    VALUES (@Nombre, @Costo, @Tipo, @Stock, @FechaRegistro);

    INSERT INTO logs (TablaAfectada, Operacion)
    VALUES ('productos', 'Producto ingresado: ' + @Nombre);
END;

-- SP: NOTIFICACIÓN DE STOCK BAJO
CREATE PROCEDURE sp_notificar_stock_bajo
AS
BEGIN
    DECLARE @Id INT, @Nombre VARCHAR(100), @Stock INT;

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Stock FROM productos;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Stock;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Stock < 10
        BEGIN
            INSERT INTO logs (TablaAfectada, Operacion)
            VALUES ('productos', 'Stock bajo: ' + @Nombre + ' - Cantidad: ' + CAST(@Stock AS VARCHAR));
        END

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Stock;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;

-- SP: AUMENTO PRECIO SEGÚN TIPO DE PRODUCTO
CREATE PROCEDURE sp_aumento_precio_tipo_producto
AS
BEGIN
    DECLARE @Id INT, @Nombre VARCHAR(100), @Tipo VARCHAR(50), @Costo DECIMAL(10,2), @Aumento DECIMAL(5,2);

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Tipo, Costo FROM productos;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Tipo, @Costo;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @Tipo = 'importados'
            SET @Aumento = 0.44;
        ELSE IF @Tipo = 'nacionales'
            SET @Aumento = 0.12;
        ELSE
            SET @Aumento = 0.00;

        UPDATE productos
        SET Costo = Costo + (Costo * @Aumento)
        WHERE Id = @Id;

        INSERT INTO logs (TablaAfectada, Operacion)
        VALUES ('productos', 'Aumento por tipo "' + @Tipo + '" aplicado a: ' + @Nombre);

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Tipo, @Costo;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;
