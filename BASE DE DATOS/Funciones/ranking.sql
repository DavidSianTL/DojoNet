SELECT 
    Nombre,
    Precio,
    RANK() OVER (ORDER BY Precio DESC) AS RankingGeneral
FROM Productos;


SELECT 
    Categoria,
    Nombre,
    Precio,
    RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS RankingPorCategoria
FROM Productos
ORDER BY Categoria, RankingPorCategoria;



-- Consulta con DENSE_RANK() por categoría
SELECT 
    Categoria,
    Nombre,
    Precio,
    DENSE_RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS DenseRanking
FROM Productos
ORDER BY Categoria, DenseRanking;

-- Consulta con ROW_NUMBER() por categoría
SELECT 
    Categoria,
    Nombre,
    Precio,
    ROW_NUMBER() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Fila
FROM Productos
ORDER BY Categoria, Fila;
