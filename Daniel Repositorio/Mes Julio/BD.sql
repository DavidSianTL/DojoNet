USE master;

CREATE DATABASE AutoExpressDB;

USE AutoExpressDB;

-- Tabla Estados
CREATE TABLE Estados (
    idEstado INT PRIMARY KEY IDENTITY(1,1),
    estado NVARCHAR(100),
    descripcion NVARCHAR(100)
);

-- Tabla Tipo de Vehículo
CREATE TABLE TipoVehiculo (
    idTipoVehiculo INT PRIMARY KEY IDENTITY(1,1),
    tipo NVARCHAR(100),
    descripcion NVARCHAR(100)
);

-- Tabla Países
CREATE TABLE Paises (
    idPais INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(100),
    codigo NVARCHAR(10)
);

-- Tabla Vehículos
CREATE TABLE Vehiculos (
    idVehiculo INT PRIMARY KEY IDENTITY(1,1),
    marca NVARCHAR(100),
    modelo NVARCHAR(100),
    anio INT,
    precio DECIMAL(10,2),
    fk_IdTipoVehiculo INT,
    fk_IdEstado INT,
    fk_IdPais INT,
    CONSTRAINT fk_Tipo FOREIGN KEY (fk_IdTipoVehiculo) REFERENCES TipoVehiculo(idTipoVehiculo),
    CONSTRAINT fk_Estado FOREIGN KEY (fk_IdEstado) REFERENCES Estados(idEstado),
    CONSTRAINT fk_Pais FOREIGN KEY (fk_IdPais) REFERENCES Paises(idPais)
);

-- Tabla Logs
CREATE TABLE Logs (
    idLog INT PRIMARY KEY IDENTITY(1,1),
    fecha DATETIME DEFAULT GETDATE(),
    mensaje NVARCHAR(500)
);

-- Tabla Bitácora
CREATE TABLE Bitacora (
    idBitacora INT PRIMARY KEY IDENTITY(1,1),
    fecha DATETIME DEFAULT GETDATE(),
    accion NVARCHAR(100),
    descripcion NVARCHAR(200),
    usuario NVARCHAR(50)
);
