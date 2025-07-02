-- Selecciona la base de datos
USE AutoExpressDB;
GO

-- Tabla Paises
INSERT INTO Paises (nombre, codigo) VALUES 
('Guatemala', 'GT'),
('México', 'MX'),
('El Salvador', 'SV');

-- Tabla TipoVehiculo
INSERT INTO TipoVehiculo (tipo, descripcion) VALUES 
('Sedán', 'Vehículo compacto para uso familiar'),
('SUV', 'Vehículo utilitario deportivo'),
('Pick-up', 'Camioneta de carga ligera');

-- Tabla Estados
INSERT INTO Estados (estado, descripcion) VALUES 
('Disponible', 'Vehículo está disponible para la venta'),
('Vendido', 'Vehículo ya ha sido vendido'),
('No disponible', 'Vehículo no disponible por estos momentos');


SELECT * FROM Paises;	
SELECT * FROM TipoVehiculo;
SELECT * FROM Estados;
