USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('DBPracticaSubconsultas') IS NOT NULL
BEGIN
    ALTER DATABASE DBPracticaSubconsultas SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBPracticaSubconsultas;
END
GO

-- Crear la base de datos
CREATE DATABASE DBPracticaSubconsultas;
GO

-- Usar la base de datos recién creada
USE DBPracticaSubconsultas;
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