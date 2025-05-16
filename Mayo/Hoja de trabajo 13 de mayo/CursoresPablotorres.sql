USE master;
GO
IF DB_ID('CursoresPablotorres') IS NOT NULL
BEGIN
    ALTER DATABASE CursoresPablotorres SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CursoresPablotorres;
END;
GO

CREATE DATABASE CursoresPablotorres;
GO

USE CursoresPablotorres;
GO


CREATE TABLE empleados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100),
    Puesto VARCHAR(50),
    Rendimiento VARCHAR(10),
    Salario DECIMAL(10,2),
    FechaIngreso DATE DEFAULT GETDATE()
);
CREATE TABLE productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100),
    Costo DECIMAL(10,2),
    Tipo VARCHAR(50),
    Stock INT,
    FechaRegistro DATE
);
CREATE TABLE logs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME DEFAULT GETDATE(),
    Entidad VARCHAR(50),
    EntidadId INT,
    Accion VARCHAR(100),
    Detalle VARCHAR(200)
);
GO


CREATE PROCEDURE sp_ingresar_empleado
    @Nombre VARCHAR(100),
    @Puesto VARCHAR(50),
    @Rendimiento VARCHAR(10),
    @Salario DECIMAL(10,2)
AS
BEGIN
    INSERT INTO empleados (Nombre,Puesto,Rendimiento,Salario)
    VALUES (@Nombre,@Puesto,@Rendimiento,@Salario);

    DECLARE @NewId INT = SCOPE_IDENTITY();
    INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
    VALUES ('empleados', @NewId, 'INSERT', 'Empleado ingresado');
END;
GO

USE CursoresPablotorres
GO

CREATE PROCEDURE sp_aumentar_salario_empleado
    @EmpleadoId INT
AS
BEGIN
    DECLARE 
	@Puesto VARCHAR(50),
	@Salario DECIMAL(10,2), 
	@Factor DECIMAL(5,4);

    SELECT @Puesto=Puesto, @Salario=Salario FROM empleados WHERE Id=@EmpleadoId;

    SET @Factor = CASE @Puesto
        WHEN 'Analista'   THEN 1.15
        WHEN 'Gerente'    THEN 1.30
        WHEN 'SubGerente' THEN 1.20
        ELSE 1
    END;

    UPDATE empleados
       SET Salario = @Salario * @Factor
     WHERE Id = @EmpleadoId;

    INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
    VALUES ('empleados', @EmpleadoId, 'UPDATE', 'Aumento base aplicado');
END;
GO


CREATE PROCEDURE sp_aumentar_salario_rendimiento
    @EmpleadoId INT
AS
BEGIN
    DECLARE @Rend VARCHAR(10), @Sal DECIMAL(10,2), @Bono DECIMAL(5,4), @Detalle VARCHAR(100);
    SELECT @Rend=Rendimiento, @Sal=Salario FROM empleados WHERE Id=@EmpleadoId;

    SET @Bono = CASE @Rend
        WHEN 'Bueno' THEN 1.02
        WHEN 'Medio' THEN 1.05
        WHEN 'Alto'  THEN 1.07
        ELSE 1
    END;

    UPDATE empleados
       SET Salario = @Sal * @Bono
     WHERE Id = @EmpleadoId;

    SET @Detalle = CASE @Rend
        WHEN 'Bueno' THEN 'bono por rendimiento Bueno'
        WHEN 'Medio' THEN 'bono por rendimiento Medio'
        WHEN 'Alto'  THEN 'bono por rendimiento Alto'
        ELSE 'Sin bono por rendimiento bajo'
    END;

    INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
    VALUES ('empleados', @EmpleadoId, 'UPDATE', @Detalle);
END;
GO

CREATE PROCEDURE sp_notificacion_rendimiento
    @EmpleadoId INT
AS
BEGIN
    DECLARE @Rend VARCHAR(10);
    SELECT @Rend = Rendimiento FROM empleados WHERE Id = @EmpleadoId;

    IF @Rend = 'Bajo'
        INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
        VALUES ('empleados', @EmpleadoId, 'NOTIFY', 'Advertencia por bajo rendimiento');
END;
GO


CREATE PROCEDURE sp_ingresar_producto
    @Nombre VARCHAR(100),
    @Costo DECIMAL(10,2),
    @Tipo VARCHAR(50),
    @Stock INT,
    @FechaRegistro DATE
AS
BEGIN
    INSERT INTO productos (Nombre,Costo,Tipo,Stock,FechaRegistro)
    VALUES (@Nombre,@Costo,@Tipo,@Stock,@FechaRegistro);

    DECLARE @NewId INT = SCOPE_IDENTITY();
    INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
    VALUES ('productos', @NewId, 'INSERT', 'Producto ingresado');
END;
GO
 
 

CREATE PROCEDURE sp_notificar_stock_bajo
AS
BEGIN
    DECLARE cur CURSOR FAST_FORWARD FOR
        SELECT Id, Stock FROM productos WHERE Stock <= 10;
    OPEN cur;
    DECLARE @Id INT, @Stock INT;
    FETCH NEXT FROM cur INTO @Id, @Stock;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO logs(Entidad,EntidadId,Accion,Detalle)
        VALUES ('productos', @Id, 'NOTIFY', CONCAT('Stock bajo: ', @Stock));
        FETCH NEXT FROM cur INTO @Id, @Stock;
    END;
    CLOSE cur; 
    DEALLOCATE cur;
END;
GO

CREATE PROCEDURE sp_aumento_precio_tipo_producto
    @Tipo VARCHAR(50),
    @Porcentaje DECIMAL(5,2)
AS
BEGIN
    UPDATE productos
       SET Costo = Costo * (1 + @Porcentaje/100)
     WHERE Tipo = @Tipo;

    INSERT INTO logs (Entidad, EntidadId, Accion, Detalle)
    VALUES (
      'productos',
       0,
      'UPDATE',
      CONCAT('Aumento de precio de ', @Tipo, ' en ', @Porcentaje, '%')
    );
END;
GO


EXEC sp_ingresar_empleado 'Diego Fernández', 'SubGerente', 'Medio', 63000.00;
EXEC sp_ingresar_empleado 'Verónica Castillo', 'Analista', 'Alto', 47000.00;
GO


DECLARE cursorEmp CURSOR FAST_FORWARD FOR SELECT Id FROM empleados;
OPEN cursorEmp;
DECLARE @EmpId INT;
FETCH NEXT FROM cursorEmp INTO @EmpId;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_aumentar_salario_empleado @EmpId;
    EXEC sp_aumentar_salario_rendimiento @EmpId;
    EXEC sp_notificacion_rendimiento @EmpId;
    FETCH NEXT FROM cursorEmp INTO @EmpId;
END;
CLOSE cursorEmp;
DEALLOCATE cursorEmp;

DECLARE cursorProd CURSOR FAST_FORWARD FOR SELECT Id FROM productos;
OPEN cursorProd;
DECLARE @ProdId INT;
FETCH NEXT FROM cursorProd INTO @ProdId;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_notificar_stock_bajo;
    EXEC sp_aumento_precio_tipo_producto 'importados', 44;
    EXEC sp_aumento_precio_tipo_producto 'Nacionales', 12;
    FETCH NEXT FROM cursorProd INTO @ProdId;
END;
CLOSE cursorProd;
DEALLOCATE cursorProd;
GO

SELECT Id, Nombre, Puesto, Rendimiento, Salario, FechaIngreso
FROM empleados;
GO
