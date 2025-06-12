
USE AeroDB;
GO
-- Nuevos vuelos con destino a Guatemala
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora) VALUES
(104, 'San Salvador', 'Ciudad de Guatemala', '2025-05-17', '09:00'),
(105, 'Tegucigalpa', 'Ciudad de Guatemala', '2025-05-18', '11:45');

-- Asientos para estos vuelos
INSERT INTO Asientos (AsientoID, VueloID, Clase, Precio) VALUES
(5, 104, 'Económica', 130.00),
(6, 104, 'Ejecutiva', 250.00),
(7, 105, 'Económica', 140.00);

-- Nuevos boletos vendidos para estos vuelos
INSERT INTO Boletos (BoletoID, ClienteID, AsientoID, FechaCompra) VALUES
(1004, 1, 5, '2025-05-12'),
(1005, 2, 6, '2025-05-13'),
(1006, 3, 7, '2025-05-13');

-- Alimentos disponibles en estos vuelos
INSERT INTO VuelosAlimentos (VueloID, AlimentoID) VALUES
(104, 1),
(104, 2),
(105, 3);

-- Nuevos pilotos asignados si se desea alternar
INSERT INTO Pilotos (PilotoID, Nombre, Licencia) VALUES
(3, 'Ernesto Ramírez', 'LIC-P003');

-- Aeromosas para los nuevos vuelos
INSERT INTO Aeromosas (AeromosaID, Nombre, VueloID) VALUES
(4, 'Gabriela Castillo', 104),
(5, 'Mónica Herrera', 105);
