
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
