  CREATE DATABASE BdCursores;
  USE BdCursores;

-- Tabla empleados
CREATE TABLE empleados (
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(25),
    Puesto NVARCHAR(25),
    Rendimiento NVARCHAR(25),
    Salario DECIMAL(10,2),
    FechaIngreso DATE
);

-- Tabla productos
CREATE TABLE productos (
    Id INT IDENTITY PRIMARY KEY,
    Nombre NVARCHAR(25),
    Costo DECIMAL(10,2),
    Tipo NVARCHAR(25),
    Stock INT,
    FechaRegistro DATE
);

-- Tabla logs
CREATE TABLE logs (
    Id INT IDENTITY PRIMARY KEY,
    Mensaje NVARCHAR(100),
    Fecha DATETIME DEFAULT GETDATE()
);
