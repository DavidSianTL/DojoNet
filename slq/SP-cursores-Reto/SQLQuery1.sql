CREATE DATABASE ControlInventario;
GO

USE ControlInventario;
GO

-- Tabla empleados
CREATE TABLE empleados (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Puesto NVARCHAR(50),
    Rendimiento NVARCHAR(20),
    Salario DECIMAL(10,2),
    FechaIngreso DATE
);

-- Tabla productos
CREATE TABLE productos (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Costo DECIMAL(10,2),
    Tipo NVARCHAR(50),
    Stock INT,
    FechaRegistro DATE
);

-- Tabla logs
CREATE TABLE logs (
    Id INT PRIMARY KEY IDENTITY,
    Mensaje NVARCHAR(255),
    FechaLog DATETIME
);
