USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('AeroDB') IS NOT NULL
BEGIN
    ALTER DATABASE AeroDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AeroDB;
END
GO
CREATE DATABASE AeroDB;
go

USE AeroDB;
GO

-- Crear tablas
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY,
    Nombre VARCHAR(100),
    Correo VARCHAR(100)
);

CREATE TABLE Vuelos (
    VueloID INT PRIMARY KEY,
    Origen VARCHAR(50),
    Destino VARCHAR(50),
    Fecha DATE,
    Hora TIME
);

CREATE TABLE Asientos (
    AsientoID INT PRIMARY KEY,
    VueloID INT,
    Clase VARCHAR(20),
    Precio DECIMAL(10, 2),
    FOREIGN KEY (VueloID) REFERENCES Vuelos(VueloID)
);

CREATE TABLE Boletos (
    BoletoID INT PRIMARY KEY,
    ClienteID INT,
    AsientoID INT,
    FechaCompra DATE,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    FOREIGN KEY (AsientoID) REFERENCES Asientos(AsientoID)
);

CREATE TABLE Rutas (
    RutaID INT PRIMARY KEY,
    Origen VARCHAR(50),
    Destino VARCHAR(50)
);

CREATE TABLE Alimentos (
    AlimentoID INT PRIMARY KEY,
    Nombre VARCHAR(50),
    Precio DECIMAL(10, 2)
);

CREATE TABLE VuelosAlimentos (
    VueloID INT,
    AlimentoID INT,
    PRIMARY KEY (VueloID, AlimentoID),
    FOREIGN KEY (VueloID) REFERENCES Vuelos(VueloID),
    FOREIGN KEY (AlimentoID) REFERENCES Alimentos(AlimentoID)
);

CREATE TABLE Pilotos (
    PilotoID INT PRIMARY KEY,
    Nombre VARCHAR(100),
    Licencia VARCHAR(50)
);

CREATE TABLE Aeromosas (
    AeromosaID INT PRIMARY KEY,
    Nombre VARCHAR(100),
    VueloID INT,
    FOREIGN KEY (VueloID) REFERENCES Vuelos(VueloID)
);
GO

-- Insertar clientes
INSERT INTO Clientes (ClienteID, Nombre, Correo) VALUES
(1, 'Ana González', 'ana.gonzalez@email.com'),
(2, 'Luis Martínez', 'luis.martinez@email.com'),
(3, 'Carlos Ruiz', 'carlos.ruiz@email.com');

-- Insertar rutas
INSERT INTO Rutas (RutaID, Origen, Destino) VALUES
(1, 'Ciudad de Guatemala', 'San Salvador'),
(2, 'Ciudad de Guatemala', 'Tegucigalpa'),
(3, 'San Salvador', 'Managua');

-- Insertar vuelos
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora) VALUES
(101, 'Ciudad de Guatemala', 'San Salvador', '2025-05-15', '08:00'),
(102, 'Ciudad de Guatemala', 'Tegucigalpa', '2025-05-15', '10:30'),
(103, 'San Salvador', 'Managua', '2025-05-16', '14:00');

-- Insertar asientos
INSERT INTO Asientos (AsientoID, VueloID, Clase, Precio) VALUES
(1, 101, 'Económica', 150.00),
(2, 101, 'Ejecutiva', 300.00),
(3, 102, 'Económica', 170.00),
(4, 103, 'Económica', 200.00);

-- Insertar boletos
INSERT INTO Boletos (BoletoID, ClienteID, AsientoID, FechaCompra) VALUES
(1001, 1, 1, '2025-05-10'),
(1002, 2, 2, '2025-05-10'),
(1003, 3, 3, '2025-05-11');

-- Insertar alimentos
INSERT INTO Alimentos (AlimentoID, Nombre, Precio) VALUES
(1, 'Sandwich', 5.00),
(2, 'Jugo de Naranja', 2.50),
(3, 'Agua Embotellada', 1.00);

-- Relacionar alimentos con vuelos
INSERT INTO VuelosAlimentos (VueloID, AlimentoID) VALUES
(101, 1),
(101, 3),
(102, 2),
(103, 1),
(103, 2),
(103, 3);

-- Insertar pilotos
INSERT INTO Pilotos (PilotoID, Nombre, Licencia) VALUES
(1, 'Miguel Pérez', 'LIC-P001'),
(2, 'Julia Sánchez', 'LIC-P002');

-- Insertar aeromosas
INSERT INTO Aeromosas (AeromosaID, Nombre, VueloID) VALUES
(1, 'Laura Ramírez', 101),
(2, 'Paola Díaz', 102),
(3, 'Verónica López', 103);

-- Nuevos vuelos con destino a Guatemala
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora) VALUES
(104, 'San Salvador', 'Ciudad de Guatemala', '2025-05-17', '09:00'),
(105, 'Tegucigalpa', 'Ciudad de Guatemala', '2025-05-18', '11:45');

-- Asientos para estos vuelos
INSERT INTO Asientos (AsientoID, VueloID, Clase, Precio) VALUES
(5, 104, 'Económica', 130.00),
(6, 104, 'Ejecutiva', 250.00),
(7, 105, 'Económica', 140.00);

-- Nuevos boletos vendidos para estos vuelos
INSERT INTO Boletos (BoletoID, ClienteID, AsientoID, FechaCompra) VALUES
(1004, 1, 5, '2025-05-12'),
(1005, 2, 6, '2025-05-13'),
(1006, 3, 7, '2025-05-13');

-- Alimentos disponibles en estos vuelos
INSERT INTO VuelosAlimentos (VueloID, AlimentoID) VALUES
(104, 1),
(104, 2),
(105, 3);

-- Nuevos pilotos asignados si se desea alternar
INSERT INTO Pilotos (PilotoID, Nombre, Licencia) VALUES
(3, 'Ernesto Ramírez', 'LIC-P003');

-- Aeromosas para los nuevos vuelos
INSERT INTO Aeromosas (AeromosaID, Nombre, VueloID) VALUES
(4, 'Gabriela Castillo', 104),
(5, 'Mónica Herrera', 105);
GO

-- PASO 1: Agregar columna PilotoID a la tabla Vuelos
ALTER TABLE Vuelos
ADD PilotoID INT;
GO

-- PASO 2: Establecer la clave foránea
ALTER TABLE Vuelos
ADD CONSTRAINT FK_Vuelos_Pilotos
FOREIGN KEY (PilotoID) REFERENCES Pilotos(PilotoID);
GO

-- PASO 3: Agregar más vuelos para simular carga
-- Suponemos 3 pilotos existentes: IDs 1, 2 y 3
-- El piloto 1 tendrá 11 vuelos para cumplir la condición del ejemplo

-- Vuelos adicionales del Piloto 1
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora, PilotoID) VALUES
(201, 'Ciudad de Guatemala', 'San Salvador', '2025-06-01', '08:00', 1),
(202, 'Ciudad de Guatemala', 'Tegucigalpa', '2025-06-02', '08:30', 1),
(203, 'San Salvador', 'Ciudad de Guatemala', '2025-06-03', '09:00', 1),
(204, 'Ciudad de Guatemala', 'Managua', '2025-06-04', '10:00', 1),
(205, 'Tegucigalpa', 'Ciudad de Guatemala', '2025-06-05', '11:00', 1),
(206, 'Ciudad de Guatemala', 'Belice', '2025-06-06', '12:00', 1),
(207, 'Ciudad de Guatemala', 'San Pedro Sula', '2025-06-07', '13:00', 1),
(208, 'Ciudad de Guatemala', 'Panamá', '2025-06-08', '14:00', 1),
(209, 'Ciudad de Guatemala', 'México', '2025-06-09', '15:00', 1),
(210, 'Ciudad de Guatemala', 'San José', '2025-06-10', '16:00', 1);
GO

-- Asignar vuelos existentes a los pilotos
UPDATE Vuelos SET PilotoID = 1 WHERE VueloID IN (101, 104);
UPDATE Vuelos SET PilotoID = 2 WHERE VueloID IN (102, 105);
UPDATE Vuelos SET PilotoID = 3 WHERE VueloID = 103;
GO

-- Mostrar todos los vuelos y sus detalles
SELECT * FROM Vuelos;

-- Mostrar los boletos con información del cliente
SELECT B.BoletoID, C.Nombre AS Cliente, A.AsientoID, V.VueloID, V.Origen, V.Destino
FROM Boletos B
JOIN Clientes C ON B.ClienteID = C.ClienteID
JOIN Asientos A ON B.AsientoID = A.AsientoID
JOIN Vuelos V ON A.VueloID = V.VueloID;

-- Mostrar los alimentos disponibles en cada vuelo
SELECT V.VueloID, V.Origen, V.Destino, A.Nombre AS Alimento
FROM VuelosAlimentos VA
JOIN Vuelos V ON VA.VueloID = V.VueloID
JOIN Alimentos A ON VA.AlimentoID = A.AlimentoID;
GO

--ejemplo1
--Obtener los nombres de los clientes que han comprado boletos en vuelos con destino a 'Guatemala':
SELECT Nombre
FROM Clientes
WHERE ClienteID IN (
    -- Selecciona los IDs de cliente que tienen boletos asociados a ciertos asientos
    SELECT ClienteID
    FROM Boletos
    WHERE AsientoID IN (
        -- Filtra los asientos que pertenecen a vuelos con destino a 'Ciudad de Guatemala'
        SELECT AsientoID
        FROM Asientos
        WHERE VueloID IN (
            -- Busca los vuelos cuyo destino es 'Ciudad de Guatemala'
            SELECT VueloID
            FROM Vuelos
            WHERE Destino = 'Ciudad de Guatemala'
        )
    )
);
GO

--ejemplo2
--Listar los vuelos con al menos un aliment
SELECT VueloID, Origen, Destino
FROM Vuelos V
-- Buscamos si existe al menos un registro en la tabla de VuelosAlimentos
WHERE EXISTS (
    SELECT 1
    FROM VuelosAlimentos VA
    WHERE VA.VueloID = V.VueloID
);

--ejemplo3
--Mostrar los pilotos que han volado en rutas con más de 10 vuelos:
SELECT Nombre
FROM Pilotos
-- Buscamos los pilotos con más de 10 vuelos
WHERE PilotoID IN (
    SELECT PilotoID
    FROM Vuelos
    GROUP BY PilotoID
    HAVING COUNT(*) > 1
);


-- primera parte de la hoja del trabajo
-- se hizo los select 
SELECT 
    VueloID,
    Origen,
    Destino,
    Fecha
FROM 
    Vuelos;

	-- inner join "boletos" combina cada cliente con sus boletos usando el Cliente ID 
	--  **   **   "vuelos" enlaza cada boleto con su vuelo correspondientes, osea con el VuelosID
	-- where lo puse como ejemplo que solo aquellos registros esten programados para el 15 de mayo 
	-- select Se muestra el ID y el nombre del cliente junto con los datos del vuelo 
   DECLARE @FechaEspecifica DATE = '2025-05-15';
SELECT
    c.ClienteID,
    c.Nombre      AS ClienteNombre,
    b.BoletoID,
    v.VueloID,
    v.Origen,
    v.Destino,
    v.Fecha
FROM
    Clientes AS c
    INNER JOIN Boletos AS b
        ON c.ClienteID = b.ClienteID
	INNER JOIN Asientos AS a
	    ON a.AsientoID = b.AsientoID
    INNER JOIN Vuelos AS v
        ON v.VueloID = a.VueloID
WHERE
    v.Fecha = @FechaEspecifica;
GO

SELECT * FROM Vuelos
SELECT * FROM Asientos
select * from Boletos

-- parte 3 
-- Los alimentos van asociados para cada vuelo ejemplo "Guatemala" se hace un tipo filtro de la comida que se va a dar en el vuelo
-- Distict se utiliza para evitar alimentos duplicaos en el mismo vuelo
SELECT DISTINCT 
    a.Nombre AS Alimento
FROM Alimentos AS a
INNER JOIN VuelosAlimentos AS va
    ON a.AlimentoID = va.AlimentoID
INNER JOIN Vuelos AS v
    ON va.VueloID = v.VueloID
WHERE v.Origen = 'Ciudad de Guatemala';
GO
select *from Alimentos
select * from VuelosAlimentos -- Los utilice para hacerle consultas a las tablas y tener relacion 
-- 4
-- Un left Join entre vuelos y pilotos  asi se obtiene cada vuelo y el nombre de los pilotos 
SELECT
  v.VueloID,
  v.Origen,
  v.Destino,
  v.Fecha,
  v.Hora,
  p.Nombre AS Piloto
FROM Vuelos AS v
LEFT JOIN Pilotos AS p
  ON v.PilotoID = p.PilotoID;
  go

  -- 5 Mostrar los nombres de los clientes que han
 -- comprado boletos para vuelos con destino a Guatemala
 --se hace relacion de clientes boletos y vuelos para relacionar cada venta por cada vuelo
 -- se hace un filtro cuyo destino sea Ciudad de Guatemala
 -- Distict se asegura listar una sola vez por cliente 
 SELECT DISTINCT 
    c.Nombre AS Cliente
FROM Clientes AS c
INNER JOIN Boletos AS b
    ON c.ClienteID = b.ClienteID
INNER JOIN Asientos AS a
    ON b.AsientoID = a.AsientoID
INNER JOIN Vuelos AS v
    ON a.VueloID = v.VueloID
WHERE v.Destino = 'Ciudad de Guatemala';
GO

select * from Vuelos
select * from Boletos --Utilizando para ver las tablas y tener relacion 

--6 
-- cada vuelo le corresponde un alimento donde se usa Exist por vuelo y alimento 
-- 

SELECT
  v.VueloID,
  v.Origen,
  v.Destino,
  v.Fecha,
  v.Hora
FROM Vuelos AS v
WHERE EXISTS (
  SELECT 1
  FROM VuelosAlimentos AS va
  WHERE va.VueloID = v.VueloID
);

-- 7 Pilotos que han volado mas de 5 veces 
-- la subconsulta de este agrupa vuelo con pilotos Id utilizando el Having count > 5 para quedarse como pilotos que han volado 5 veces 


SELECT p.Nombre AS Piloto
FROM Pilotos AS p
WHERE p.PilotoID IN (
  SELECT v.PilotoID
  FROM Vuelos AS v
  GROUP BY v.PilotoID
  HAVING COUNT(*) > 5
);
 
-- 8 
-- cada alimento que este destinado a "a" 
SELECT a.Nombre AS Alimento
FROM Alimentos AS a
WHERE  EXISTS (
  SELECT 1
  FROM Vuelos AS v
  WHERE v.Destino = 'San Salvador'
    AND EXISTS (
      SELECT 1
      FROM VuelosAlimentos AS va
      WHERE va.VueloID    = v.VueloID
        AND va.AlimentoID = a.AlimentoID
    )
);
                      



-- inner join 1.1
-- Join clientes y boletos (relacion)
-- Join Boletos y vuelos
SELECT
  c.Nombre    AS Cliente,
  v.VueloID,
  v.Origen,
  v.Destino
FROM Clientes AS c
INNER JOIN Boletos AS b
    ON c.ClienteID = b.ClienteID
INNER JOIN Asientos AS a
    ON b.AsientoID = a.AsientoID
INNER JOIN Vuelos AS v
    ON a.VueloID = v.VueloID;
GO


-- 1.2
-- Left join Se asegura que todos tengan piloto incluso si no hay piloto 
SELECT
  v.VueloID,
  p.Nombre   AS Piloto,
  v.Fecha
FROM Vuelos AS v
LEFT JOIN Pilotos AS p
  ON v.PilotoID = p.PilotoID;

  --1.3 Obtener los nombres de alimentos servidos en cada vuelo
  -- relacion de vuelo-alimento
  -- se lista que alimento se sirve en cada vuelo
  SELECT
  a.Nombre    AS Alimento,
  v.Origen,
  v.Destino
FROM Alimentos AS a
JOIN VuelosAlimentos AS va
  ON a.AlimentoID = va.AlimentoID
JOIN Vuelos AS v
  ON va.VueloID = v.VueloID
WHERE v.Origen = 'Ciudad de Guatemala';
GO

  -- 1.4 
  -- Se obtiene cada reserva, el cliente, asiento y el vuelo 
  /*
  SELECT
  c.Nombre    AS Cliente,
  b.AsientoID,
  v.Destino
FROM Clientes AS c
JOIN Boletos AS b
  ON c.ClienteID = b.ClienteID
JOIN Asientos AS a
   ON a.AsientoID = b.AsientoID
JOIN Vuelos AS v
  ON v.VueloID = a.VueloID;

  */
SELECT
    c.Nombre AS Cliente,      
    b.AsientoID,              
    v.Destino                


FROM Clientes AS c           
JOIN Boletos AS b
    ON c.ClienteID = b.ClienteID  -- Relacionamos al cliente con su boleto

JOIN Asientos AS a
    ON a.AsientoID = b.AsientoID  -- Relacionamos el boleto con un asiento

JOIN Vuelos AS v
    ON v.VueloID = a.VueloID      -- Relacionamos el asiento con su vuelo

  Select * FROM Asientos
  select *from Clientes
  select *from Vuelos



 -- 2.1 Listar todos los vuelos y, si existen, los alimentos disponibles en cada uno.
-- Usamos LEFT JOIN de Vuelos hacia la tabla intermedia y luego hacia Alimentos.
SELECT
  v.VueloID,                 
  v.Origen,               
  v.Destino,                 
  a.Nombre     AS Alimento  
FROM Vuelos AS v
  LEFT JOIN VuelosAlimentos AS va  -- Tabla intermedia que asocia vuelos con alimentos
    ON v.VueloID = va.VueloID      -- Emparejamos cada vuelo con sus registros en la intermedia
  LEFT JOIN Alimentos AS a        -- Traemos los datos de los alimentos
    ON va.AlimentoID = a.AlimentoID;  -- Emparejamos con la tabla Alimentos

	-- 2.2 Listar todos los alimentos y en qué vuelos se han servido.
-- Incluimos también los alimentos que no se han servido en ningún vuelo.
SELECT
  a.AlimentoID,             
  a.Nombre    AS Alimento,   
  v.Destino                  
FROM Alimentos AS a
  LEFT JOIN VuelosAlimentos AS va  
    ON a.AlimentoID = va.AlimentoID
  LEFT JOIN Vuelos AS v            
    ON va.VueloID = v.VueloID;

-- 2.3 Mostrar todas las rutas posibles y los vuelos programados para cada una,
-- incluso si no se ha usado la ruta aún.
-- Asumimos que existe una tabla Rutas con RutaID, Origen y Destino,
-- y que Vuelos tiene columna RutaID como clave foránea.

SELECT
  r.RutaID,                 
  r.Origen   AS RutaOrigen,  
  r.Destino  AS RutaDestino, 
  v.VueloID,                 
  v.Fecha,                   
  v.Hora                     
FROM Rutas AS r
  LEFT JOIN Vuelos AS v      
    ON r.RutaID = v.RutaID;  

-- 3.1 Listar todos los vuelos y todas las rutas, mostrando coincidencias cuando existan
-- 1) Si aún no existe, añade la columna RutaID y la FK
ALTER TABLE Vuelos
ADD RutaID INT NULL;
GO

ALTER TABLE Vuelos
ADD CONSTRAINT FK_Vuelos_Rutas
  FOREIGN KEY (RutaID) REFERENCES Rutas(RutaID);
GO

-- 2) (Opcional) Rellena RutaID según origen/destino si lo necesitas
UPDATE v
SET v.RutaID = r.RutaID
FROM Vuelos AS v
JOIN Rutas  AS r
  ON v.Origen = r.Origen
 AND v.Destino = r.Destino;
GO

-- 3) Ahora sí, el FULL OUTER JOIN para listar vuelos y rutas combinadas
SELECT
  v.VueloID,
  v.Origen    AS OrigenVuelo,
  v.Destino   AS DestinoVuelo,
  r.RutaID,
  r.Origen    AS OrigenRuta,
  r.Destino   AS DestinoRuta
FROM Vuelos AS v
FULL OUTER JOIN Rutas AS r
  ON v.RutaID = r.RutaID;
GO
-- si utilice aqui chat no se cual era el error 

-- 4.1 
SELECT
  p.PilotoID,            
  p.Nombre    AS Piloto,  
  a.AeromosaID,          
  a.Nombre    AS Aeromosa 
FROM Pilotos AS p
  CROSS JOIN Aeromosas AS a; --Cross join combina pilotos con aeromosa

SELECT
  COUNT(*) AS TotalCombinaciones
FROM Pilotos AS p
  CROSS JOIN Aeromosas AS a;

-- 4.2 
SELECT
  f.AlimentoID,            
  f.Nombre     AS Alimento,
  v.VueloID,               
  v.Origen,                
  v.Destino               
FROM Alimentos AS f
  CROSS JOIN Vuelos AS v;    --Cross combinaciones de Alimento  Vuelo


SELECT
  COUNT(*) AS TotalSimulaciones
FROM Alimentos AS f
  CROSS JOIN Vuelos AS v;

-- 5.1 Mas vuelos 
SELECT TOP 1            -- Se obtiene el primer registro del piloto             
  p.PilotoID,                        
  p.Nombre    AS Piloto,            
  COUNT(v.VueloID) AS TotalVuelos   
FROM Pilotos AS p                    
  JOIN Vuelos AS v                 
    ON p.PilotoID = v.PilotoID       
GROUP BY                       -- Se agrupa por piloto para contar los vuelos realizados     
  p.PilotoID,
  p.Nombre
ORDER BY                          -- se ordena de mayor a menor numero de vuelos   
  COUNT(v.VueloID) DESC;

  Select * From Vuelos
  Select * From Boletos

  -- 5.2 Mostrar los vuelos con comida distinatas
SELECT
  v.VueloID,                      
  v.Origen,                         
  v.Destino,                       
  COUNT(DISTINCT va.AlimentoID)     -- Contamos alimentos distintos por vuelo
    AS NumAlimentos
FROM Vuelos AS v                   
  JOIN VuelosAlimentos AS va         
    ON v.VueloID = va.VueloID       
GROUP BY                            -- Agrupamos por vuelo para aplicar el HAVING
  v.VueloID,
  v.Origen,
  v.Destino
HAVING                              -- Filtramos solo los que superan 2 alimentos distintos
  COUNT(DISTINCT va.AlimentoID) > 2;

  -- 5.3 VUELOS QUE SE RESERVARON PARA TEGU
SELECT DISTINCT
  c.ClienteID,                      
  c.Nombre     AS Cliente           
FROM Clientes AS c                 
  JOIN Boletos AS b                 
    ON c.ClienteID = b.ClienteID    
  JOIN Asientos AS a               
    ON b.AsientoID = a.AsientoID    
  JOIN Vuelos AS v                  
    ON a.VueloID = v.VueloID        
WHERE
  v.Destino = 'Tegucigalpa';       

-- 6.1 Mostrar los vuelos con el precio promedio de asiento más alto
SELECT
  v.VueloID,                       
  v.Origen,                        
  v.Destino,                       
  sa.avg_precio                    
FROM Vuelos AS v
  JOIN (
    SELECT
      VueloID,
      AVG(Precio) AS avg_precio    -- Se calcula el promedio de precios 
    FROM Asientos
    GROUP BY VueloID               
  ) AS sa
    ON v.VueloID = sa.VueloID      -- SE UNE VUELO CON SU PROMEDIO
WHERE
  sa.avg_precio = (               
    SELECT MAX(avg_precio)          -- SE FILTRA SOLO EL VUELO CON EL MAYOR PROMEDIO
    FROM (
      SELECT AVG(Precio) AS avg_precio
      FROM Asientos
      GROUP BY VueloID                -- SUBCONSULTA DEL PROMEDIO
    ) AS sub_max                 
  );

  -- 6.2 cliente con el boleto mas caro 
SELECT
  c.Nombre AS Cliente            
FROM Boletos AS b
  JOIN Asientos AS a              
    ON b.AsientoID = a.AsientoID   --union de boletos y clientes 
  JOIN Clientes AS c              
    ON b.ClienteID = c.ClienteID
WHERE
  a.Precio = (                    -- Se Filtra el boleto cuyo precio de asiento es máximo
    SELECT MAX(a2.Precio)
    FROM Boletos AS b2
      JOIN Asientos AS a2
        ON b2.AsientoID = a2.AsientoID
  );

  -- 6.3 Listar los vuelos con alimentos cuyo precio es mayor al promedio de todos los alimentos
SELECT DISTINCT
  v.VueloID,                    
  v.Origen,                       
  v.Destino,                      
  al.Nombre   AS Alimento,        
  al.Precio                       
FROM Vuelos AS v
  JOIN VuelosAlimentos AS va       --  Asocia vuelos con alimentos servidos
    ON v.VueloID = va.VueloID
  JOIN Alimentos AS al            -- Obtiene datos de cada alimento
    ON va.AlimentoID = al.AlimentoID
WHERE
  al.Precio > (                   -- Solo aquellos alimentos cuyo precio supera
    SELECT AVG(Precio)            --    el precio promedio de todos los alimentos
    FROM Alimentos
  );

