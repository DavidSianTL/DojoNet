USE TiendaElectronica;

-- Insertar Usuarios
INSERT INTO Usuarios (Nombre, Email, Password) VALUES
('Carlos García', 'carlos@example.com', '123456'),
('Ana López', 'ana@example.com', 'abcdef'),
('Luis Pérez', 'luis@example.com', 'qwerty');

-- Insertar Categorías
INSERT INTO Categorias (Nombre) VALUES
('Electrónica'),
('Computadoras'),
('Accesorios');

-- Insertar Proveedores
INSERT INTO Proveedores (Nombre, Contacto) VALUES
('TechSupplier', 'contacto@techsupplier.com'),
('PCParts', 'ventas@pcparts.com'),
('GadgetWorld', 'info@gadgetworld.com');

-- Insertar Productos
INSERT INTO Productos (Nombre, Precio, Stock, CategoriaID, ProveedorID) VALUES
('Laptop Gamer', 1200.00, 10, 2, 2),
('Smartphone X', 800.00, 5, 1, 3),
('Mouse Inalámbrico', 50.00, 15, 3, 1);

-- Insertar Ventas
INSERT INTO Ventas (UsuarioID, FechaVenta) VALUES
(1, '2025-05-01 14:30:00'),
(2, '2025-05-02 10:15:00'),
(3, '2025-05-03 16:45:00');

-- Insertar Detalle de Ventas
INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, Total) VALUES
(1, 1, 1, 1200.00),
(2, 2, 1, 800.00),
(3, 3, 2, 100.00);

-- Consultas generales de la base de datos
SELECT * FROM Usuarios;
SELECT * FROM Categorias;
SELECT * FROM Proveedores;
SELECT * FROM Productos;
SELECT * FROM Ventas;
SELECT * FROM DetalleVenta;

-- Productos con menos de 5 unidades en stock
SELECT Nombre, Stock FROM Productos WHERE Stock < 5;

-- Total vendido por cada producto
SELECT p.Nombre, SUM(dv.Total) AS TotalVendido
FROM DetalleVenta dv
JOIN Productos p ON dv.ProductoID = p.ProductoID
GROUP BY p.Nombre;

-- Historial de ventas con usuario y total
SELECT v.VentaID, u.Nombre AS Usuario, SUM(dv.Total) AS TotalVendido
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
GROUP BY v.VentaID, u.Nombre;

-- Vista resumen de ventas
CREATE VIEW Vista_VentasResumen AS
SELECT u.Nombre AS Usuario, v.FechaVenta, COUNT(dv.ProductoID) AS CantidadProductos, SUM(dv.Total) AS TotalVenta
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
GROUP BY u.Nombre, v.FechaVenta;
