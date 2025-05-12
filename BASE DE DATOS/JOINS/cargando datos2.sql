-- Insertar datos adicionales en la tabla Clientes
USE BancoDB;
GO

INSERT INTO Clientes (ClienteID, Nombre, Email)
VALUES
(4, 'María Torres', 'maria.torres@email.com'),
(5, 'Pedro Martínez', 'pedro.martinez@email.com'),
(6, 'Laura Sánchez', 'laura.sanchez@email.com'),
(7, 'Miguel Ruiz', 'miguel.ruiz@email.com'),
(8, 'Sofia Romero', 'sofia.romero@email.com'),
(9, 'Roberto García', 'roberto.garcia@email.com'),
(10, 'Lucía Pérez', 'lucia.perez@email.com'),
(11, 'Javier Díaz', 'javier.diaz@email.com'),
(12, 'Raquel Fernández', 'raquel.fernandez@email.com'),
(13, 'Andrés Castro', 'andres.castro@email.com'),
(14, 'Esteban Pérez', 'esteban.perez@email.com');
GO

USE BancoDB;
GO
-- Insertar datos en la tabla Cuentas
INSERT INTO Cuentas (CuentaID, ClienteID, TipoCuenta, Saldo)
VALUES
(201, 4, 'Ahorro', 1500.00),
(202, 5, 'Monetario', 3000.00),
(203, 6, 'Ahorro', 500.00),
(204, 7, 'Monetario', 4000.00),
(205, 8, 'Ahorro', 2500.00),
(206, 9, 'Ahorro', 1000.00),
(207, 10, 'Monetario', 7000.00),
(208, 11, 'Ahorro', 1200.00),
(209, 12, 'Monetario', 5000.00),
(210, 13, 'Ahorro', 2200.00),
(211, 14, 'Ahorro', 3500.00);
GO

-- Insertar datos en la tabla Transacciones
INSERT INTO Transacciones (TransaccionID, CuentaID, Fecha, Monto, TipoTransaccion)
VALUES
(1001, 201, '2025-01-15', 200.00, 'Depósito'),
(1002, 202, '2025-01-16', 500.00, 'Retiro'),
(1003, 203, '2025-01-17', 100.00, 'Depósito'),
(1004, 204, '2025-01-18', 150.00, 'Retiro'),
(1005, 205, '2025-01-19', 400.00, 'Depósito'),
(1006, 206, '2025-01-20', 50.00, 'Retiro'),
(1007, 207, '2025-01-21', 300.00, 'Depósito'),
(1008, 208, '2025-01-22', 120.00, 'Retiro'),
(1009, 209, '2025-01-23', 250.00, 'Depósito'),
(1010, 210, '2025-01-24', 350.00, 'Retiro'),
(1011, 211, '2025-03-01', 500.00, 'Depósito');
GO
