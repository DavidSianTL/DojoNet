-- a) Valor absoluto del precio
SELECT Nombre, ABS(Precio) AS Precio_Absoluto FROM Productos;

-- b) Redondear hacia arriba y hacia abajo
SELECT Nombre, CEILING(Precio) AS Precio_Arriba, FLOOR(Precio) AS Precio_Abajo FROM Productos;

-- c) Raíz cuadrada y cuadrado del precio
SELECT Nombre, SQRT(Precio) AS Raiz_Cuadrada, POWER(Precio, 2) AS Cuadrado FROM Productos;

-- d) Signo y número aleatorio por producto
SELECT Nombre, SIGN(Precio) AS Signo_Precio, RAND() AS Numero_Aleatorio FROM Productos;

