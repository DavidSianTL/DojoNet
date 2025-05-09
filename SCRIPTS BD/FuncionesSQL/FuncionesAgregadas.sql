USE FuncionesSQLDemo

--FUNCIONES AGREGADAS

--a) Calcular el precio promedio de todos los productos.
SELECT
	AVG(Precio) AS PrecioPromedio
FROM Productos;

--b) Mostrar el precio mínimo y máximo por categoría.
SELECT Categoria,
	MIN(Precio) AS PrecioMinimo,
	MAX(Precio) AS PrecioMaximo
FROM Productos
GROUP BY Categoria;

--c) Contar cuántos productos hay por categoría.
SELECT Categoria,
	COUNT(*) AS TotalProductos
FROM Productos
GROUP BY Categoria;

--d) Sumar el total de precios por categoría.
SELECT Categoria,
	SUM(Precio) AS TotalPrecios
FROM Productos
GROUP BY Categoria;
