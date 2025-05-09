-- a) Ranking global por precio (descendente)
SELECT Nombre, Precio, RANK() OVER(ORDER BY Precio DESC) AS Ranking_Global FROM Productos;

-- b) Ranking por categoría
SELECT Nombre, Categoria, Precio, RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS Ranking_Categoria FROM Productos;

-- c) Número de fila por categoría
SELECT Nombre, Categoria, Precio, ROW_NUMBER() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS Numero_Fila FROM Productos;

-- d) Comparación RANK vs DENSE_RANK
SELECT Nombre, Categoria, Precio,
       RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS Rank_Normal,
       DENSE_RANK() OVER(PARTITION BY Categoria ORDER BY Precio DESC) AS Dense_Rank
FROM Productos;
