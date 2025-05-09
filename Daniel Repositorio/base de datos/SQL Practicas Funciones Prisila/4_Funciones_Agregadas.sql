--- 4: Funciones Agregadas
-- a) Calcular el precio promedio de todos los productos
SELECT AVG(Precio) AS Precio_Promedio
FROM Productos;

-- b) Mostrar el precio mínimo y máximo por categoría
SELECT Categoria,
       MIN(Precio) AS Precio_Minimo,
       MAX(Precio) AS Precio_Maximo FROM Productos GROUP BY Categoria;

-- c) Contar cuántos productos hay por categoría
SELECT Categoria, COUNT(*) AS Cantidad_Productos
FROM Productos GROUP BY Categoria;

-- d) Sumar el total de precios por categoría
SELECT Categoria, SUM(Precio) AS Total_Precios
FROM Productos GROUP BY Categoria;
