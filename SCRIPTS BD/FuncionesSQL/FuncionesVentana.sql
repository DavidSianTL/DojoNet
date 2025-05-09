USE FuncionesSQLDemo

--FUNCIONES VENTANA

--a) Obtener un ranking global por precio (descendente).
SELECT Nombre,
    Categoria,
    Precio,
    RANK() OVER (ORDER BY Precio DESC) AS RankingPrecio
FROM Productos;

--b) Obtener un ranking por categoría usando RANK().
SELECT Categoria,
    Nombre,
    Precio,
    RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankingCategoria
FROM Productos;

--c) Mostrar el número de fila por categoría con ROW_NUMBER().
SELECT 
    Categoria,
    Nombre,
    Precio,
    ROW_NUMBER() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS NumeroFilaCategoria
FROM Productos;

--d) Comparar los resultados de RANK vs DENSE_RANK.
SELECT 
    Categoria,
    Nombre,
    Precio,
    RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankPrecio,
    DENSE_RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS DenseRankPrecio
FROM Productos;
