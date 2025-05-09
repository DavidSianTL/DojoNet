
CREATE DATABASE PabloHojadetrabajo;
GO
USE PabloHojadetrabajo;
GO


CREATE TABLE Empleados (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    FechaIngreso DATE,
    Salario DECIMAL(10,2),
    Departamento VARCHAR(30)
);


CREATE TABLE Productos (
    ID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    Precio DECIMAL(10,2),
    Categoria VARCHAR(30)
);


INSERT INTO Productos (ID, Nombre, Precio, Categoria)
VALUES
    (1, 'Laptop Lenovo', 1200.00, 'Tecnología'),
    (2, 'Smartphone Samsung', 900.00, 'Tecnología'),
    (3, 'Teclado Logitech', 60.00, 'Tecnología'),
    (4, 'Cafetera Oster', 85.00, 'Electrodomésticos'),
    (5, 'Refrigeradora LG', 650.00, 'Electrodomésticos'),
    (6, 'Batidora Philips', 85.00, 'Electrodomésticos'),
    (7, 'Mouse HP', 25.00, 'Tecnología'),
    (8, 'Monitor Dell', 250.00, 'Tecnología'),
    (9, 'Aspiradora Samsung', 300.00, 'Electrodomésticos'),
    (10, 'Tablet Huawei', 350.00, 'Tecnología');


INSERT INTO Empleados (ID, Nombre, FechaIngreso, Salario, Departamento)
VALUES
    (1, 'Ana', '2020-03-01', 1500.00, 'Ventas'),
    (2, 'Luis', '2019-06-15', 1800.00, 'TI'),
    (3, 'Marta', '2022-01-20', 2000.00, 'Ventas'),
    (4, 'Carlos', '2021-09-10', 1700.00, 'Marketing'),
    (5, 'Elena', '2018-02-25', 2100.00, 'TI');
GO


SELECT ID, Nombre, Precio, ABS(Precio) AS PrecioAbs
FROM Productos;


SELECT ID, Nombre, Precio,
       CEILING(Precio) AS PrecioArriba,
       FLOOR(Precio)   AS PrecioAbajo
FROM Productos;


SELECT ID, Nombre, Precio,
       SQRT(Precio)             AS RaizCuadrada,
       POWER(Precio, 2)         AS CuadradoPrecio
FROM Productos;


SELECT ID, Nombre, Precio,
       SIGN(Precio)                               AS SignoPrecio,
       RAND(CHECKSUM(NEWID()))                   AS NumeroAleatorio
FROM Productos;


SELECT ID, Nombre, UPPER(Nombre)                AS NombreMayusculas
FROM Productos;


SELECT ID, Nombre, LEN(Nombre)                  AS LongitudNombre
FROM Productos;


SELECT ID, Nombre, LEFT(Nombre, 5)               AS Primeras5Letras
FROM Productos;


SELECT ID, Nombre,
       REPLACE(Nombre, 'Samsung', 'LG')         AS NombreReemplazado
FROM Productos;


SELECT GETDATE()                                 AS FechaActual;


SELECT ID, Nombre,
       DATEDIFF(DAY, FechaIngreso, GETDATE())   AS DiasEnEmpresa
FROM Empleados;


SELECT ID, Nombre,
       DATEADD(DAY, 30, FechaIngreso)           AS FechaMas30Dias
FROM Empleados;


SELECT ID, Nombre,
       YEAR(FechaIngreso)                       AS AnioIngreso
FROM Empleados;


SELECT AVG(Precio)                               AS PrecioPromedio
FROM Productos;


SELECT Categoria,
       MIN(Precio)       AS PrecioMinimo,
       MAX(Precio)       AS PrecioMaximo
FROM Productos
GROUP BY Categoria;


SELECT Categoria,
       COUNT(*)          AS CantidadProductos
FROM Productos
GROUP BY Categoria;


SELECT Categoria,
       SUM(Precio)       AS TotalPrecios
FROM Productos
GROUP BY Categoria;


SELECT ID, Nombre, Precio,
       RANK() OVER(ORDER BY Precio DESC)       AS RankGlobal,
       DENSE_RANK() OVER(ORDER BY Precio DESC) AS DenseRankGlobal,
       ROW_NUMBER() OVER(ORDER BY Precio DESC) AS RowNumGlobal
FROM Productos;

SELECT ID, Nombre, Categoria, Precio,
       RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS RankPorCategoria
FROM Productos;


SELECT ID, Nombre, Categoria, Precio,
       ROW_NUMBER() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS RowNumberCategoria
FROM Productos;


SELECT ID, Nombre, Categoria, Precio,
       RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC)       AS RankCategoria,
       DENSE_RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS DenseRankCategoria
FROM Productos;


CREATE FUNCTION dbo.CalcularSalarioAnual(
    @Salario DECIMAL(10,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN @Salario * 13;
END;
GO


SELECT ID, Nombre, Salario,
       dbo.CalcularSalarioAnual(Salario)        AS SalarioAnual13Pagos
FROM Empleados;


ALTER FUNCTION dbo.CalcularSalarioAnual(
    @Salario DECIMAL(10,2),
    @NumPagos INT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN @Salario * @NumPagos;
END;
GO


SELECT ID, Nombre, Salario,
       dbo.CalcularSalarioAnual(Salario, 14)    AS SalarioAnual14Pagos
FROM Empleados;


CREATE FUNCTION dbo.CalcularSalarioConPrestaciones(
    @Salario DECIMAL(10,2),
    @NumPagos INT,
    @PorcPrestaciones DECIMAL(5,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN @Salario * @NumPagos * (1 + @PorcPrestaciones / 100);
END;
GO


SELECT ID, Nombre, Salario,
       dbo.CalcularSalarioConPrestaciones(Salario, 13, 30) AS SalarioConPrestaciones
FROM Empleados;
