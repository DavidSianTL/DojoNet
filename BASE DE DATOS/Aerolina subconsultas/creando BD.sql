-- Crear la base de datos
CREATE DATABASE AeroDB;
GO

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
