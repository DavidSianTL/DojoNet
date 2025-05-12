-- Crear la base de datos
CREATE DATABASE FuncionesSQLDemo;
GO

-- Usar la base de datos
USE FuncionesSQLDemo;
GO

-- Crear tabla de empleados
CREATE TABLE Empleados (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    FechaIngreso DATE,
    Salario DECIMAL(10,2),
    Departamento VARCHAR(30)
);

-- Insertar datos de ejemplo
INSERT INTO Empleados (ID, Nombre, FechaIngreso, Salario, Departamento)
VALUES 
(1, 'Ana', '2020-03-01', 1500, 'Ventas'),
(2, 'Luis', '2019-06-15', 1800, 'TI'),
(3, 'Marta', '2022-01-20', 2000, 'Ventas'),
(4, 'Carlos', '2021-09-10', 1700, 'Marketing'),
(5, 'Elena', '2018-02-25', 2100, 'TI');
