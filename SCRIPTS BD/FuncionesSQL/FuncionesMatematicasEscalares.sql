USE FuncionesSQLDemo

--Funciones matemáticas escalares

--a) Mostrar el valor absoluto del precio de cada producto.
SELECT Nombre,
	Precio, 
	ABS(Precio) AS PrecioAbs
FROM Productos;

--b) Redondear el precio hacia arriba y hacia abajo.
SELECT Nombre,
	Precio,
	CEILING(Precio) AS RedondearArriba,
	FLOOR(Precio) AS RedondearAbajo
FROM Productos;

--c) Calcular la raíz cuadrada y el cuadrado del precio.
SELECT Nombre,
	Precio,
	SQRT(Precio) AS RaizCuadrada,
	SQUARE(Precio) AS CuadradoPrecio
FROM Productos;

--d) Obtener el signo del precio y un número aleatorio por producto.
SELECT Nombre,
	Precio,
	SIGN(Precio) AS SignoPrecio,
	RAND(CHECKSUM(NEWID())) AS NumAleatorio 
FROM Productos;


