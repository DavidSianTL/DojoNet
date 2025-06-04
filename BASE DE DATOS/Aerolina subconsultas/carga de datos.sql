USE AeroDB;
GO

-- Insertar clientes
INSERT INTO Clientes (ClienteID, Nombre, Correo) VALUES
(1, 'Ana González', 'ana.gonzalez@email.com'),
(2, 'Luis Martínez', 'luis.martinez@email.com'),
(3, 'Carlos Ruiz', 'carlos.ruiz@email.com');

-- Insertar rutas
INSERT INTO Rutas (RutaID, Origen, Destino) VALUES
(1, 'Ciudad de Guatemala', 'San Salvador'),
(2, 'Ciudad de Guatemala', 'Tegucigalpa'),
(3, 'San Salvador', 'Managua');

-- Insertar vuelos
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora) VALUES
(101, 'Ciudad de Guatemala', 'San Salvador', '2025-05-15', '08:00'),
(102, 'Ciudad de Guatemala', 'Tegucigalpa', '2025-05-15', '10:30'),
(103, 'San Salvador', 'Managua', '2025-05-16', '14:00');

-- Insertar asientos
INSERT INTO Asientos (AsientoID, VueloID, Clase, Precio) VALUES
(1, 101, 'Económica', 150.00),
(2, 101, 'Ejecutiva', 300.00),
(3, 102, 'Económica', 170.00),
(4, 103, 'Económica', 200.00);

-- Insertar boletos
INSERT INTO Boletos (BoletoID, ClienteID, AsientoID, FechaCompra) VALUES
(1001, 1, 1, '2025-05-10'),
(1002, 2, 2, '2025-05-10'),
(1003, 3, 3, '2025-05-11');

-- Insertar alimentos
INSERT INTO Alimentos (AlimentoID, Nombre, Precio) VALUES
(1, 'Sandwich', 5.00),
(2, 'Jugo de Naranja', 2.50),
(3, 'Agua Embotellada', 1.00);

-- Relacionar alimentos con vuelos
INSERT INTO VuelosAlimentos (VueloID, AlimentoID) VALUES
(101, 1),
(101, 3),
(102, 2),
(103, 1),
(103, 2),
(103, 3);

-- Insertar pilotos
INSERT INTO Pilotos (PilotoID, Nombre, Licencia) VALUES
(1, 'Miguel Pérez', 'LIC-P001'),
(2, 'Julia Sánchez', 'LIC-P002');

-- Insertar aeromosas
INSERT INTO Aeromosas (AeromosaID, Nombre, VueloID) VALUES
(1, 'Laura Ramírez', 101),
(2, 'Paola Díaz', 102),
(3, 'Verónica López', 103);
