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
    FechaIngreso DATE
);

-- Tabla de materiales
CREATE TABLE Materiales (
    MaterialID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Unidad NVARCHAR(50),
    Costo DECIMAL(10,2),
	FechaIngreso datetime default current_timestamp
);

-- Tabla de obras de construcción
CREATE TABLE Obras_Construccion (
    ObraID INT PRIMARY KEY IDENTITY(1,1),
    NombreObra NVARCHAR(150),
    Ubicacion NVARCHAR(200),
    FechaInicio DATE,
    FechaFin DATE NULL
);

-- Tabla de logs
CREATE TABLE Logs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT GETDATE(),
    Procedimiento NVARCHAR(100),
    Mensaje NVARCHAR(MAX),
    Error BIT
);
go

-- Procedimiento para empleados
CREATE PROCEDURE sp_InsertarEmpleado
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Cargo NVARCHAR(100),
    @FechaIngreso DATE
AS
BEGIN
    BEGIN TRY
        INSERT INTO Empleados (Nombre, Apellido, Cargo, FechaIngreso)
        VALUES (@Nombre, @Apellido, @Cargo, @FechaIngreso);

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarEmpleado', CONCAT('Empleado insertado: ', @Nombre, ' ', @Apellido), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarEmpleado', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

EXEC sp_InsertarEmpleado 'Carlos', 'Ramírez', 'Albañil', '2020-03-15';
EXEC sp_InsertarEmpleado 'María', 'González', 'Ingeniera Civil', '2018-07-01';
EXEC sp_InsertarEmpleado 'Luis', 'Torres', 'Supervisor', '2021-11-22';
GO

-- Procedimiento para materiales
CREATE PROCEDURE sp_InsertarMaterial
    @Nombre NVARCHAR(100),
    @Unidad NVARCHAR(50),
    @Costo DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Materiales (Nombre, Unidad, Costo)
        VALUES (@Nombre, @Unidad, @Costo);

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarMaterial', CONCAT('Material insertado: ', @Nombre), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarMaterial', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

EXEC sp_InsertarMaterial 'Cemento', 'Sacos', 5.50;
EXEC sp_InsertarMaterial 'Arena', 'Metros cúbicos', 3.25;
EXEC sp_InsertarMaterial 'Ladrillo', 'Unidades', 0.45;
GO

-- Procedimiento para obras
CREATE PROCEDURE sp_InsertarObra
    @NombreObra NVARCHAR(150),
    @Ubicacion NVARCHAR(200),
    @FechaInicio DATE,
    @FechaFin DATE = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Obras_Construccion (NombreObra, Ubicacion, FechaInicio, FechaFin)
        VALUES (@NombreObra, @Ubicacion, @FechaInicio, @FechaFin);

        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarObra', CONCAT('Obra insertada: ', @NombreObra), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_InsertarObra', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

EXEC sp_InsertarObra 'Residencial Los Álamos', 'San Salvador', '2024-01-10', NULL;
EXEC sp_InsertarObra 'Centro Comercial El Roble', 'Santa Ana', '2023-09-01', '2024-12-31';
EXEC sp_InsertarObra 'Hospital Regional', 'San Miguel', '2025-03-01', NULL;
GO
