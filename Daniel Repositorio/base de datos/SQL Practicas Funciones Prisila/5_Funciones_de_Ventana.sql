--5: Funciones de Ventana Utilizando RANK, DENSE_RANK y ROW_NUMBER:

-- a) Obtener un ranking global por precio (descendente)
SELECT ID, Nombre, Precio, RANK() OVER (ORDER BY Precio DESC) AS Ranking_Global
FROM Productos;

-- b) Obtener un ranking por categoría usando RANK()
SELECT ID, Nombre, Categoria, Precio, RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Ranking_Por_Categoria
FROM Productos;

-- c) Mostrar el número de fila por categoría con ROW_NUMBER()
SELECT ID, Nombre, Categoria, Precio, ROW_NUMBER() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Fila_Por_Categoria
FROM Productos;

-- d) Comparar los resultados de RANK vs DENSE_RANK
SELECT ID, Nombre, Categoria, Precio, RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Rank_Con_Saltos,
DENSE_RANK() OVER (PARTITION BY Categoria ORDER BY Precio DESC) AS Rank_Sin_Saltos
FROM Productos;
