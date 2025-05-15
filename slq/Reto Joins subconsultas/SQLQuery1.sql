--1. Crear una consulta que liste todos los
--vuelos con su origen, destino y fecha

select VueloID,Origen,Destino,Fecha from Vuelos

-- 2. Consulta que muestre los clientes que han
--comprado boletos para vuelos en una fecha específica

SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
  SELECT b.ClienteID
  FROM Boletos b
  INNER JOIN Asientos a ON b.AsientoID = a.AsientoID
  INNER JOIN Vuelos v ON a.VueloID = v.VueloID
  WHERE v.Fecha = '2025-05-15'
);


-- 3. Consulta que indique qué alimentos están
--disponibles en vuelos con origen en 'Ciudad de Guatemala'

SELECT Nombre
FROM Alimentos
WHERE AlimentoID IN (
  SELECT va.AlimentoID
  FROM VuelosAlimentos va
  INNER JOIN Vuelos v ON va.VueloID = v.VueloID
  WHERE v.Origen = 'Ciudad de Guatemala'
);



--4. Listar todos los vuelos con el nombre del
--piloto asignado
SELECT 
  v.VueloID,
  (SELECT p.Nombre FROM Pilotos p WHERE p.PilotoID = v.PilotoID) AS Piloto,
  v.Fecha
FROM Vuelos v;



--5. Mostrar los nombres de los clientes que han
--comprado boletos para vuelos con destino a Guatemala

SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
  SELECT b.ClienteID
  FROM Boletos b
  INNER JOIN Asientos a ON b.AsientoID = a.AsientoID
  INNER JOIN Vuelos v ON a.VueloID = v.VueloID
  WHERE v.Destino = 'Ciudad de Guatemala'
);



-- 6. Subconsulta: Vuelos que tienen alimentos
--servidos (usando EXISTS)
SELECT VueloID, Fecha
FROM Vuelos v
WHERE EXISTS (
  SELECT 1
  FROM VuelosAlimentos va
  WHERE va.VueloID = v.VueloID
);



-- 7. Subconsulta: Mostrar los nombres de los
--pilotos que han volado más de 5 veces


SELECT Nombre
FROM Pilotos
WHERE PilotoID IN (
  SELECT PilotoID
  FROM Vuelos
  GROUP BY PilotoID
  HAVING COUNT(*) > 5
);




-- 8. Subconsulta: Mostrar alimentos que están
--disponibles en todos los vuelos con destino 'San Salvador'
SELECT a.Nombre
FROM Alimentos a
WHERE NOT EXISTS (
  SELECT 1
  FROM Vuelos v
  WHERE v.Destino = 'San Salvador'
  AND NOT EXISTS (
    SELECT 1
    FROM VuelosAlimentos va
    WHERE va.VueloID = v.VueloID
    AND va.AlimentoID = a.AlimentoID
  )
);
