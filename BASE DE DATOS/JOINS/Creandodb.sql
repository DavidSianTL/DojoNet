--
-- Crear la base de datos
CREATE DATABASE BancoDB;
GO
USE BancoDB;
GO

-- Tabla de clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100)
);
GO

-- Tabla de cuentas bancarias
CREATE TABLE Cuentas (
    CuentaID INT PRIMARY KEY,
    ClienteID INT,
    TipoCuenta NVARCHAR(50),
    Saldo DECIMAL(10, 2),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);
GO

-- Tabla de transacciones
CREATE TABLE Transacciones (
    TransaccionID INT PRIMARY KEY,
    CuentaID INT,
    Fecha DATETIME,
    Monto DECIMAL(10, 2),
    TipoTransaccion NVARCHAR(50),
    FOREIGN KEY (CuentaID) REFERENCES Cuentas(CuentaID)
);
GO
