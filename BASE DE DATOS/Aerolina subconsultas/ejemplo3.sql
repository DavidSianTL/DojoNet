--ejemplo3
--Mostrar los pilotos que han volado en rutas con m�s de 10 vuelos:
USE AeroDB;
GO
SELECT Nombre
FROM Pilotos
WHERE PilotoID IN (
    SELECT PilotoID
    FROM Vuelos
    GROUP BY PilotoID
    HAVING COUNT(*) > 1
);
