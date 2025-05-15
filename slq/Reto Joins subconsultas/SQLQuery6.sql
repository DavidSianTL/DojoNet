--SECCIÓN
--4: CROSS JOIN


--4.1
--Generar una lista de todas las posibles combinaciones de pilotos y aeromosas.

SELECT p.Nombre AS Piloto, a.Nombre AS Aeromosa
FROM Pilotos p
CROSS JOIN Aeromosas a;

--select * from Pilotos
--select * from Aeromosas

--4.2
--Simular emparejamientos posibles entre todos los alimentos y vuelos (sin importar
--disponibilidad real).
SELECT a.Nombre AS Alimento, v.VueloID
FROM Alimentos a
CROSS JOIN Vuelos v;




