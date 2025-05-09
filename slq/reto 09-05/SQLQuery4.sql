--1: Funciones Matemáticas Escalares

-- Valor absoluto
SELECT Nombre, Precio, ABS(Precio) AS Precio_Absoluto FROM Productos;

-- Redondear hacia arriba y hacia abajo
SELECT Nombre, Precio, CEILING(Precio) AS Precio_Arriba, FLOOR(Precio) AS Precio_Abajo FROM Productos;

-- Raíz cuadrada y cuadrado
SELECT Nombre, Precio, SQRT(Precio) AS Raiz_Cuadrada, POWER(Precio, 2) AS Cuadrado FROM Productos;

-- Signo y número aleatorio
SELECT Nombre, Precio, SIGN(Precio) AS Signo, RAND(CHECKSUM(NEWID())) AS Aleatorio FROM Productos;



--2: Funciones de Texto
-- Mayúsculas
SELECT UPPER(Nombre) AS Nombre_Mayusculas FROM Productos;

-- Longitud
SELECT Nombre, LEN(Nombre) AS Longitud_Nombre FROM Productos;

-- Primeras 5 letras
SELECT Nombre, LEFT(Nombre, 5) AS Primeras_5_Letras FROM Productos;

-- Reemplazar 'Samsung' por 'LG'
SELECT REPLACE(Nombre, 'Samsung', 'LG') AS Nombre_Reemplazado FROM Productos;



--3: Funciones de Fecha
-- Agregar columna (solo una vez)
ALTER TABLE Empleados ADD FechaIngreso DATE;

-- Fecha actual
SELECT GETDATE() AS Fecha_Actual;

-- Días en la empresa
SELECT Nombre, DATEDIFF(DAY, FechaIngreso, GETDATE()) AS Dias_En_Empresa FROM Empleados;

--  Sumar 30 días
SELECT Nombre, DATEADD(DAY, 30, FechaIngreso) AS Fecha_Mas_30_Dias FROM Empleados;

-- Año de ingreso
SELECT Nombre, YEAR(FechaIngreso) AS Año_Ingreso FROM Empleados;



--4: Funciones Agregadas
-- Precio promedio
SELECT AVG(Precio) AS Precio_Promedio FROM Productos;

-- Precio mínimo y máximo por categoría
SELECT Categoria, MIN(Precio) AS Precio_Minimo, MAX(Precio) AS Precio_Maximo
FROM Productos
GROUP BY Categoria;

-- Contar productos por categoría
SELECT Categoria, COUNT(*) AS Cantidad_Productos FROM Productos GROUP BY Categoria;

--  Sumar precios por categoría
SELECT Categoria, SUM(Precio) AS Total_Precios FROM Productos GROUP BY Categoria;



--5: Funciones de Ventana
-- Ranking global por precio descendente
SELECT Nombre, Precio, RANK() OVER (ORDER BY Precio DESC) AS Ranking_Global FROM Productos;

-- Ranking por categoría
SELECT Categoria, Nombre, Precio, RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Ranking_Categoria
FROM Productos;

-- Número de fila por categoría
SELECT Categoria, Nombre, ROW_NUMBER() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Fila_Categoria
FROM Productos;

-- Comparar RANK vs DENSE_RANK
SELECT Categoria, Nombre, Precio,
       RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Rank_,
       DENSE_RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Dense_Rank_
FROM Productos;




--6: Funciones Definidas por el Usuario (UDF)
-- Crear función: salario anual (13 pagos)
CREATE FUNCTION dbo.SalarioAnual13 (@salarioMensual DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @salarioMensual * 13;
END;

-- Aplicar a empleados
SELECT Nombre, Salario, dbo.SalarioAnual13(Salario) AS Salario_Anual FROM Empleados;

-- b) Función con número de pagos
CREATE FUNCTION dbo.SalarioAnualFlexible (@salarioMensual DECIMAL(10,2), @pagos INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @salarioMensual * @pagos;
END;

-- c) Calcular con 14 pagos
SELECT Nombre, Salario, dbo.SalarioAnualFlexible(Salario, 14) AS Salario_Anual_14_Pagos FROM Empleados;

-- d) Con prestaciones (ej: 10% adicional)
CREATE FUNCTION dbo.SalarioConPrestaciones (
    @salarioMensual DECIMAL(10,2), 
    @pagos INT, 
    @porcentajePrestaciones DECIMAL(5,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @salarioBase DECIMAL(10,2)
    SET @salarioBase = @salarioMensual * @pagos
    RETURN @salarioBase + (@salarioBase * @porcentajePrestaciones / 100)
END;

-- Usar función con prestaciones
SELECT Nombre, Salario,
       dbo.SalarioConPrestaciones(Salario, 14, 10) AS Salario_Con_Prestaciones
FROM Empleados;
