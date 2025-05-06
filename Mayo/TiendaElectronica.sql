

CREATE DATABASE TiendaElectronica;
USE TiendaElectronica;

CREATE TABLE Usuarios (
    UsuarioID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100),
    Email VARCHAR(100)
);

CREATE TABLE Categorias (
    CategoriaID INT AUTO_INCREMENT PRIMARY KEY,
    NombreCategoria VARCHAR(100)
);

CREATE TABLE Proveedores (
    ProveedorID INT AUTO_INCREMENT PRIMARY KEY,
    NombreProveedor VARCHAR(100),
    Telefono VARCHAR(20)
);

CREATE TABLE Productos (
    ProductoID INT AUTO_INCREMENT PRIMARY KEY,
    NombreProducto VARCHAR(100),
    Precio DECIMAL(10,2),
    Stock INT,
    CategoriaID INT,
    ProveedorID INT,
    FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID),
    FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);

CREATE TABLE Ventas (
    VentaID INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioID INT,
    FechaVenta DATE,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

CREATE TABLE DetalleVenta (
    DetalleID INT AUTO_INCREMENT PRIMARY KEY,
    VentaID INT,
    ProductoID INT,
    Cantidad INT,
    PrecioUnitario DECIMAL(10,2),
    FOREIGN KEY (VentaID) REFERENCES Ventas(VentaID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);

INSERT INTO Usuarios (Nombre, Email) VALUES
('Pablo', 'Pablo@gmail.com'),
('Torres', 'Torres@gmail.com'),
('Leonel', 'Leonel@gmail.com');

INSERT INTO Categorias (NombreCategoria) VALUES
('Electrónica'),
('Hogar'),
('Juguetes');

INSERT INTO Proveedores (NombreProveedor, Telefono) VALUES
('TechPro S.A.', '111-222-333'),
('Casa&Más', '444-555-666'),
('Juegos SRL', '777-888-999');

INSERT INTO Productos (NombreProducto, Precio, Stock, CategoriaID, ProveedorID) VALUES
('Smartphone X', 350.00, 10, 1, 1),
('Licuadora Max', 75.00, 4, 2, 2),
('Robot Interactivo', 120.00, 2, 3, 3);

INSERT INTO Ventas (UsuarioID, FechaVenta) VALUES
(1, '2025-05-01'),
(2, '2025-05-02'),
(3, '2025-05-03');

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario) VALUES
(1, 1, 1, 350.00),
(2, 2, 2, 75.00),
(3, 3, 1, 120.00);

-- Consultas

SELECT p.NombreProducto, c.NombreCategoria, pr.NombreProveedor
FROM Productos p
JOIN Categorias c ON p.CategoriaID = c.CategoriaID
JOIN Proveedores pr ON p.ProveedorID = pr.ProveedorID;

SELECT v.VentaID, u.Nombre, v.FechaVenta,
       SUM(dv.Cantidad * dv.PrecioUnitario) AS TotalVendido
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
GROUP BY v.VentaID, u.Nombre, v.FechaVenta;

SELECT NombreProducto, Stock
FROM Productos
WHERE Stock < 5;

SELECT p.NombreProducto, SUM(dv.Cantidad * dv.PrecioUnitario) AS TotalVendido
FROM DetalleVenta dv
JOIN Productos p ON dv.ProductoID = p.ProductoID
GROUP BY p.NombreProducto;

CREATE VIEW Vista_VentasResumen AS
SELECT u.Nombre AS Usuario, v.FechaVenta,
       SUM(dv.Cantidad) AS CantidadProductos,
       SUM(dv.Cantidad * dv.PrecioUnitario) AS TotalVenta
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
GROUP BY v.VentaID, u.Nombre, v.FechaVenta;

DELIMITER //
CREATE PROCEDURE RegistrarVenta(
    IN pUsuarioID INT,
    IN pFecha DATE,
    IN pProductoID INT,
    IN pCantidad INT,
    IN pPrecioUnitario DECIMAL(10,2)
)
BEGIN
    DECLARE vID INT;

    INSERT INTO Ventas (UsuarioID, FechaVenta) VALUES (pUsuarioID, pFecha);
    SET vID = LAST_INSERT_ID();

    INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
    VALUES (vID, pProductoID, pCantidad, pPrecioUnitario);

    UPDATE Productos
    SET Stock = Stock - pCantidad
    WHERE ProductoID = pProductoID;
END;
//
DELIMITER ;
