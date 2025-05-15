--JOINS 

-- INNER JOINS 

-- 1.1
SELECT cli.Nombre, vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha 
	FROM Clientes cli
	INNER JOIN Boletos bol ON cli.ClienteID = bol.ClienteID
	INNER JOIN Asientos asi ON asi.AsientoID = bol.AsientoID
	INNER JOIN Vuelos vuel ON vuel.VueloID = asi.VueloID
;


-- 1.2
SELECT vuel.VueloID, pil.Nombre AS Piloto, vuel.Fecha AS Fecha_vuelo
	FROM Vuelos AS vuel
	JOIN Pilotos AS pil ON pil.PilotoID = vuel.PilotoID


-- 1.3

SELECT ali.Nombre AS Alimento_Servido, vuel.VueloID, vuel.Origen, vuel.Destino
	FROM Alimentos ali
	JOIN VuelosAlimentos vual ON vual.AlimentoID = ali.AlimentoID
	JOIN Vuelos vuel ON vual.VueloID = vuel.VueloID

-- 1.4

SELECT cli.Nombre, asi.AsientoID AS Asiento_No, vuel.Destino AS Destino_de_Vuelo
	FROM Clientes cli 
	JOIN Boletos bol ON bol.ClienteID = cli.ClienteID
	JOIN Asientos asi ON asi.AsientoID = bol.AsientoID
	JOIN Vuelos vuel ON vuel.VueloID = asi.VueloID


-- LEFT JOIN 

-- 2.1

SELECT vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha, ali.Nombre AS Alimento
	FROM Vuelos vuel
	LEFT JOIN VuelosAlimentos vual ON vual.VueloID = vuel.VueloID
	LEFT JOIN Alimentos ali ON vual.AlimentoID = ali.AlimentoID


-- 2.2

SELECT ali.Nombre AS Alimento, vuel.VueloID, vuel.Origen, vuel.Destino
	FROM Alimentos ali
	LEFT JOIN VuelosAlimentos vual ON vual.AlimentoID = ali.AlimentoID
	LEFT JOIN Vuelos vuel ON vuel.VueloID = vual.VueloID


-- 2.3

SELECT rut.Origen, rut.Destino, vuel.VueloID, vuel.Fecha
FROM Rutas rut
LEFT JOIN Vuelos vuel ON vuel.Origen = rut.Origen AND vuel.Destino = rut.Destino



-- FULL JOIN

-- 3.1

SELECT vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha, rut.Origen, rut.Destino
	FROM Vuelos vuel
	FULL OUTER JOIN Rutas rut ON vuel.Origen = rut.Origen AND vuel.Destino = rut.Destino




-- CROSS JOIN 
-- 4.1
SELECT ae.Nombre AS Aeromosas, pil.Nombre AS Pilotos
	FROM Pilotos pil
	CROSS JOIN Aeromosas ae



-- 4.2

SELECT  vuel.VueloID, vuel.Origen, vuel.Destino, al.Nombre AS Alimento, al.Precio AS Precio
FROM Vuelos vuel
CROSS JOIN Alimentos al

