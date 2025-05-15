use AeroDB


-- 5 JOIN + FILTRADO/AGRUPACION

-- 5.1 

SELECT TOP 1 pil.Nombre AS Piloto_mas_pro, pil.PilotoID, COUNT(vuel.VueloID) AS Total_de_vuelos
	FROM Pilotos pil
	JOIN Vuelos vuel ON pil.PilotoID = vuel.PilotoID
	GROUP BY pil.PilotoID, pil.Nombre
	ORDER BY  Total_de_vuelos DESC;


-- 5.2

SELECT v.VueloID, v.Origen, v.Destino,  COUNT(vual.AlimentoID) Total_alimentos
	FROM Vuelos v
	JOIN VuelosAlimentos vual ON v.VueloID = vual.VueloID
	JOIN Alimentos ali ON ali.AlimentoID = vual.AlimentoID
	GROUP BY v.VueloID, v.Origen, v.Destino

	HAVING COUNT(vual.AlimentoID) > 2  -- // 'where' count(algo) > 2


-- 5.3
SELECT DISTINCT cli.ClienteID, cli.Nombre AS Clientes, vuel.Destino
	FROM Clientes cli
	JOIN Boletos bol ON cli.ClienteID = bol.ClienteID
	JOIN Asientos asi ON asi.AsientoID = bol.AsientoID
	JOIN Vuelos vuel ON vuel.VueloID = asi.VueloID
	WHERE vuel.Destino = 'Tegucigalpa'
		--vuelo 202 con destino a Tegucigalpa no tiene asientos >> no tiene boletos >> no tiene clientes;







-- JOINS + SUBCONSULTAS

-- 6 .1

SELECT vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha, AVG(asi.Precio) promedio_mas_alto 
FROM Vuelos vuel
JOIN Asientos asi ON asi.VueloID = vuel.VueloID					-- hasta aqui parece una consulta normal
GROUP BY vuel.VueloID, vuel.Origen, vuel.Destino, vuel.Fecha	-- GROUP BY porque usamos una funcion agregada
	HAVING AVG(asi.Precio) IN(									-- HAVING porque usamos GROUP BY
		SELECT MAX(Promedios_precio_asiento)				-- Obtenemos el mas alto de los promedios
		FROM(
			SELECT AVG(Precio) AS Promedios_precio_asiento	-- Devolvemos el promedio de los precios de los asiento de cada vuelo 
			FROM Asientos 
			GROUP BY VueloID					-- GROUP BY porque usamos una funcion agregada
		)AS sub_sub_consulta

	);
-- xd		<---- Un 'xd' porque consumimos muchos recursos cerebrales;




-- 6.2
SELECT TOP 1 cli.Nombre, asi.Precio
FROM Clientes cli
JOIN Boletos bol ON cli.ClienteID = bol.ClienteID
JOIN Asientos asi ON asi.AsientoID = bol.AsientoID
WHERE cli.ClienteID = (
	SELECT ClienteID 
	FROM Boletos
	WHERE AsientoID = (
		SELECT AsientoID 
		FROM Asientos
		WHERE Precio = (
			SELECT MAX(Precio) AS Precio_maximo
		FROM Asientos
		)
	)
)
ORDER BY asi.Precio DESC




-- 6.3

SELECT vuel.VueloID, vuel.Origen, vuel.Destino, ali.Nombre AS Alimento
FROM Vuelos vuel
JOIN VuelosAlimentos vual ON vual.VueloID = vuel.VueloID
JOIN Alimentos ali ON ali.AlimentoID = vual.AlimentoID
WHERE ali.Precio > (
	SELECT AVG(Precio) AS Promedio_precio
	FROM Alimentos
);




-- 7.1 
--INNER JOIN
SELECT cli.Nombre, bol.BoletoID
FROM Clientes cli
INNER JOIN Boletos bol ON bol.ClienteID = cli.ClienteID


--LEFT JOIN
SELECT cli.Nombre, bol.BoletoID
FROM Clientes cli
LEFT JOIN Boletos bol ON bol.ClienteID = cli.ClienteID



-- 7.2 
-- CROSS JOIN
SELECT ali.Nombre AS Alimentos, cli.Nombre AS Clientes
	FROM Alimentos ali
	CROSS JOIN Clientes cli