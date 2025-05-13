CREATE DATABASE practicacursores;

CREATE TABLE empleados (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Puesto NVARCHAR(50),
    Rendimiento INT, -- escala de 1 a 10
    Salario DECIMAL(10,2),
    FechaIngreso DATE
);


CREATE TABLE productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Costo DECIMAL(10,2),
    Tipo NVARCHAR(50),
    Stock INT,
    FechaRegistro DATE
);


CREATE TABLE logs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT GETDATE(),
    TablaAfectada NVARCHAR(50),
    Accion NVARCHAR(100),
    Detalles NVARCHAR(MAX)
);
GO
--Crear empleado
CREATE PROCEDURE sp_ingresar_empleado
    @Nombre NVARCHAR(100),
    @Puesto NVARCHAR(50),
    @Rendimiento INT,
    @Salario DECIMAL(10,2),
    @FechaIngreso DATE
AS
BEGIN
    INSERT INTO empleados (Nombre, Puesto, Rendimiento, Salario, FechaIngreso)
    VALUES (@Nombre, @Puesto, @Rendimiento, @Salario, @FechaIngreso);

    INSERT INTO logs (TablaAfectada, Accion, Detalles)
    VALUES ('empleados', 'Inserción',
            CONCAT('Empleado agregado: ', @Nombre, ', Puesto: ', @Puesto));
END;
GO


--Aunmentar salario
CREATE PROCEDURE sp_aumentar_salario_empleado
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(100), @Puesto NVARCHAR(50), @Aumento FLOAT, @SalarioAnterior DECIMAL(10,2);

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Puesto, Salario FROM empleados;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Puesto, @SalarioAnterior;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Aumento = 
            CASE 
                WHEN @Puesto = 'Analista' THEN 0.15
                WHEN @Puesto = 'Gerente' THEN 0.30
                WHEN @Puesto = 'SubGerente' THEN 0.20
                ELSE 0.00
            END;

        IF @Aumento > 0
        BEGIN
            UPDATE empleados
            SET Salario = Salario * (1 + @Aumento)
            WHERE Id = @Id;

            INSERT INTO logs (TablaAfectada, Accion, Detalles)
            VALUES ('empleados', 'Aumento por puesto',
                    CONCAT('Aumento aplicado a ', @Nombre, ' (', @Puesto, '). Nuevo salario: ', @SalarioAnterior * (1 + @Aumento)));
        END;

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Puesto, @SalarioAnterior;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO

--Aumentar salario por rendimiento

CREATE PROCEDURE sp_aumentar_salario_rendimiento
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(100), @Rendimiento NVARCHAR(20),
            @Bono FLOAT, @Mensaje NVARCHAR(100), @SalarioAnterior DECIMAL(10,2);

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Rendimiento, Salario FROM empleados;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento, @SalarioAnterior;

    WHILE @@FETCH_STATUS = 0
    BEGIN
       
        IF @Rendimiento = 'Bajo'
        BEGIN
            SET @Bono = 0.00;
            SET @Mensaje = 'Sin bono por rendimiento bajo';
        END
        ELSE IF @Rendimiento = 'Medio'
        BEGIN
            SET @Bono = 0.05;
            SET @Mensaje = 'Bono por rendimiento Medio';
        END
        ELSE IF @Rendimiento = 'Bueno'
        BEGIN
            SET @Bono = 0.02;
            SET @Mensaje = 'Bono por rendimiento Bueno';
        END
        ELSE IF @Rendimiento = 'Alto'
        BEGIN
            SET @Bono = 0.07;
            SET @Mensaje = 'Bono por rendimiento Alto';
        END
        ELSE
        BEGIN
            SET @Bono = 0.00;
            SET @Mensaje = 'Rendimiento desconocido';
        END

        
        IF @Bono > 0
        BEGIN
            UPDATE empleados
            SET Salario = Salario * (1 + @Bono)
            WHERE Id = @Id;
        END

        
        INSERT INTO logs (TablaAfectada, Accion, Detalles)
        VALUES ('empleados', 'Bono por rendimiento',
                CONCAT('Empleado: ', @Nombre, ' - ', @Mensaje, '. Nuevo salario: ', FORMAT(@SalarioAnterior * (1 + @Bono), 'N2')));

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento, @SalarioAnterior;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;

GO
--Notificacion

CREATE PROCEDURE sp_notificacion_rendimiento
AS
BEGIN
    INSERT INTO logs (TablaAfectada, Accion, Detalles)
    SELECT 
        'empleados',
        'Advertencia',
        CONCAT('Advertencia por bajo rendimiento: ', Nombre)
    FROM empleados
    WHERE CAST(Rendimiento AS INT) < 4;  -- Convertir Rendimiento de VARCHAR a INT para la comparación
END;
GO

--insertar producto 
CREATE PROCEDURE sp_ingresar_producto
    @Nombre NVARCHAR(100),
    @Costo DECIMAL(10,2),
    @Tipo NVARCHAR(50),
    @Stock INT
AS
BEGIN
    INSERT INTO productos (Nombre, Costo, Tipo, Stock, FechaRegistro)
    VALUES (@Nombre, @Costo, @Tipo, @Stock, GETDATE());

    INSERT INTO logs (TablaAfectada, Accion, Detalles)
    VALUES ('productos', 'Inserción',
            CONCAT('Producto agregado: ', @Nombre, ', Tipo: ', @Tipo, ', Fecha: ', CONVERT(NVARCHAR, GETDATE(), 120)));
END;
GO

--notificar productos con stock bajo
CREATE PROCEDURE sp_notificar_stock_bajo
AS
BEGIN
    INSERT INTO logs (TablaAfectada, Accion, Detalles)
    SELECT 'productos', 'Stock bajo',
           CONCAT('Producto ', Nombre, ' tiene stock crítico: ', Stock)
    FROM productos
    WHERE Stock <= 5;
END;
GO

--aumento de precio 
CREATE PROCEDURE sp_aumento_precio_tipo_producto
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(100), @Tipo NVARCHAR(50), @CostoAnterior DECIMAL(10,2), @PorcentajeAumento FLOAT;

    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Tipo, Costo FROM productos;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Tipo, @CostoAnterior;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @PorcentajeAumento =
            CASE 
                WHEN LOWER(@Tipo) = 'importados' THEN 0.44
                WHEN LOWER(@Tipo) = 'nacionales' THEN 0.12
                ELSE 0.00
            END;

        IF @PorcentajeAumento > 0
        BEGIN
            UPDATE productos
            SET Costo = Costo * (1 + @PorcentajeAumento)
            WHERE Id = @Id;

            INSERT INTO logs (TablaAfectada, Accion, Detalles)
            VALUES ('productos', 'Aumento de precio',
                    CONCAT('Producto: ', @Nombre, ', Tipo: ', @Tipo, 
                           ', Costo anterior: ', @CostoAnterior, 
                           ', Nuevo costo: ', @CostoAnterior * (1 + @PorcentajeAumento)));
        END;

        FETCH NEXT FROM cur INTO @Id, @Nombre, @Tipo, @CostoAnterior;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO



-- cursor para aumentar salario por cada empleado
DECLARE @IdEmpleado INT;

DECLARE cursor_salario CURSOR 
FORWARD_ONLY READ_ONLY
FOR 
SELECT Id FROM empleados;

OPEN cursor_salario;
FETCH NEXT FROM cursor_salario INTO @IdEmpleado;

WHILE @@FETCH_STATUS = 0
BEGIN
   
    EXEC sp_aumentar_salario_empleado;

    FETCH NEXT FROM cursor_salario INTO @IdEmpleado;
END;

CLOSE cursor_salario;
DEALLOCATE cursor_salario;

-- cursor para ejecutar sp_aumentar_salario_rendimiento
DECLARE @IdEmpleado2 INT;

DECLARE cursor_rendimiento CURSOR 
FORWARD_ONLY READ_ONLY
FOR 
SELECT Id FROM empleados;

OPEN cursor_rendimiento;
FETCH NEXT FROM cursor_rendimiento INTO @IdEmpleado2;

WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_aumentar_salario_rendimiento;

    FETCH NEXT FROM cursor_rendimiento INTO @IdEmpleado2;
END;

CLOSE cursor_rendimiento;
DEALLOCATE cursor_rendimiento;

-- cursor para aplicar aumento por tipo de producto
DECLARE @IdProducto INT;

DECLARE cursor_producto CURSOR 
FORWARD_ONLY READ_ONLY
FOR 
SELECT Id FROM productos;

OPEN cursor_producto;
FETCH NEXT FROM cursor_producto INTO @IdProducto;

WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_aumento_precio_tipo_producto;

    FETCH NEXT FROM cursor_producto INTO @IdProducto;
END;

CLOSE cursor_producto;
DEALLOCATE cursor_producto;
GO
SELECT * FROM logs ORDER BY Fecha DESC;
GO
CREATE PROCEDURE sp_notificacion_rendimiento
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(100), @Rendimiento NVARCHAR(20);


    DECLARE cur CURSOR FOR
    SELECT Id, Nombre, Rendimiento FROM empleados
    WHERE Rendimiento = 'Bajo';

   
    OPEN cur;
    FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento;

   
    WHILE @@FETCH_STATUS = 0
    BEGIN
        
        INSERT INTO logs (TablaAfectada, Accion, Detalles)
        VALUES ('empleados', 'Advertencia', CONCAT('Advertencia por bajo rendimiento: ', @Nombre));

      
        FETCH NEXT FROM cur INTO @Id, @Nombre, @Rendimiento;
    END;

 
    CLOSE cur;
    DEALLOCATE cur;
END;
GO
EXECUTE sp_notificacion_rendimiento;

SELECT * FROM logs;
