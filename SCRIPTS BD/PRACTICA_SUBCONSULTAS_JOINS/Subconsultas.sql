USE AeroDB;
GO

-- 1. Crear una consulta que liste todos los vuelos con su origen, destino y fecha
SELECT 
	VueloID, 
	Origen, 
	Destino, 
	Fecha 
FROM Vuelos

-- 2. Consulta que muestre los clientes que han comprado boletos para vuelos en una fecha específica
SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
    SELECT ClienteID
    FROM Boletos
    WHERE AsientoID IN (
        SELECT AsientoID
        FROM Asientos
        WHERE VueloID IN (
            SELECT VueloID
            FROM Vuelos
            WHERE Fecha = '2025-05-15'
        )
    )
);

-- 3. Consulta que indique qué alimentos están disponibles en vuelos con origen en 'Ciudad de Guatemala'
SELECT Nombre
FROM Alimentos
	WHERE AlimentoID IN(
	SELECT AlimentoID
	  FROM VuelosAlimentos
	  WHERE VueloID IN (
		SELECT VueloID
		FROM Vuelos
		WHERE Origen = 'Ciudad de Guatemala'
	)
); 

-- 4. Listar todos los vuelos con el nombre del piloto asignado

SELECT VueloID
FROM Vuelos
	WHERE PilotoID IN (
	SELECT PilotoID
	FROM Pilotos
		WHERE Nombre = 'Miguel Pérez'
);

-- 5. Mostrar los nombres de los clientes que han comprado boletos para vuelos con destino a Guatemala
SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
    SELECT ClienteID
    FROM Boletos
    WHERE AsientoID IN (
        SELECT AsientoID
        FROM Asientos
        WHERE VueloID IN (
            SELECT VueloID
            FROM Vuelos
            WHERE Destino = 'Ciudad de Guatemala'
        )
    )
);

-- 6. Subconsulta: Vuelos que tienen alimentos servidos (usando EXISTS)
SELECT VueloID,
	Origen, 
	Destino
FROM Vuelos
WHERE EXISTS (
    SELECT 1
    FROM VuelosAlimentos
    WHERE VuelosAlimentos.VueloID = Vuelos.VueloID
);

-- 7. Subconsulta: Mostrar los nombres de los pilotos que han volado más de 5 veces
SELECT Nombre
FROM Pilotos
WHERE PilotoID IN (
	SELECT PilotoID
	FROM Vuelos
	GROUP BY PilotoID
	HAVING COUNT(*) >=5
);

-- 8. Subconsulta: Mostrar alimentos que están disponibles en todos los vuelos con destino 'San Salvador'
SELECT A.Nombre
FROM Alimentos A
WHERE A.AlimentoID IN (
    SELECT VA.AlimentoID
    FROM VuelosAlimentos VA
    WHERE VA.VueloID IN (
        SELECT VueloID
        FROM Vuelos
        WHERE Destino = 'San Salvador'
    )
);


