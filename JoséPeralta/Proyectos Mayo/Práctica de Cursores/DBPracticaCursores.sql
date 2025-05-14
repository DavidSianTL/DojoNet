USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('DBPracticaCursores') IS NOT NULL
BEGIN
    ALTER DATABASE DBPracticaCursores SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBPracticaCursores;
END
GO

-- Crear la base de datos
CREATE DATABASE DBPracticaCursores;
GO

-- Usar la base de datos recién creada
USE DBPracticaCursores;
GO

-- Tabla de empleados
CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    Cargo NVARCHAR(100),
	Salario DECIMAL(10,2),
	Rendimiento NVARCHAR(50),
    FechaIngreso DATE DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de materiales
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
	Tipo NVARCHAR(50),
    Costo DECIMAL(10,2),
	Stock INT,
	FechaIngreso DATE DEFAULT CURRENT_TIMESTAMP
);

-- Tabla de logs
CREATE TABLE Logs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    Procedimiento NVARCHAR(100),
    Mensaje NVARCHAR(MAX),
    Error BIT
);
go

-- Procedimiento para agregar empleados
CREATE PROCEDURE sp_InsertarEmpleado
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Cargo NVARCHAR(100),
	@Salario DECIMAL(10,2),
	@Rendimiento NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Empleados (Nombre, Apellido, Cargo, Salario, Rendimiento)
        VALUES (@Nombre, @Apellido, @Cargo, @Salario, @Rendimiento);

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarEmpleado', CONCAT('Empleado insertado: ', @Nombre, ' ', @Apellido), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarEmpleado', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

EXEC sp_InsertarEmpleado N'Carlos', N'Ramírez', N'Analista', 3500.00, N'Bajo';
EXEC sp_InsertarEmpleado N'María', N'González', N'SubGerente', 4000.00, N'Alto';
EXEC sp_InsertarEmpleado N'Luis', N'Torres', N'Gerente', 7500.00, N'Bueno';

SELECT * FROM Empleados;
GO

-- Crear un procedimiento para poder aumentar el salario del empleado según su puesto
CREATE PROCEDURE sp_AumentarSalarioEmpleado
    @EmpleadoID INT
AS
BEGIN
    DECLARE @Cargo NVARCHAR(100)
    DECLARE @SalarioActual DECIMAL(10,2)
    DECLARE @NuevoSalario DECIMAL(10,2)
    DECLARE @PorcentajeAumento DECIMAL(5,2)

    BEGIN TRY
        -- Obtener cargo y salario actual
        SELECT @Cargo = Cargo, @SalarioActual = Salario
        FROM Empleados
        WHERE EmpleadoID = @EmpleadoID;

        -- Determinar porcentaje según el cargo
        SET @PorcentajeAumento = 
            CASE 
                WHEN @Cargo = 'Analista' THEN 0.15
                WHEN @Cargo = 'Gerente' THEN 0.30
                WHEN @Cargo = 'SubGerente' THEN 0.20
                ELSE 0.00
            END;

        -- Calcular nuevo salario
        SET @NuevoSalario = @SalarioActual + (@SalarioActual * @PorcentajeAumento);

        -- Actualizar salario
        UPDATE Empleados
        SET Salario = @NuevoSalario
        WHERE EmpleadoID = @EmpleadoID;

        -- Registrar log
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentarSalarioEmpleado', CONCAT('Salario actualizado para el empleado ID ', @EmpleadoID), 0);

    END TRY
    BEGIN CATCH

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentarSalarioEmpleado', ERROR_MESSAGE(), 1);

    END CATCH
END;
GO

-- Aplicar un bono adicional al salario del empleado según su rendimiento.
CREATE PROCEDURE sp_AumentarSalarioRendimiento
    @EmpleadoID INT
AS
BEGIN
    DECLARE @Bono DECIMAL(5,2) = 0
    DECLARE @Salario DECIMAL(10,2)
    DECLARE @NuevoSalario DECIMAL(10,2)
    DECLARE @Mensaje NVARCHAR(MAX)
    DECLARE @Rendimiento NVARCHAR(50)

    BEGIN TRY
		
		-- Obtener el rendimiento actual
		SELECT @Rendimiento = Rendimiento FROM Empleados WHERE EmpleadoID = @EmpleadoID;

        -- Obtener salario actual
        SELECT @Salario = Salario FROM Empleados WHERE EmpleadoID = @EmpleadoID;

        -- Determinar bono según rendimiento
        IF @Rendimiento = 'Alto'
        BEGIN
            SET @Bono = 0.07;
            SET @Mensaje = 'Bono por rendimiento Alto';
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
        ELSE
        BEGIN
            SET @Bono = 0.00;
            SET @Mensaje = 'Sin bono por rendimiento Bajo';
        END

        -- Calcular nuevo salario
        SET @NuevoSalario = @Salario + (@Salario * @Bono);

        -- Actualizar salario
        UPDATE Empleados
        SET Salario = @NuevoSalario
        WHERE EmpleadoID = @EmpleadoID;

        -- Registrar log
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentarSalarioRendimiento', CONCAT('Empleado ID ', @EmpleadoID, ': ', @Mensaje), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentarSalarioRendimiento', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

-- Crear un sp (un trigger más bien) para notificar al administrador si el rendimiento del empleado es Bajo “Advertencia por bajo rendimiento”
CREATE TRIGGER tr_NotificarRendimientoEmpleado
ON Empleados
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    SELECT 
        'tr_NotificarRendimientoEmpleado',
        CONCAT('Empleado ID ', i.EmpleadoID, ': Advertencia por bajo rendimiento'),
        0
    FROM inserted i
    WHERE i.Rendimiento = 'Bajo';

END;
GO

EXEC sp_InsertarEmpleado N'Javier', N'Martínez', N'Analista', 3500.00, N'Bajo';
SELECT * FROM Logs;
GO

-- Procedimiento para agregar productos
CREATE PROCEDURE sp_InsertarProducto
    @Nombre NVARCHAR(100),
	@Tipo NVARCHAR(50),
    @Costo DECIMAL(10,2),
	@Stock INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Productos (Nombre, Tipo, Costo, Stock)
        VALUES (@Nombre, @Tipo, @Costo, @Stock);

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarProducto', CONCAT('Producto insertado: ', @Nombre), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarProducto', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO
		
EXEC sp_InsertarProducto 'Cemento', 'Importado', 5.50, 50;
EXEC sp_InsertarProducto 'Arena', 'Nacional', 3.25, 25;
EXEC sp_InsertarProducto 'Ladrillo', 'Importado', 0.45, 35;

SELECT * FROM Productos;
GO

-- Crear un sp (un trigger también) para notificar al administrador sobre los productos que tengan un mínimo de cantidad o stock
CREATE TRIGGER tr_NotificarStockProducto
ON Productos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        SELECT 
            'tr_NotificarStockProducto',
            CONCAT('Advertencia: Producto ', i.Nombre, ' con stock bajo (', i.Stock, ' unidades)'),
            0
        FROM inserted i
        WHERE i.Stock <= 10;
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('tr_NotificarStockProducto', ERROR_MESSAGE(), 1);
    END CATCH

END;
GO

EXEC sp_InsertarProducto N'Cortadora de sesped', 'Importado', 1750.00, 5;
SELECT * FROM Logs;
GO

-- Crear un sp el cual permita ingresar un porcentaje de sobre precio según el tipo de producto ingresado, por ejemplo: importados, nacionales
CREATE PROCEDURE sp_AumentoPrecioTipoProducto
    @Tipo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PorcentajeAumento DECIMAL(5,2);

    BEGIN TRY
        -- Asignar porcentaje según tipo
        SET @PorcentajeAumento = 
            CASE 
                WHEN @Tipo = 'Importado' THEN 0.44
                WHEN @Tipo = 'Nacional' THEN 0.12
                ELSE 0 -- Si el tipo no es válido, no se aplica un aumento
            END;

        -- Verificamos si se debe aplicar aumento
        IF @PorcentajeAumento = 0
        BEGIN
            INSERT INTO Logs (Procedimiento, Mensaje, Error)
            VALUES ('sp_AumentoPrecioTipoProducto', CONCAT('Tipo no válido: ', @Tipo), 1);
            RETURN;
        END

        UPDATE Productos
        SET Costo = Costo + (Costo * @PorcentajeAumento)
        WHERE Tipo = @Tipo;

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentoPrecioTipoProducto', 
                CONCAT('Aumento aplicado del ', @PorcentajeAumento, '% a productos tipo ', @Tipo), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_AumentoPrecioTipoProducto', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

-- Sección de punteros
-- Cursor para actualizar salarios por cargo
DECLARE @EmpleadoID INT

DECLARE cursor_salario CURSOR STATIC FOR
    SELECT EmpleadoID FROM Empleados;

OPEN cursor_salario

FETCH NEXT FROM cursor_salario INTO @EmpleadoID
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_AumentarSalarioEmpleado @EmpleadoID
    FETCH NEXT FROM cursor_salario INTO @EmpleadoID
END

CLOSE cursor_salario
DEALLOCATE cursor_salario

SELECT * FROM Empleados;

-- Cursor para actualizar salarios por rendimiento
DECLARE cursor_salario_rendimiento CURSOR STATIC FOR
    SELECT EmpleadoID FROM Empleados;

OPEN cursor_salario_rendimiento

FETCH NEXT FROM cursor_salario_rendimiento INTO @EmpleadoID
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_AumentarSalarioRendimiento @EmpleadoID
    FETCH NEXT FROM cursor_salario_rendimiento INTO @EmpleadoID
END

CLOSE cursor_salario_rendimiento
DEALLOCATE cursor_salario_rendimiento

SELECT * FROM Empleados;

-- Cursor para aplicar aumento a productos importados y nacionales
DECLARE @TipoProducto NVARCHAR(50)

DECLARE cursor_productos CURSOR STATIC FOR
    SELECT DISTINCT Tipo FROM Productos WHERE Tipo IN ('Importado', 'Nacional');

OPEN cursor_productos

FETCH NEXT FROM cursor_productos INTO @TipoProducto
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC sp_AumentoPrecioTipoProducto @TipoProducto
    FETCH NEXT FROM cursor_productos INTO @TipoProducto
END

CLOSE cursor_productos
DEALLOCATE cursor_productos

SELECT * FROM Productos;
