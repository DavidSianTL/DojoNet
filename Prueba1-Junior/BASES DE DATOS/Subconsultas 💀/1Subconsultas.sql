-- 1
SELECT VueloID, Origen, Destino, Fecha
FROM Vuelos;


-- 2 
SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
	SELECT bol.ClienteID
	FROM Boletos AS bol
	JOIN Asientos AS asi ON asi.AsientoID = bol.AsientoID
	JOIN Vuelos AS vuel ON vuel.VueloID = asi.VueloID
	WHERE vuel.Fecha = '2025-05-15'
);


-- 3
SELECT Nombre AS Alimentos_disponibles
FROM Alimentos  
WHERE AlimentoID IN (
	SELECT vuali.AlimentoID 
		FROM VuelosAlimentos AS vuali
		JOIN Vuelos vuel ON vuali.VueloID = vuel.VueloID
	WHERE vuel.Origen = 'Ciudad de Guatemala'
);


-- 4

SELECT vuel.Origen, vuel.Destino, vuel.Fecha, vuel.Hora, pil.Nombre AS Piloto
FROM Vuelos vuel
JOIN Pilotos pil ON vuel.PilotoID = pil.PilotoID



-- 5
SELECT cli.Nombre
	FROM Boletos bol
	JOIN Clientes cli ON bol.ClienteID = cli.ClienteID
		WHERE bol.AsientoID IN (
			SELECT AsientoID
			FROM Asientos
				WHERE VueloID IN (
					SELECT vuel.VueloID
					FROM Vuelos vuel
					WHERE Destino = 'Ciudad de Guatemala'
				)
		);




-- 6 

SELECT vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha
FROM Vuelos vuel
WHERE EXISTS (
	SELECT 1 
	FROM VuelosAlimentos vual 
	WHERE vual.VueloID = vuel.VueloID

);


-- 7 

SELECT p.Nombre AS Piloto_pro_player
FROM Pilotos p
WHERE p.PilotoID IN (
	SELECT PilotoID
	FROM Vuelos
	GROUP BY PilotoID
	HAVING COUNT(*) >5
);


-- 8 *se muere mas*

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



