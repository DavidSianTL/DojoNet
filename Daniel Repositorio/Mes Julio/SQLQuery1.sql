-- Selecciona la base de datos
USE AutoExpressDB;
GO

-- Tabla Paises
INSERT INTO Paises (nombre, codigo) VALUES 
('Guatemala', 'GT'),
('M�xico', 'MX'),
('El Salvador', 'SV');

-- Tabla TipoVehiculo
INSERT INTO TipoVehiculo (tipo, descripcion) VALUES 
('Sed�n', 'Veh�culo compacto para uso familiar'),
('SUV', 'Veh�culo utilitario deportivo'),
('Pick-up', 'Camioneta de carga ligera');

-- Tabla Estados
INSERT INTO Estados (estado, descripcion) VALUES 
('Disponible', 'Veh�culo est� disponible para la venta'),
('Vendido', 'Veh�culo ya ha sido vendido'),
('No disponible', 'Veh�culo no disponible por estos momentos');


SELECT * FROM Paises;	
SELECT * FROM TipoVehiculo;
SELECT * FROM Estados;
