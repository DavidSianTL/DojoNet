-- 1. Crear una consulta que liste todos los vuelos con su origen, destino y fecha

SELECT VueloID, Origen, Destino, Fecha
FROM (
    SELECT VueloID, Origen, Destino, Fecha
    FROM Vuelos
) AS vuelos;
GO
--Consulta que muestre los clientes que han comprado boletos para vuelos en una fecha específica
DECLARE @Fecha DATE = '2025-05-15';

SELECT ClienteID, Nombre, Correo
FROM Clientes
WHERE ClienteID IN (
    SELECT B.ClienteID
    FROM Boletos B
    WHERE B.AsientoID IN (
        SELECT A.AsientoID
        FROM Asientos A
        WHERE A.VueloID IN (
            SELECT V.VueloID
            FROM Vuelos V
            WHERE V.Fecha = @Fecha
        )
		)
    );

	--3. Alimentos disponibles en vuelos con origen en 'Ciudad de Guatemala'
	SELECT a.Nombre
FROM Alimentos a
WHERE a.AlimentoID IN (
    SELECT va.AlimentoID
    FROM VuelosAlimentos va
    WHERE va.VueloID IN (
        SELECT VueloID
        FROM Vuelos
        WHERE Origen = 'Ciudad de Guatemala'
    )
);
Go

--4. Listar todos los vuelos con el nombre del piloto asignado
SELECT 
    VueloID,
    Origen,
    Destino,
    Fecha,
    (SELECT Nombre 
     FROM Pilotos 
     WHERE PilotoID = V.PilotoID) AS NombrePiloto
FROM Vuelos V;

-- 5. Mostrar los nombres de los clientes que han comprado boletos para vuelos con destino a Guatemala
SELECT c.Nombre
FROM Clientes c
WHERE c.ClienteID IN (
    SELECT b.ClienteID
    FROM Boletos b
    WHERE b.AsientoID IN (
        SELECT a.AsientoID
        FROM Asientos a
        WHERE a.VueloID IN (
            SELECT VueloID
            FROM Vuelos
            WHERE Destino = 'Ciudad de Guatemala'
        )
    )
);


--6. Vuelos que tienen alimentos servidos (usando EXISTS)

SELECT VueloID, Origen, Destino, Fecha
FROM Vuelos v
WHERE EXISTS (
    SELECT 1  --devuelve cualquier valor que cumpla con el where
    FROM VuelosAlimentos va
    WHERE va.VueloID = v.VueloID
);

-- 7. Subconsulta: Mostrar los nombres de los pilotos que han volado más de 5 veces

SELECT Nombre
FROM Pilotos
WHERE PilotoID IN (
    SELECT PilotoID
    FROM Vuelos
    GROUP BY PilotoID 
    HAVING COUNT(*) > 5
);

--8. Alimentos que están disponibles en todos los vuelos con destino 'San Salvador'

SELECT Nombre AS Alimento
FROM Alimentos
WHERE AlimentoID IN (
    SELECT AlimentoID
    FROM VuelosAlimentos
    WHERE VueloID IN (
        SELECT VueloID
        FROM Vuelos
        WHERE Destino = 'San Salvador'
    )
);


-- 1.1 Mostrar los nombres de los clientes y los vuelos que han tomado.
SELECT c.Nombre AS Cliente, v.Origen, v.Destino, v.VueloID
FROM Clientes c
JOIN Boletos b ON c.ClienteID = b.ClienteID
JOIN Asientos a ON b.AsientoID = a.AsientoID
JOIN Vuelos v ON a.VueloID = v.VueloID;

-- 1.2 Listar el ID del vuelo, el nombre del piloto y la fecha del vuelo.
SELECT v.VueloID, p.Nombre AS Piloto, v.Fecha
FROM Vuelos v
JOIN Pilotos p ON v.PilotoID = p.PilotoID;


-- 1.3 Obtener los nombres de alimentos servidos en cada vuelo, junto con el origen y destino del vuelo.
SELECT v.VueloID, a.Nombre AS Alimento, v.Origen, v.Destino
FROM Vuelos v
JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
JOIN Alimentos a ON va.AlimentoID = a.AlimentoID;

-- 1.4 Mostrar nombre del cliente, asiento y el destino del vuelo reservado.
SELECT c.Nombre AS Cliente, a.AsientoID, v.Destino
FROM Boletos b
JOIN Clientes c ON b.ClienteID = c.ClienteID
JOIN Asientos a ON b.AsientoID = a.AsientoID
JOIN Vuelos v ON a.VueloID = v.VueloID;

-- 2.1 Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno.
SELECT v.VueloID, v.Origen, v.Destino, a.Nombre AS Alimento
FROM Vuelos v
LEFT JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
LEFT JOIN Alimentos a ON va.AlimentoID = a.AlimentoID;


-- 2.2 Listar todos los alimentos y en qué vuelos se han servido. Incluir alimentos que no se han servido en ningún vuelo.
SELECT a.AlimentoID, a.Nombre AS Alimento, v.VueloID, v.Origen, v.Destino
FROM Alimentos a
LEFT JOIN VuelosAlimentos va ON a.AlimentoID = va.AlimentoID
LEFT JOIN Vuelos v ON va.VueloID = v.VueloID;


-- 2.3 Mostrar todas las rutas posibles y los vuelos programados para cada una, incluso si no se ha usado la ruta aún.
SELECT r.RutaID, r.Origen AS Origen, r.Destino AS Destino, v.VueloID, v.Fecha, v.Hora
FROM Rutas r
LEFT JOIN Vuelos v ON r.Origen = v.Origen AND r.Destino = v.Destino;

-- 3.1 Listar todos los vuelos y todas las rutas combinadas, mostrando coincidencias cuando existan.
SELECT 
    v.VueloID, v.Origen AS VueloOrigen, v.Destino AS VueloDestino, v.Fecha, v.Hora,
    r.RutaID, r.Origen AS RutaOrigen, r.Destino AS RutaDestino
FROM Vuelos v
FULL OUTER JOIN Rutas r
    ON v.Origen = r.Origen AND v.Destino = r.Destino;

-- 4.1 Generar una lista de todas las posibles combinaciones de pilotos y aeromosas.
SELECT 
    p.PilotoID,
    p.Nombre AS NombrePiloto,
    a.AeromosaID,
    a.Nombre AS NombreAeromosa
FROM Pilotos p
CROSS JOIN Aeromosas a;

-- 4.2 Simular emparejamientos posibles entre todos los alimentos y vuelos.
SELECT 
    a.AlimentoID,
    a.Nombre AS NombreAlimento,
    v.VueloID,
    v.Origen,
    v.Destino
FROM Alimentos a
CROSS JOIN Vuelos v;

-- 5.1 ¿Qué piloto ha realizado más vuelos?
SELECT TOP 1
    p.Nombre AS NombrePiloto,
    COUNT(v.VueloID) AS TotalVuelos
FROM Pilotos p
JOIN Vuelos v ON p.PilotoID = v.PilotoID
GROUP BY p.PilotoID, p.Nombre
ORDER BY TotalVuelos DESC;

-- 5.2 Mostrar los vuelos que ofrecen más de 2 alimentos distintos.
SELECT v.VueloID, COUNT(DISTINCT va.AlimentoID) AS TotalAlimentos
FROM Vuelos v
JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
GROUP BY v.VueloID
HAVING COUNT(DISTINCT va.AlimentoID) > 1;

-- 5.3 Mostrar clientes que han reservado boletos en vuelos a Tegucigalpa.
SELECT c.Nombre AS Cliente, b.FechaCompra
FROM Clientes c
JOIN Boletos b ON c.ClienteID = b.ClienteID
JOIN Asientos a ON b.AsientoID = a.AsientoID
JOIN Vuelos v ON a.VueloID = v.VueloID
WHERE v.Destino = 'Tegucigalpa';

-- 6.1 Mostrar los vuelos con el precio promedio de asiento más alto (usa subconsulta con AVG y JOIN).
SELECT v.VueloID, v.Origen, v.Destino, v.Fecha, AVG(a.Precio) AS PrecioPromedio
FROM Vuelos v
JOIN Asientos a ON v.VueloID = a.VueloID
GROUP BY v.VueloID, v.Origen, v.Destino, v.Fecha, v.Hora
HAVING AVG(a.Precio) = (
    SELECT MAX(PrecioPromedio)
    FROM (
        SELECT AVG(a.Precio) AS PrecioPromedio
        FROM Vuelos v
        JOIN Asientos a ON v.VueloID = a.VueloID
        GROUP BY v.VueloID
    ) AS Subconsulta
);

-- 6.2 Mostrar el nombre del cliente que pagó el boleto más caro.
SELECT c.Nombre, a.Precio
FROM Clientes c
JOIN Boletos b ON c.ClienteID = b.ClienteID
JOIN Asientos a ON b.AsientoID = a.AsientoID
WHERE a.Precio = (
    SELECT MAX(a.Precio)
    FROM Asientos a
    JOIN Boletos b ON a.AsientoID = b.AsientoID
)

-- 6.3 Listar los vuelos con alimentos cuyo precio es mayor al promedio de todos los alimentos.
SELECT v.VueloID, v.Origen, v.Destino, a.Nombre AS Alimento, a.Precio
FROM Vuelos v
JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
JOIN Alimentos a ON va.AlimentoID = a.AlimentoID
WHERE a.Precio > (
    SELECT AVG(Precio)
    FROM Alimentos
)


-- 8. Alimentos disponibles en todos los vuelos con destino 'San Salvador'
SELECT A.Nombre 
FROM Alimentos A
WHERE EXISTS (
    SELECT V.VueloID FROM Vuelos V
    WHERE V.Destino = 'San Salvador'
    AND EXISTS (
        SELECT VA.AlimentoID FROM VuelosAlimentos VA
        WHERE VA.VueloID = V.VueloID AND VA.AlimentoID = A.AlimentoID
    )
);