--- 4: Funciones Agregadas
-- a) Calcular el precio promedio de todos los productos
SELECT AVG(Precio) AS Precio_Promedio
FROM Productos;

-- b) Mostrar el precio m�nimo y m�ximo por categor�a
SELECT Categoria,
       MIN(Precio) AS Precio_Minimo,
       MAX(Precio) AS Precio_Maximo FROM Productos GROUP BY Categoria;

-- c) Contar cu�ntos productos hay por categor�a
SELECT Categoria, COUNT(*) AS Cantidad_Productos
FROM Productos GROUP BY Categoria;

-- d) Sumar el total de precios por categor�a
SELECT Categoria, SUM(Precio) AS Total_Precios
FROM Productos GROUP BY Categoria;
