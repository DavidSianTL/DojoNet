USE AeroDB;
GO

-- PASO 1: Agregar columna PilotoID a la tabla Vuelos
ALTER TABLE Vuelos
ADD PilotoID INT;

USE AeroDB;
GO
-- PASO 2: Establecer la clave foránea
ALTER TABLE Vuelos
ADD CONSTRAINT FK_Vuelos_Pilotos
FOREIGN KEY (PilotoID) REFERENCES Pilotos(PilotoID);

USE AeroDB;
GO
-- PASO 3: Agregar más vuelos para simular carga
-- Suponemos 3 pilotos existentes: IDs 1, 2 y 3
-- El piloto 1 tendrá 11 vuelos para cumplir la condición del ejemplo

-- Vuelos adicionales del Piloto 1
INSERT INTO Vuelos (VueloID, Origen, Destino, Fecha, Hora, PilotoID) VALUES
(201, 'Ciudad de Guatemala', 'San Salvador', '2025-06-01', '08:00', 1),
(202, 'Ciudad de Guatemala', 'Tegucigalpa', '2025-06-02', '08:30', 1),
(203, 'San Salvador', 'Ciudad de Guatemala', '2025-06-03', '09:00', 1),
(204, 'Ciudad de Guatemala', 'Managua', '2025-06-04', '10:00', 1),
(205, 'Tegucigalpa', 'Ciudad de Guatemala', '2025-06-05', '11:00', 1),
(206, 'Ciudad de Guatemala', 'Belice', '2025-06-06', '12:00', 1),
(207, 'Ciudad de Guatemala', 'San Pedro Sula', '2025-06-07', '13:00', 1),
(208, 'Ciudad de Guatemala', 'Panamá', '2025-06-08', '14:00', 1),
(209, 'Ciudad de Guatemala', 'México', '2025-06-09', '15:00', 1),
(210, 'Ciudad de Guatemala', 'San José', '2025-06-10', '16:00', 1);

-- Asignar vuelos existentes a los pilotos
UPDATE Vuelos SET PilotoID = 1 WHERE VueloID IN (101, 104);
UPDATE Vuelos SET PilotoID = 2 WHERE VueloID IN (102, 105);
UPDATE Vuelos SET PilotoID = 3 WHERE VueloID = 103;
