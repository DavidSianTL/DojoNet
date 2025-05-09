CREATE DATABASE EmpresaDB;
GO

USE EmpresaDB;
GO


-- Tabla de Empleados
CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    FechaNacimiento DATE,
    FechaIngreso DATE,
    Puesto NVARCHAR(100),
    SalarioBase DECIMAL(10,2),
    Activo BIT DEFAULT 1
);

-- Tabla de Planilla
CREATE TABLE Planilla (
    PlanillaID INT PRIMARY KEY IDENTITY(1,1),
    EmpleadoID INT FOREIGN KEY REFERENCES Empleados(EmpleadoID),
    Mes INT,
    Anio INT,
    HorasTrabajadas INT,
    Bonos DECIMAL(10,2),
    Deducciones DECIMAL(10,2),
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Tabla de Pagos
CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY(1,1),
    PlanillaID INT FOREIGN KEY REFERENCES Planilla(PlanillaID),
    FechaPago DATE,
    MontoPagado DECIMAL(10,2),
    MetodoPago NVARCHAR(50)
);

-- Tabla de Logs
CREATE TABLE Logs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT GETDATE(),
    Procedimiento NVARCHAR(100),
    Mensaje NVARCHAR(MAX),
    Error BIT
);
