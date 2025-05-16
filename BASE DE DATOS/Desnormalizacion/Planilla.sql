-- Crear la base de datos
CREATE DATABASE PlanillaDesnormalizada;
GO

USE PlanillaDesnormalizada;
GO

-- Crear tabla desnormalizada
CREATE TABLE Planilla (
    ID INT PRIMARY KEY IDENTITY(1,1),
    NombreEmpleado VARCHAR(100),
    Puesto VARCHAR(50),
    Jornada VARCHAR(20),
    SalarioBase DECIMAL(10, 2),
    FechaPago DATE,
    HorasExtras INT,
    TipoPago VARCHAR(30),
    Descuento DECIMAL(10, 2),
    PagoNeto DECIMAL(10, 2)
);
GO

-- Insertar datos de ejemplo
INSERT INTO Planilla (NombreEmpleado, Puesto, Jornada, SalarioBase, FechaPago, HorasExtras, TipoPago, Descuento, PagoNeto)
VALUES 
('Juan Pérez', 'Analista', 'Tiempo Completo', 800.00, '2024-04-30', 5, 'Transferencia', 50.00, 850.00),
('Ana Torres', 'Administrador', 'Medio Tiempo', 500.00, '2024-04-30', 2, 'Cheque', 30.00, 520.00),
('Carlos Ruiz', 'Analista', 'Tiempo Completo', 800.00, '2024-04-30', 3, 'Transferencia', 40.00, 835.00),
('María López', 'Contador', 'Tiempo Completo', 950.00, '2024-04-30', 0, 'Transferencia', 60.00, 890.00),
('Lucía Sánchez', 'Administrador', 'Medio Tiempo', 500.00, '2024-04-30', 4, 'Cheque', 25.00, 545.00),
('Pedro García', 'Contador', 'Tiempo Completo', 950.00, '2024-04-30', 1, 'Efectivo', 50.00, 900.00),
('Ernesto Vega', 'Supervisor', 'Tiempo Completo', 1200.00, '2024-04-30', 6, 'Transferencia', 80.00, 1270.00),
('Sofía Díaz', 'Analista', 'Medio Tiempo', 600.00, '2024-04-30', 2, 'Cheque', 20.00, 610.00),
('Marco Jiménez', 'Contador', 'Tiempo Completo', 950.00, '2024-04-30', 3, 'Transferencia', 60.00, 920.00),
('Elena Rivas', 'Supervisor', 'Tiempo Completo', 1200.00, '2024-04-30', 4, 'Efectivo', 90.00, 1250.00);
