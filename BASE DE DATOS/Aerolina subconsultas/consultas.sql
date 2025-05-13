

USE AeroDB;
GO
-- Mostrar todos los vuelos y sus detalles
SELECT * FROM Vuelos;

-- Mostrar los boletos con información del cliente
SELECT B.BoletoID, C.Nombre AS Cliente, A.AsientoID, V.VueloID, V.Origen, V.Destino
FROM Boletos B
JOIN Clientes C ON B.ClienteID = C.ClienteID
JOIN Asientos A ON B.AsientoID = A.AsientoID
JOIN Vuelos V ON A.VueloID = V.VueloID;

-- Mostrar los alimentos disponibles en cada vuelo
SELECT V.VueloID, V.Origen, V.Destino, A.Nombre AS Alimento
FROM VuelosAlimentos VA
JOIN Vuelos V ON VA.VueloID = V.VueloID
JOIN Alimentos A ON VA.AlimentoID = A.AlimentoID;
