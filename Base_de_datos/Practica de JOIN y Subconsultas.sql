-- 1. Listar todos los vuelos con su origen, destino y fecha
SELECT VueloID, Origen, Destino, Fecha FROM Vuelos;

-- 2. Clientes que han comprado boletos para vuelos en una fecha específica (Ejemplo: vuelos del 2025-05-15)
SELECT Nombre, BoletoID,
    (SELECT A.VueloID 
     FROM Asientos A 
     WHERE A.AsientoID = B.AsientoID) AS VueloID,
    (SELECT V.Fecha 
     FROM Vuelos V 
     WHERE V.VueloID = (
         SELECT A.VueloID 
         FROM Asientos A 
         WHERE A.AsientoID = B.AsientoID
     )) AS Fecha
FROM Clientes
WHERE ClienteID IN (
    SELECT ClienteID 
    FROM Boletos 
    WHERE AsientoID IN (
        SELECT AsientoID 
        FROM Asientos 
        WHERE VueloID IN (
            SELECT VueloID 
            FROM Vuelos 
            WHERE Fecha = '2025-05-15'
        )
    )
);
-- 3. Alimentos disponibles en vuelos con origen en 'Ciudad de Guatemala'
SELECT DISTINCT Nombre 
FROM Alimentos 
WHERE AlimentoID IN (
    SELECT AlimentoID 
    FROM VuelosAlimentos 
    WHERE VueloID IN (
        SELECT VueloID 
        FROM Vuelos 
        WHERE Origen = 'Ciudad de Guatemala'
    )
);
-- 4. Listar todos los vuelos con el nombre del piloto asignado 
SELECT VueloID, Origen, Destino,
    (SELECT Nombre 
     FROM Pilotos 
     WHERE PilotoID = V.PilotoID) AS Piloto
FROM Vuelos V;

-- 5. Clientes con boletos en vuelos con destino a Guatemala
SELECT DISTINCT Nombre 
FROM Clientes 
WHERE ClienteID IN (
    SELECT ClienteID 
    FROM Boletos 
    WHERE AsientoID IN (
        SELECT AsientoID 
        FROM Asientos 
        WHERE VueloID IN (
            SELECT VueloID 
            FROM Vuelos 
            WHERE Destino = 'Ciudad de Guatemala'
        )
    )
);

-- 6. Vuelos que tienen alimentos servidos (usando EXISTS)
SELECT VueloID, Origen, Destino 
FROM Vuelos V
WHERE EXISTS (
    SELECT 1 FROM VuelosAlimentos VA
    WHERE VA.VueloID = V.VueloID
);

-- 7. Pilotos que han volado más de 5 veces
SELECT P.Nombre 
FROM Pilotos P
WHERE (SELECT COUNT(*) FROM Vuelos V WHERE V.PilotoID = P.PilotoID) > 5;

-- 8. Alimentos disponibles en todos los vuelos con destino 'San Salvador'
SELECT A.Nombre 
FROM Alimentos A
WHERE NOT EXISTS (
    SELECT V.VueloID FROM Vuelos V
    WHERE V.Destino = 'San Salvador'
    AND NOT EXISTS (
        SELECT VA.AlimentoID FROM VuelosAlimentos VA
        WHERE VA.VueloID = V.VueloID AND VA.AlimentoID = A.AlimentoID
    )
);

-- SECCIÓN 1: INNER JOIN
-- 1.1 Mostrar los nombres de los clientes y los vuelos que han tomado
SELECT C.Nombre, V.VueloID, V.Origen, V.Destino 
FROM Clientes C
INNER JOIN Boletos B ON C.ClienteID = B.ClienteID
INNER JOIN Asientos A ON B.AsientoID = A.AsientoID
INNER JOIN Vuelos V ON A.VueloID = V.VueloID;

-- 1.2 Listar el ID del vuelo, el nombre del piloto y la fecha del vuelo
SELECT V.VueloID, P.Nombre AS Piloto, V.Fecha 
FROM Vuelos V
INNER JOIN Pilotos P ON V.PilotoID = P.PilotoID;

-- 1.3 Obtener nombres de alimentos servidos en cada vuelo, junto con origen y destino
SELECT V.VueloID, V.Origen, V.Destino, A.Nombre AS Alimento
FROM Alimentos A
INNER JOIN VuelosAlimentos VA ON A.AlimentoID = VA.AlimentoID
INNER JOIN Vuelos V ON VA.VueloID = V.VueloID;

-- 1.4 Mostrar nombre del cliente, asiento y destino del vuelo reservado
SELECT C.Nombre, A.AsientoID, V.Destino 
FROM Clientes C
INNER JOIN Boletos B ON C.ClienteID = B.ClienteID
INNER JOIN Asientos A ON B.AsientoID = A.AsientoID
INNER JOIN Vuelos V ON A.VueloID = V.VueloID;

-- SECCIÓN 2: LEFT / RIGHT JOIN
-- 2.1 Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno (LEFT JOIN)
SELECT V.VueloID, V.Origen, V.Destino, A.Nombre AS Alimento
FROM Vuelos V
LEFT JOIN VuelosAlimentos VA ON V.VueloID = VA.VueloID
LEFT JOIN Alimentos A ON VA.AlimentoID = A.AlimentoID;

-- 2.2 Listar todos los alimentos y en qué vuelos se han servido (incluyendo los que no han sido servidos)
SELECT A.Nombre, V.VueloID, V.Origen, V.Destino
FROM Alimentos A
LEFT JOIN VuelosAlimentos VA ON A.AlimentoID = VA.AlimentoID
LEFT JOIN Vuelos V ON VA.VueloID = V.VueloID;

-- 2.3 Mostrar todas las rutas posibles y los vuelos programados para cada una
SELECT R.RutaID, R.Origen, R.Destino, V.VueloID
FROM Rutas R
LEFT JOIN Vuelos V ON R.Origen = V.Origen AND R.Destino = V.Destino;

-- SECCIÓN 3: FULL OUTER JOIN
-- 3.1 Listar todos los vuelos y rutas combinadas, mostrando coincidencias cuando existan
SELECT V.VueloID, V.Origen, V.Destino, R.RutaID 
FROM Vuelos V
FULL OUTER JOIN Rutas R ON V.Origen = R.Origen AND V.Destino = R.Destino;

-- SECCIÓN 4: CROSS JOIN
-- 4.1 Generar combinaciones de pilotos y aeromosas
SELECT P.Nombre AS Piloto, A.Nombre AS Aeromosa
FROM Pilotos P
CROSS JOIN Aeromosas A;

-- 4.2 Simular emparejamientos posibles entre todos los alimentos y vuelos
SELECT V.VueloID, A.Nombre AS Alimento
FROM Vuelos V
CROSS JOIN Alimentos A;

-- SECCIÓN 5: JOIN + AGRUPACIÓN
-- 5.1 Piloto que ha realizado más vuelos
SELECT P.Nombre, COUNT(V.VueloID) AS NumeroVuelos
FROM Pilotos P
INNER JOIN Vuelos V ON P.PilotoID = V.PilotoID
GROUP BY P.Nombre
ORDER BY NumeroVuelos DESC
LIMIT 1;

-- SECCIÓN 6: JOIN + SUBCONSULTAS
-- 6.1 Vuelos con el precio promedio de asiento más alto
SELECT V.VueloID, AVG(A.Precio) AS PrecioPromedio
FROM Vuelos V
INNER JOIN Asientos A ON V.VueloID = A.VueloID
GROUP BY V.VueloID
ORDER BY PrecioPromedio DESC
LIMIT 1;

-- 6.2 Nombre del cliente que pagó el boleto más caro
SELECT C.Nombre
FROM Clientes C
INNER JOIN Boletos B ON C.ClienteID = B.ClienteID
INNER JOIN Asientos A ON B.AsientoID = A.AsientoID
WHERE A.Precio = (SELECT MAX(Precio) FROM Asientos);

-- 6.3 Vuelos con alimentos cuyo precio es mayor al promedio de todos los alimentos
SELECT V.VueloID, A.Nombre AS Alimento, A.Precio
FROM Alimentos A
INNER JOIN VuelosAlimentos VA ON A.AlimentoID = VA.AlimentoID
INNER JOIN Vuelos V ON VA.VueloID = V.VueloID;
WHERE A.Precio > (SELECT AVG(Precio) FROM Alimentos);
