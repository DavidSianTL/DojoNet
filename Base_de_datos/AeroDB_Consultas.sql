-- Crear la base de datos
CREATE DATABASE AeroDB;
GO

-- Usar la base de datos
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

-- CONSULTAS

-- Consulta 1: Listar todos los vuelos con su origen, destino y fecha
SELECT VueloID, Origen, Destino, Fecha
FROM Vuelos;
GO

-- Consulta 2: Mostrar los clientes que han comprado boletos para vuelos en una fecha específica
-- Reemplaza '2025-06-15' con la fecha que desees consultar
SELECT C.ClienteID, C.Nombre, C.Correo, B.FechaCompra
FROM Clientes C
JOIN Boletos B ON C.ClienteID = B.ClienteID
JOIN Asientos A ON B.AsientoID = A.AsientoID
JOIN Vuelos V ON A.VueloID = V.VueloID
WHERE V.Fecha = '2025-06-15';
GO

-- Consulta 3: Indicar qué alimentos están disponibles en vuelos con origen en 'Ciudad de Guatemala'
SELECT DISTINCT A.Nombre AS Alimento, A.Precio
FROM Alimentos A
JOIN VuelosAlimentos VA ON A.AlimentoID = VA.AlimentoID
JOIN Vuelos V ON VA.VueloID = V.VueloID
WHERE V.Origen = 'Ciudad de Guatemala';
GO
