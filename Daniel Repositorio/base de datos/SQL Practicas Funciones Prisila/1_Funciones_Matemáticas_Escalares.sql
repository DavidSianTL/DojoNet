----1: Funciones Matemáticas Escalares

-- a) Mostrar el valor absoluto del precio de cada producto
SELECT ID, Nombre, Precio, ABS(Precio) AS Precio_Absoluto
FROM Productos;

-- b) Redondear el precio hacia arriba  y hacia abajo 
SELECT ID, Nombre, Precio, CEILING(Precio) AS Precio_Arriba, FLOOR(Precio) AS Precio_Abajo
FROM Productos;

-- c) Calcular la raíz cuadrada  y el cuadrado 
SELECT ID, Nombre, Precio, 
       SQRT(Precio) AS Raiz_Cuadrada, 
       POWER(Precio, 2) AS Cuadrado
FROM Productos;

-- d) Obtener el signo del precio y un número aleatorio por producto
SELECT ID, Nombre, Precio, SIGN(Precio) AS Signo,
       RAND(CHECKSUM(NEWID())) * 100 AS Numero_Aleatorio FROM Productos;
