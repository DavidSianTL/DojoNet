--ejemplo2
--Listar los vuelos con al menos un aliment
USE AeroDB;
GO
SELECT VueloID, Origen, Destino
FROM Vuelos
WHERE EXISTS (
    SELECT 1
    FROM VuelosAlimentos
    WHERE VuelosAlimentos.VueloID = Vuelos.VueloID
);
