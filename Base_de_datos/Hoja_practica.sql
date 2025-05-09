-- Crear la base de datos
CREATE DATABASE EmpresaDB;
GO

-- Usar la base de datos
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
GO

-- Insertar empleados
INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase)
VALUES 
('Ana', 'Martínez', '1990-04-12', '2015-01-10', 'Analista', 800.00),
('Luis', 'Pérez', '1985-07-19', '2012-03-15', 'Supervisor', 1200.00),
('Carla', 'Gómez', '1992-11-05', '2018-06-20', 'Asistente', 650.00);
GO

-- Insertar planillas
INSERT INTO Planilla (EmpleadoID, Mes, Anio, HorasTrabajadas, Bonos, Deducciones)
VALUES 
(1, 4, 2025, 160, 100.00, 50.00),
(2, 4, 2025, 160, 200.00, 70.00),
(3, 4, 2025, 160, 50.00, 30.00);
GO

-- Insertar pagos
INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
VALUES
(1, '2025-05-01', 850.00, 'Transferencia'),
(2, '2025-05-01', 1330.00, 'Cheque'),
(3, '2025-05-01', 670.00, 'Efectivo');
GO

-- Función para calcular la edad
CREATE FUNCTION fn_CalcularEdad (@FechaNacimiento DATE)
RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @FechaNacimiento, GETDATE()) -
           CASE WHEN MONTH(@FechaNacimiento) > MONTH(GETDATE()) 
                 OR (MONTH(@FechaNacimiento) = MONTH(GETDATE()) AND DAY(@FechaNacimiento) > DAY(GETDATE()))
                THEN 1 ELSE 0 END;
END;
GO

-- Procedimiento almacenado para registrar pagos
IF OBJECT_ID('sp_RegistrarPago', 'P') IS NOT NULL
    DROP PROCEDURE sp_RegistrarPago;
GO

CREATE PROCEDURE sp_RegistrarPago
    @EmpleadoID INT,
    @Mes INT,
    @Anio INT,
    @MetodoPago NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        DECLARE @PlanillaID INT, @Pago DECIMAL(10,2);

        -- Obtener planilla
        SELECT TOP 1 @PlanillaID = PlanillaID,
                     @Pago = (E.SalarioBase + P.Bonos - P.Deducciones)
        FROM Planilla P
        INNER JOIN Empleados E ON P.EmpleadoID = E.EmpleadoID
        WHERE P.EmpleadoID = @EmpleadoID AND Mes = @Mes AND Anio = @Anio;

        -- Validar existencia de la planilla
        IF @PlanillaID IS NULL
        BEGIN
            THROW 50001, 'No se encontró la planilla para el empleado en ese mes.', 1;
        END

        -- Validar que el monto a pagar sea correcto
        IF @Pago IS NULL OR @Pago <= 0
        BEGIN
            THROW 50002, 'El monto a pagar es inválido.', 1;
        END

        -- Insertar pago
        INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
        VALUES (@PlanillaID, GETDATE(), @Pago, @MetodoPago);

        -- Log éxito
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', CONCAT('Pago registrado exitosamente para empleado ID: ', @EmpleadoID), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO

-- Vista de resumen de pagos
IF OBJECT_ID('vw_ResumenPagos', 'V') IS NOT NULL
    DROP VIEW vw_ResumenPagos;
GO

CREATE VIEW vw_ResumenPagos AS
SELECT E.EmpleadoID, E.Nombre, E.Apellido, E.SalarioBase, 
       COALESCE(SUM(PL.Bonos), 0) AS TotalBonos, 
       COALESCE(SUM(PL.Deducciones), 0) AS TotalDeducciones, 
       COALESCE(SUM(P.MontoPagado), 0) AS TotalPagado
FROM Empleados E
LEFT JOIN Planilla PL ON E.EmpleadoID = PL.EmpleadoID
LEFT JOIN Pagos P ON PL.PlanillaID = P.PlanillaID
GROUP BY E.EmpleadoID, E.Nombre, E.Apellido, E.SalarioBase;
GO

-- Trigger para registrar cambios en empleados
IF OBJECT_ID('trg_RegistroCambiosEmpleados', 'TR') IS NOT NULL
    DROP TRIGGER trg_RegistroCambiosEmpleados;
GO

CREATE TRIGGER trg_RegistroCambiosEmpleados
ON Empleados
AFTER UPDATE
AS
BEGIN
    INSERT INTO Logs (Procedimiento, Mensaje, Error)
    VALUES ('trg_RegistroCambiosEmpleados', 'Se actualizó información de un empleado.', 0);
END;
GO
