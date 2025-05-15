--SECCIÓN
--6: JOIN + SUBCONSULTAS

--6.1
--Mostrar los vuelos con el precio promedio de asiento más alto (usa subconsulta
--con AVG y JOIN).

SELECT VueloID, AVGPrecio
FROM (
  SELECT v.VueloID, AVG(a.Precio) AS AVGPrecio
  FROM Vuelos v
  INNER JOIN Asientos a ON v.VueloID = a.VueloID
  GROUP BY v.VueloID
) AS Promedios
WHERE AVGPrecio = (
  SELECT MAX(AVGPrecio)
  FROM (
    SELECT AVG(a.Precio) AS AVGPrecio
    FROM Asientos a
    GROUP BY a.VueloID
  ) AS SubMax
);



--6.2
--Mostrar el nombre del cliente que pagó el boleto más caro.
--select * from Clientes
--select * from Boletos
--select * from Asientos


SELECT c.Nombre, a.Precio
FROM Clientes c
INNER JOIN Boletos b ON c.ClienteID = b.ClienteID
INNER JOIN Asientos a ON b.AsientoID = a.AsientoID
WHERE a.Precio = (
  SELECT MAX(Precio)
  FROM Asientos
);




--6.3 Vuelos con alimentos cuyo precio supera el promedio
SELECT v.VueloID, a.Nombre, a.Precio
FROM Vuelos v
INNER JOIN VuelosAlimentos va ON v.VueloID = va.VueloID
INNER JOIN Alimentos a ON va.AlimentoID = a.AlimentoID
WHERE a.Precio > (
  SELECT AVG(Precio)
  FROM Alimentos
);
