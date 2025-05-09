-- Usar la base de datos
USE FuncionesSQL;
GO


SELECT 
    ID,
    Nombre,
    Precio,

    -- a) Valor absoluto
    ABS(Precio) AS Precio_Absoluto,

    -- b) Redondeos
    CEILING(Precio) AS Precio_Redondeado_Arriba,
    FLOOR(Precio) AS Precio_Redondeado_Abajo,

    -- c) Ra�z cuadrada y cuadrado
    SQRT(Precio) AS Raiz_Cuadrada,
    POWER(Precio, 2) AS Precio_Cuadrado,

    -- d) Signo y n�mero aleatorio
    SIGN(Precio) AS Signo,
    RAND(CHECKSUM(NEWID())) * 1000 AS Numero_Aleatorio

FROM Productos;
GO
SELECT 
    ID,
    Nombre AS NombreOriginal,
    
    -- a) Convertir a may�sculas
    UPPER(Nombre) AS NombreMayusculas,
    
    -- b) Longitud del nombre
    LEN(Nombre) AS LongitudNombre,
    
    -- c) Primeras 5 letras
    LEFT(Nombre, 5) AS Primeras5Letras,
    
    -- d) Reemplazar 'Samsung' por 'LG'
    REPLACE(Nombre, 'Samsung', 'LG') AS NombreReemplazado

FROM Productos;
GO
SELECT 
    ID,
    Nombre,
    FechaIngreso,

    -- a) Fecha actual del sistema
    GETDATE() AS FechaActual,

    -- b) D�as en la empresa
    DATEDIFF(DAY, FechaIngreso, GETDATE()) AS DiasEnEmpresa,

    -- c) Fecha de ingreso + 30 d�as
    DATEADD(DAY, 30, FechaIngreso) AS FechaMas30Dias,

    -- d) Solo el a�o de ingreso
    YEAR(FechaIngreso) AS AnioIngreso

FROM Empleados;
Go
-- a) Precio promedio de todos los productos
SELECT 
    AVG(Precio) AS PrecioPromedio
FROM Productos;

-- b) Precio m�nimo y m�ximo por categor�a
SELECT 
    Categoria,
    MIN(Precio) AS PrecioMinimo,
    MAX(Precio) AS PrecioMaximo
FROM Productos
GROUP BY Categoria;

-- c) Contar cu�ntos productos hay por categor�a
SELECT 
    Categoria,
    COUNT(*) AS TotalProductos
FROM Productos
GROUP BY Categoria;

-- d) Sumar el total de precios por categor�a
SELECT 
    Categoria,
    SUM(Precio) AS TotalPrecios
FROM Productos
GROUP BY Categoria;
GO
-- a) Ranking global por precio (mayor a menor) usando RANK
SELECT 
    ID,
    Nombre,
    Precio,
    RANK() OVER (ORDER BY Precio DESC) AS RankingGlobal
FROM Productos;

-- b) Ranking por categor�a usando RANK (hay saltos si hay empates)
SELECT 
    ID,
    Nombre,
    Categoria,
    Precio,
    RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankingPorCategoria
FROM Productos;

-- c) N�mero de fila por categor�a con ROW_NUMBER (sin empates)
SELECT 
    ID,
    Nombre,
    Categoria,
    Precio,
    ROW_NUMBER() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS FilaPorCategoria
FROM Productos;

-- d) Comparaci�n entre RANK y DENSE_RANK
SELECT 
    ID,
    Nombre,
    Categoria,
    Precio,
    RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankClasico,
    DENSE_RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankDenso
FROM Productos;
GO

--a) Aplica la funci�n a la tabla Empleados.
CREATE FUNCTION CalcularSalarioAnual13 (@SalarioMensual DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * 13;
END;
GO


SELECT 
    ID,
    Nombre,
    Salario,
    dbo.CalcularSalarioAnual13(Salario) AS SalarioAnual
FROM Empleados;
GO

-- Modificar funci�n para aceptar n�mero de pagos
CREATE FUNCTION CalcularSalarioAnualVariable (
    @SalarioMensual DECIMAL(10,2),
    @Pagos INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @SalarioMensual * @Pagos;
END;
GO


SELECT 
    ID,
    Nombre,
    Salario,
    dbo.CalcularSalarioAnualVariable(Salario, 11) AS SalarioAnual
FROM Empleados;
Go
-- Usar la funci�n con 14 pagos
SELECT 
    ID,
    Nombre,
    Salario,
    dbo.CalcularSalarioAnualVariable(Salario, 14) AS SalarioAnual14Pagos
FROM Empleados;
GO
-- Modificar funci�n para incluir prestaciones
CREATE FUNCTION CalcularSalarioConPrestaciones (
    @SalarioMensual DECIMAL(10,2),
    @Pagos INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @SalarioBase DECIMAL(10,2)
    SET @SalarioBase = @SalarioMensual * @Pagos
    RETURN @SalarioBase + (@SalarioBase * 0.10) 
END;
GO


SELECT 
    ID,
    Nombre,
    Salario,
    dbo.CalcularSalarioConPrestaciones(Salario, 13) AS SalarioAnualConPrestaciones
FROM Empleados;


