-- 2: Funciones de Texto
-- a) Convertir todos los nombres a mayúsculas
SELECT ID, Nombre, UPPER(Nombre) AS Nombre_Mayusculas
FROM Productos;

-- b) Obtener la longitud del nombre del producto
SELECT ID, Nombre, LEN(Nombre) AS Longitud_Nombre
FROM Productos;

-- c) Extraer las primeras 5 letras del nombre del producto
SELECT ID, Nombre, LEFT(Nombre, 5) AS Primeras_5_Letras
FROM Productos;

-- d) Reemplazar la palabra 'Samsung' por 'LG'
SELECT ID, Nombre, REPLACE(Nombre, 'Samsung', 'LG') AS Nombre_Reemplazado
FROM Productos;
