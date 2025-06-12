-- Empleados
INSERT INTO Empleado (NombreEmpleado)
VALUES ('Juan Pérez'), ('Ana Torres'), ('Carlos Ruiz'), ('María López');

-- Puestos
INSERT INTO Puesto (NombrePuesto, SalarioBase)
VALUES 
('Analista', 800.00),
('Administrador', 500.00),
('Contador', 950.00);

-- Jornadas
INSERT INTO Jornada (TipoJornada)
VALUES ('Tiempo Completo'), ('Medio Tiempo');

-- Tipos de Pago
INSERT INTO TipoPago (Descripcion)
VALUES ('Transferencia'), ('Cheque'), ('Efectivo');

-- Planilla (usando claves foráneas correctas)
INSERT INTO Planilla (IDEmpleado, IDPuesto, IDJornada, IDTipoPago, FechaPago, HorasExtras, Descuento, PagoNeto)
VALUES
-- Juan Pérez - Analista - Tiempo Completo - Transferencia
(1, 1, 1, 1, '2024-04-30', 5, 50.00, 850.00),

-- Ana Torres - Administrador - Medio Tiempo - Cheque
(2, 2, 2, 2, '2024-04-30', 2, 30.00, 520.00),

-- Carlos Ruiz - Analista - Tiempo Completo - Transferencia
(3, 1, 1, 1, '2024-04-30', 3, 40.00, 835.00),

-- María López - Contador - Tiempo Completo - Transferencia
(4, 3, 1, 1, '2024-04-30', 0, 60.00, 890.00);
