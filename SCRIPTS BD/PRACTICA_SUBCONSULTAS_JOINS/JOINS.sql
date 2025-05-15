USE AeroDB;
GO

--SECCI�N
--1 INNER JOIN (Fundamentales)

--1.1 Mostrar los nombres de los clientes y los vuelos que han tomado.
SELECT 
	C.Nombre AS Cliente,
	V.VueloID, 
	V.Origen, 
	V.Destino
FROM Boletos B
INNER JOIN Clientes C ON B.ClienteID = C.ClienteID
INNER JOIN Asientos A ON B.AsientoID = A.AsientoID
INNER JOIN Vuelos V ON A.VueloID = V.VueloID;

--1.2 Listar el ID del vuelo, el nombre del piloto y la fecha del vuelo.
SELECT V.VueloID, 
	V.PilotoID,
	P.Nombre,
	V.Fecha
FROM Vuelos V
INNER JOIN Pilotos P ON V.PilotoID = P.PilotoID;

--1.3 Obtener los nombres de alimentos servidos en cada vuelo, junto con el origen y destino del vuelo.
SELECT V.VueloID, 
	V.Origen, 
	V.Destino,
	A.Nombre AS Alimento
FROM VuelosAlimentos VA
INNER JOIN Vuelos V ON VA.VueloID = V.VueloID
INNER JOIN Alimentos A ON VA.AlimentoID = A.AlimentoID

--1.4 Mostrar nombre del cliente, asiento y el destino del vuelo reservado.
SELECT 
	C.Nombre AS Cliente,
	A.AsientoID,
	V.Destino
FROM Boletos B
INNER JOIN Clientes C ON B.ClienteID = C.ClienteID
INNER JOIN Asientos A ON B.AsientoID = A.AsientoID
INNER JOIN Vuelos V ON A.VueloID = V.VueloID;

--SECCI�N
--2: LEFT / RIGHT JOIN

--2.1 Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno.
--(LEFT JOIN desde vuelos hacia alimentos)
SELECT
	V.VueloID,
	A.Nombre AS Alimento
FROM Vuelos V
LEFT JOIN VuelosAlimentos VA ON V.VueloID = VA.VueloID
LEFT JOIN Alimentos A ON VA.AlimentoID= A.AlimentoID;

--2.2 Listar todos los alimentos y en qu� vuelos se han servido. Incluir alimentos
--que no se han servido en ning�n vuelo.
SELECT 
	A.AlimentoID,
	A.Nombre AS Alimento,
	V.VueloID
FROM Alimentos A
LEFT JOIN VuelosAlimentos VA ON A.AlimentoID = VA.AlimentoID
LEFT JOIN Vuelos V ON VA.VueloID= V.VueloID


--2.3 Mostrar todas las rutas posibles y los vuelos programados para cada una,
--incluso si no se ha usado la ruta a�n.
SELECT 
	V.VueloID,
	V.Origen AS OrigenVuelo,
	V.Destino AS DestinoVuelo,
	R.RutaID,
	R.Origen AS OrigenRuta,
	R.Destino AS DestinoRuta
FROM Rutas R
LEFT JOIN Vuelos V ON R.Origen = V.Origen AND R.Destino= V.Destino


--SECCI�N
--3: FULL OUTER JOIN

--3.1 Listar todos los vuelos y todas las rutas combinadas, mostrando coincidencias
--cuando existan.

--(�Qu� vuelos no tienen ruta? �Qu� rutas no se han usado en vuelos?)
SELECT
  V.VueloID,
  V.Origen AS OrigenVuelo,
  V.Destino AS DestinoVuelo,
  R.RutaID,
  R.Origen AS OrigenRuta,
  R.Destino AS DestinoRuta
FROM Vuelos V
FULL OUTER JOIN Rutas R
  ON V.Origen = R.Origen AND V.Destino = R.Destino;

--SECCI�N
--4: CROSS JOIN

--4.1
--Generar una lista de todas las posibles combinaciones de pilotos y aeromosas.
--(�Cu�ntas combinaciones hay?)

SELECT 
	P.PilotoID,
	P.Nombre AS NombrePiloto, 
	AE.AeromosaID,
	AE.Nombre
FROM Pilotos P
     CROSS JOIN Aeromosas AE

--4.2
--Simular emparejamientos posibles entre todos los alimentos y vuelos (sin importar disponibilidad real).
--(�til para crear pruebas de men�)
SELECT 
	A.Nombre AS Alimento,
	V.VueloID,
	V.Origen AS OrigenVuelo, 
	V.Destino AS DestinoVuelo
FROM Alimentos A
     CROSS JOIN Vuelos V

--SECCI�N
--5: JOIN + FILTRADO / AGRUPACI�N

--5.1 �Qu� piloto ha realizado m�s vuelos?
--Usa GROUP BY con JOIN
SELECT 
	P.PilotoID,
	P.Nombre,
	COUNT(V.VueloID) AS Vuelos
FROM Pilotos P
JOIN Vuelos V ON V.PilotoID = P.PilotoID
GROUP BY P.PilotoID, P.Nombre;

--5.2 Mostrar los vuelos que ofrecen m�s de 2 alimentos distintos.
SELECT
	VA.VueloID,
	COUNT(VA.AlimentoID) AS Alimentos
FROM VuelosAlimentos VA
GROUP BY VA.VueloID
HAVING COUNT(DISTINCT VA.AlimentoID) > 2;

--5.3 Mostrar clientes que han reservado boletos en vuelos a Tegucigalpa.
SELECT 
	C.ClienteID,
	C.Nombre,
	COUNT(B.BoletoID) AS Boletos
FROM Boletos B
JOIN Clientes C ON B.ClienteID = C.ClienteID
JOIN Asientos A ON B.AsientoID = A.AsientoID
JOIN Vuelos V ON A.VueloID = V.VueloID
WHERE V.Destino = 'Tegucigalpa'
GROUP BY C.ClienteID, C.Nombre;

--SECCI�N
--6: JOIN + SUBCONSULTAS

--6.1
--Mostrar los vuelos con el precio promedio de asiento m�s alto (usa subconsulta con AVG y JOIN).
SELECT V.VueloID, 
	V.Origen, 
	V.Destino,
    A.PrecioPromedio
FROM Vuelos V
JOIN (
    SELECT VueloID, 
		AVG(Precio) AS PrecioPromedio
    FROM Asientos
    GROUP BY VueloID
) A ON V.VueloID = A.VueloID
WHERE A.PrecioPromedio = (
    SELECT MAX(PrecioPromedio)
    FROM (
        SELECT 
			AVG(Precio) AS PrecioPromedio
        FROM Asientos
        GROUP BY VueloID
    ) AS�MaxPromedio
);

--6.2 Mostrar el nombre del cliente que pag� el boleto m�s caro.
SELECT C.ClienteID, 
	C.Nombre, 
	C.Correo, 
	A.Precio AS PrecioBoleto
FROM Clientes C
JOIN Boletos B ON C.ClienteID = B.ClienteID
JOIN Asientos A ON B.AsientoID = A.AsientoID
WHERE A.Precio = (
    SELECT MAX(A.Precio)
    FROM Asientos A
    JOIN Boletos B ON A.AsientoID =�B.AsientoID
);

--6.3 Listar los vuelos con alimentos cuyo precio es mayor al promedio de todos los alimentos.
SELECT V.VueloID,
	V.Origen, 
	V.Destino,
	A.Nombre,
	A.Precio
FROM Vuelos V
JOIN VuelosAlimentos VA ON V.VueloID = VA.VueloID
JOIN Alimentos A ON VA.AlimentoID = A.AlimentoID
WHERE A.Precio > (
    SELECT AVG(Precio)
    FROM�Alimentos
);

--SECCI�N
--7: VALIDACI�N Y EXPLICACI�N


--7.1 Explica la diferencia entre INNER JOIN y LEFT JOIN usando un ejemplo de Clientes y Boletos.
--INNER JOIN muestra los datos que coinciden en ambas tablas, en este caso muestra todos los clientes que tiene un boleto comprado
SELECT 
	C.ClienteID,
	B.BoletoID, 
	C.Nombre AS Cliente
FROM Clientes C
INNER JOIN Boletos B ON C.ClienteID = B.ClienteID;

--CROSS JOIN a diferencia del INNER JOIN, muestra todos los datos aunque no coincidan en ambas tablas, en este caso mostrar� 
--Todos los clientes aunque no tengan un boleto comprado a�n.
SELECT 
	C.ClienteID,
	B.BoletoID,
	C.Nombre AS Cliente
FROM Clientes C
LEFT JOIN Boletos B ON C.ClienteID = B.ClienteID

--7.2 �Qu� pasa si haces un CROSS JOIN entre Alimentos y Clientes? �Cu�ntas filas esperas?
--En el CROSS JOIN, va a realizar como una combinaci�n de todos los alimentos que hay con todos los clientes que existen.
SELECT 
	C.ClienteID,
	C.Nombre, 
	A.Nombre AS Alimento
FROM Alimentos A
     CROSS JOIN Clientes C
	 ORDER BY ClienteID