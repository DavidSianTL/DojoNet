-- Insertar clientes
USE BancoDB;
GO


INSERT INTO Clientes (ClienteID, Nombre, Email)
VALUES
(1, 'Juan P�rez', 'juan.perez@email.com'),
(2, 'Ana G�mez', 'ana.gomez@email.com'),
(3, 'Carlos L�pez', 'carlos.lopez@email.com');

INSERT INTO Clientes (ClienteID, Nombre, Email)
VALUES
(4, 'Mar�a Torres', 'maria.torres@email.com'),
(5, 'Pedro Mart�nez', 'pedro.martinez@email.com'),
(6, 'Laura S�nchez', 'laura.sanchez@email.com'),
(7, 'Miguel Ruiz', 'miguel.ruiz@email.com'),
(8, 'Sofia Romero', 'sofia.romero@email.com'),
(9, 'Roberto Garc�a', 'roberto.garcia@email.com'),
(10, 'Luc�a P�rez', 'lucia.perez@email.com'),
(11, 'Javier D�az', 'javier.diaz@email.com'),
(12, 'Raquel Fern�ndez', 'raquel.fernandez@email.com'),
(13, 'Andr�s Castro', 'andres.castro@email.com'),
(14, 'Esteban P�rez', 'esteban.perez@email.com');
GO

-- Insertar cuentas
INSERT INTO Cuentas (CuentaID, ClienteID, TipoCuenta, Saldo)
VALUES
(101, 1, 'Ahorro', 1500.00),
(102, 2, 'Monetario', 3000.00),
(103, 3, 'Ahorro', 500.00);
GO
INSERT INTO Cuentas (CuentaID, ClienteID, TipoCuenta, Saldo)
VALUES
(104, 4, 'Ahorro', 1500.00),
(105, 5, 'Monetario', 3000.00),
(106, 6, 'Ahorro', 500.00),
(107, 7, 'Monetario', 4000.00),
(108, 8, 'Ahorro', 2500.00),
(109, 9, 'Ahorro', 1000.00),
(110, 10, 'Monetario', 7000.00),
(111, 11, 'Ahorro', 1200.00),
(112, 12, 'Monetario', 5000.00),
(113, 13, 'Ahorro', 2200.00),
(114, 14, 'Ahorro', 3500.00);
GO

-- Insertar transacciones
INSERT INTO Transacciones (TransaccionID, CuentaID, Fecha, Monto, TipoTransaccion)
VALUES
(1001, 101, '2025-05-01', 200.00, 'Dep�sito'),
(1002, 102, '2025-05-02', 500.00, 'Retiro'),
(1003, 103, '2025-05-03', 100.00, 'Dep�sito');
GO
INSERT INTO Transacciones (TransaccionID, CuentaID, Fecha, Monto, TipoTransaccion)
VALUES
(1004, 104, '2025-01-15', 200.00, 'Dep�sito'),
(1005, 105, '2025-01-16', 500.00, 'Retiro'),
(1006, 106, '2025-01-17', 100.00, 'Dep�sito'),
(1007, 107, '2025-01-18', 150.00, 'Retiro'),
(1008, 108, '2025-01-19', 400.00, 'Dep�sito'),
(1009, 109, '2025-01-20', 50.00, 'Retiro'),
(1010, 110, '2025-01-21', 300.00, 'Dep�sito'),
(1011, 111, '2025-01-22', 120.00, 'Retiro'),
(1012, 112, '2025-01-23', 250.00, 'Dep�sito'),
(1013, 113, '2025-01-24', 350.00, 'Retiro'),
(1014, 114, '2025-03-01', 500.00, 'Dep�sito');
GO

