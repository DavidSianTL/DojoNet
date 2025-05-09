-- a) Convertir nombres a mayúsculas
SELECT UPPER(Nombre) AS Nombre_Mayusculas FROM Productos;

-- b) Obtener la longitud del nombre
SELECT Nombre, LEN(Nombre) AS Longitud_Nombre FROM Productos;

-- c) Extraer las primeras 5 letras
SELECT Nombre, LEFT(Nombre, 5) AS Primeras_Letras FROM Productos;

-- d) Reemplazar 'Samsung' por 'LG'
SELECT REPLACE(Nombre, 'Samsung', 'LG') AS Nombre_Modificado FROM Productos;
