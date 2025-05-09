USE FuncionesSQLDemo

--FUNCIONES DE TEXTO

--a) Convertir todos los nombres a mayúsculas.
SELECT Nombre,
	UPPER(Nombre) AS Mayusculas
FROM Productos;

--b) Obtener la longitud del nombre del producto
SELECT Nombre,
	LEN(Nombre) AS Longitud
FROM Productos;

--c) Extraer las primeras 5 letras del nombre del producto.
SELECT Nombre,
	SUBSTRING(Nombre, 1, 5) AS Abreviado
FROM Productos;

--d) Reemplazar la palabra 'Samsung' por 'LG'.
SELECT Nombre,
	REPLACE(Nombre,'Samsung','LG') AS Remplazado
FROM Productos;

