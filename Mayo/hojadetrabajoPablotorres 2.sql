CREATE DATABASE EmpresaDB;
GO

USE EmpresaDB;
GO
-- Insertar empleados
INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase)
VALUES 
('Ana', 'Martínez', '1990-04-12', '2015-01-10', 'Analista', 800.00),
('Luis', 'Pérez', '1985-07-19', '2012-03-15', 'Supervisor', 1200.00),
('Carla', 'Gómez', '1992-11-05', '2018-06-20', 'Asistente', 650.00);

-- Insertar planillas
INSERT INTO Planilla (EmpleadoID, Mes, Anio, HorasTrabajadas, Bonos, Deducciones)
VALUES 
(1, 4, 2025, 160, 100.00, 50.00),
(2, 4, 2025, 160, 200.00, 70.00),
(3, 4, 2025, 160, 50.00, 30.00);

-- Insertar pagos
INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
VALUES
(1, '2025-05-01', 850.00, 'Transferencia'),
(2, '2025-05-01', 1330.00, 'Cheque'),
(3, '2025-05-01', 670.00, 'Efectivo');
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


CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY(1,1),
    PlanillaID INT FOREIGN KEY REFERENCES Planilla(PlanillaID),
    FechaPago DATE,
    MontoPagado DECIMAL(10,2),
    MetodoPago NVARCHAR(50)
);

CREATE TABLE Logs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT GETDATE(),
    Procedimiento NVARCHAR(100),
    Mensaje NVARCHAR(MAX),
    Error BIT
);

GO
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


        SELECT TOP 1 @PlanillaID = PlanillaID,
                     @Pago = (E.SalarioBase + P.Bonos - P.Deducciones)
        FROM Planilla P
        INNER JOIN Empleados E ON P.EmpleadoID = E.EmpleadoID
        WHERE P.EmpleadoID = @EmpleadoID AND Mes = @Mes AND Anio = @Anio;

        IF @PlanillaID IS NULL
        BEGIN
            THROW 50001, 'No se encontró la planilla para el empleado en ese mes.', 1;
        END

      
        INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
        VALUES (@PlanillaID, GETDATE(), @Pago, @MetodoPago);

        
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', CONCAT('Pago registrado exitosamente para empleado ID: ', @EmpleadoID), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', ERROR_MESSAGE(), 1);
    END CATCH
END;
GO
--A
SELECT EmpleadoID, Nombre, Apellido, Puesto
FROM Empleados
WHERE Activo = 1;

-- B?
SELECT pg.PagoID,
       pg.FechaPago,
       pg.MontoPagado,
       pg.MetodoPago
FROM Pagos pg
JOIN Planilla p ON pg.PlanillaID = p.PlanillaID
WHERE p.Mes  = 5
  AND p.Anio = 2025;

-- C
SELECT 
  e.EmpleadoID,
  e.Nombre + ' ' + e.Apellido AS Empleado,
  SUM(pg.MontoPagado)        AS TotalPagado
FROM Empleados e
JOIN Planilla p ON e.EmpleadoID = p.EmpleadoID
JOIN Pagos pg      ON p.PlanillaID = pg.PlanillaID
GROUP BY e.EmpleadoID, e.Nombre, e.Apellido;
--B?
SELECT EmpleadoID, Nombre, Apellido,
       dbo.fn_CalcularEdad(FechaNacimiento) AS Edad
FROM Empleados;

EXEC sp_RegistrarPago @EmpleadoID = 3, @Mes = 5, @Anio = 2025, @MetodoPago = 'Transferencia';

SELECT * FROM Logs ORDER BY Fecha DESC;

-- C
EXEC sp_RegistrarPago @EmpleadoID = 2, @Mes = 1, @Anio = 2025, @MetodoPago = 'Efectivo';

SELECT * FROM Logs WHERE Error = 1 AND Procedimiento = 'sp_RegistrarPago' ORDER BY Fecha DESC;

CREATE VIEW vw_ResumenPagos AS
SELECT 
  e.EmpleadoID,
  e.Nombre + ' ' + e.Apellido AS Empleado,
  e.SalarioBase,
  SUM(p.Bonos)       AS TotalBonos,
  SUM(p.Deducciones) AS TotalDeducciones,
  SUM(pg.MontoPagado)AS TotalPagado
FROM Empleados e
LEFT JOIN Planilla p ON e.EmpleadoID = p.EmpleadoID
LEFT JOIN Pagos    pg ON p.PlanillaID = pg.PlanillaID
GROUP BY 
  e.EmpleadoID,
  e.Nombre,
  e.Apellido,
  e.SalarioBase;

CREATE TRIGGER trg_Empleados_Update
ON Empleados
AFTER UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO Logs (Procedimiento, Mensaje, Error)
  SELECT 
    'trg_Empleados_Update',
    'Empleado modificado. ID = ' + CAST(i.EmpleadoID AS NVARCHAR(10))
      + '; Nombre=' + i.Nombre
      + ', Apellido=' + i.Apellido
      + ', Puesto=' + i.Puesto,
    0
  FROM inserted i;
END;
GO
