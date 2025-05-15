--SECCI�N
--5: JOIN + FILTRADO / AGRUPACI�N

--5.1 �Qu�
--piloto ha realizado m�s vuelos?
SELECT p.Nombre, COUNT(v.VueloID) AS TotalVuelos
FROM Pilotos p
INNER JOIN Vuelos v ON p.PilotoID = v.PilotoID
GROUP BY p.Nombre
ORDER BY TotalVuelos DESC;


--5.2 Vuelos con m�s de 2 alimentos
SELECT v.VueloID, COUNT(va.AlimentoID) AS TotalAlimentos
FROM Vuelos v
INNER JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
GROUP BY v.VueloID
HAVING COUNT(va.AlimentoID) > 2;




--5.3
--Mostrar clientes que han reservado boletos en vuelos a Tegucigalpa.
SELECT c.Nombre, v.Destino
FROM Clientes c
INNER JOIN Boletos b ON c.ClienteID = b.ClienteID
INNER JOIN Asientos a ON b.AsientoID = a.AsientoID
INNER JOIN Vuelos v ON a.VueloID = v.VueloID
WHERE v.Destino = 'Tegucigalpa';

