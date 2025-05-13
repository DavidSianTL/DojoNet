--ejemplo1
--Obtener los nombres de los clientes que han comprado boletos en vuelos con destino a 'Guatemala':

USE AeroDB;
GO
SELECT Nombre
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
