-- a) Precio promedio de todos los productos
SELECT AVG(Precio) AS Precio_Promedio FROM Productos;

-- b) Precio mínimo y máximo por categoría
SELECT Categoria, MIN(Precio) AS Precio_Minimo, MAX(Precio) AS Precio_Maximo FROM Productos GROUP BY Categoria;

-- c) Contar productos por categoría
SELECT Categoria, COUNT(*) AS Cantidad_Productos FROM Productos GROUP BY Categoria;

-- d) Sumar total de precios por categoría
SELECT Categoria, SUM(Precio) AS Total_Precios FROM Productos GROUP BY Categoria;
