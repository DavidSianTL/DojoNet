
--Crear una consulta que liste todos los vuelos con su origen, destino y fecha.
SELECT *
FROM (
    SELECT VueloID, Origen, Destino, Fecha
    FROM Vuelos
) AS VuelosInfo;



--Escribir una consulta que muestre los clientes que han comprado boletos para vuelos en una fecha específica.
SELECT ClienteID, Nombre, Correo
FROM Clientes
WHERE ClienteID IN (
    SELECT ClienteID
    FROM Boletos
    WHERE FechaCompra = '2025-05-10'
);




--Desarrollar una consulta que indique qué alimentos están disponibles en vuelos con origen en 'Ciudad de Guatemala
SELECT AlimentoID, Nombre, Precio
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
