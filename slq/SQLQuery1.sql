
CREATE DATABASE TiendaElectronica;
USE TiendaElectronica;


CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Telefono VARCHAR(15),
    Rol VARCHAR(20) DEFAULT 'Cliente' 
);


CREATE TABLE Categorias (
    CategoriaID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion TEXT
);


CREATE TABLE Proveedores (
    ProveedorID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Contacto VARCHAR(50),
    Telefono VARCHAR(15),
    Direccion TEXT
);


CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion TEXT,
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT DEFAULT 0,
    CategoriaID INT FOREIGN KEY REFERENCES Categorias(CategoriaID),
    ProveedorID INT FOREIGN KEY REFERENCES Proveedores(ProveedorID)
);


CREATE TABLE Ventas (
    VentaID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT FOREIGN KEY REFERENCES Usuarios(UsuarioID),
    FechaVenta DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10, 2)
);


CREATE TABLE DetalleVenta (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    VentaID INT FOREIGN KEY REFERENCES Ventas(VentaID),
    ProductoID INT FOREIGN KEY REFERENCES Productos(ProductoID),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario)
);



INSERT INTO Usuarios (Nombre, Email, Telefono, Rol) VALUES 
('Carlos Fernando', 'carlos@ejemplo.com', '123456789', 'Administrador'),
('Daniel Torres', 'daniel@ejemplo.com', '987654321', 'Cliente'),
('Diego Jose', 'diego@ejemplo.com', '555555555', 'Cliente');

INSERT INTO Categorias (Nombre, Descripcion) VALUES 
('Electrónica', 'Dispositivos electrónicos'),
('Computación', 'Laptops, PCs y accesorios'),
('Hogar', 'Electrodomésticos');

INSERT INTO Proveedores (Nombre, Contacto, Telefono, Direccion) VALUES 
('TechSupply', 'Pedro Gómez', '111222333', 'Calle Falsa 123'),
('ElectroParts', 'Luisa Fernández', '444555666', 'Avenida Siempre Viva 456'),
('GlobalTech', 'Ana Martínez', '777888999', 'Boulevard Central 789');


INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaID, ProveedorID) VALUES 
('Laptop HP', 'Laptop de 15 pulgadas, 8GB RAM', 1200.00, 10, 2, 1),
('Smartphone Samsung', 'Teléfono Android, 128GB', 800.00, 15, 1, 2),
('Licuadora Oster', '600W, vaso de vidrio', 50.00, 5, 3, 3);


INSERT INTO Ventas (UsuarioID, Total) VALUES 
(2, 800.00),
(3, 1250.00);

INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario) VALUES 
(1, 2, 1, 800.00),
(2, 1, 1, 1200.00),
(2, 3, 1, 50.00);


---Listar productos con categoría y proveedor
SELECT p.Nombre AS Producto, c.Nombre AS Categoria, pr.Nombre AS Proveedor
FROM Productos p
JOIN Categorias c ON p.CategoriaID = c.CategoriaID
JOIN Proveedores pr ON p.ProveedorID = pr.ProveedorID;



----Historial de ventas con usuario y total
SELECT v.VentaID, u.Nombre AS Cliente, v.FechaVenta, v.Total
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID;


--- Productos con menos de 5 unidades en stock
SELECT Nombre, Stock 
FROM Productos
WHERE Stock < 5;

---Total vendido por cada producto
SELECT p.Nombre, SUM(dv.Cantidad * dv.PrecioUnitario) AS TotalVendido
FROM DetalleVenta dv
JOIN Productos p ON dv.ProductoID = p.ProductoID
GROUP BY p.ProductoID;





CREATE VIEW Vista_VentasResumen AS
SELECT 
    u.Nombre AS Usuario,
    v.FechaVenta,
    COUNT(dv.DetalleID) AS CantidadProductos,
    SUM(dv.Subtotal) AS TotalVenta
FROM Ventas v
JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
GROUP BY v.VentaID, u.Nombre, v.FechaVenta;


SELECT * FROM Vista_VentasResumen; 