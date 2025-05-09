USE FuncionesSQLDemo

--FUNCIONES AGREGADAS

--a) Calcular el precio promedio de todos los productos.
SELECT
	AVG(Precio) AS PrecioPromedio
FROM Productos;

--b) Mostrar el precio m�nimo y m�ximo por categor�a.
SELECT Categoria,
	MIN(Precio) AS PrecioMinimo,
	MAX(Precio) AS PrecioMaximo
FROM Productos
GROUP BY Categoria;

--c) Contar cu�ntos productos hay por categor�a.
SELECT Categoria,
	COUNT(*) AS TotalProductos
FROM Productos
GROUP BY Categoria;

--d) Sumar el total de precios por categor�a.
SELECT Categoria,
	SUM(Precio) AS TotalPrecios
FROM Productos
GROUP BY Categoria;
