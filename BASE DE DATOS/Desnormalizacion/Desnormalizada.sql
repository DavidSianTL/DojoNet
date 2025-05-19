-- Crear nueva base de datos normalizada
CREATE DATABASE PlanillaNormalizada;
GO
USE PlanillaNormalizada;
GO

-- Tabla: Empleados
CREATE TABLE Empleado (
    IDEmpleado INT PRIMARY KEY IDENTITY(1,1),
    NombreEmpleado VARCHAR(100)
);

-- Tabla: Puestos
CREATE TABLE Puesto (
    IDPuesto INT PRIMARY KEY IDENTITY(1,1),
    NombrePuesto VARCHAR(50),
    SalarioBase DECIMAL(10,2)
);

-- Tabla: Jornadas
CREATE TABLE Jornada (
    IDJornada INT PRIMARY KEY IDENTITY(1,1),
    TipoJornada VARCHAR(50)
);

-- Tabla: Tipos de Pago
CREATE TABLE TipoPago (
    IDTipoPago INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(50)
);

-- Tabla: Planilla (registro de pagos)
CREATE TABLE Planilla (
    IDPlanilla INT PRIMARY KEY IDENTITY(1,1),
    IDEmpleado INT,
    IDPuesto INT,
    IDJornada INT,
    IDTipoPago INT,
    FechaPago DATE,
    HorasExtras INT,
    Descuento DECIMAL(10,2),
    PagoNeto DECIMAL(10,2),
    FOREIGN KEY (IDEmpleado) REFERENCES Empleado(IDEmpleado),
    FOREIGN KEY (IDPuesto) REFERENCES Puesto(IDPuesto),
    FOREIGN KEY (IDJornada) REFERENCES Jornada(IDJornada),
    FOREIGN KEY (IDTipoPago) REFERENCES TipoPago(IDTipoPago)
);
