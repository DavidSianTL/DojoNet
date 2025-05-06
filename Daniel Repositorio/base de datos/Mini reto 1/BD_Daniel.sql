CREATE DATABASE TiendaElectronica;
GO

USE TiendaElectronica;
GO

CREATE TABLE Usuarios (
UsuarioID INT PRIMARY KEY IDENTITY(1,1),
Nombre VARCHAR(30),
Correo VARCHAR(30),
Contrasena VARCHAR(50),
);

CREATE TABLE Categorias (
CategoriaID INT PRIMARY KEY IDENTITY(1,1),
Nombre VARCHAR(20),
Descripcion VARCHAR(50)
);

CREATE TABLE Proveedores (
ProveedorID INT PRIMARY KEY IDENTITY(1,1),
Nombre VARCHAR(30),
Telefono VARCHAR(20),
Direccion VARCHAR(50)
);

CREATE TABLE Productos (
ProductoID INT PRIMARY KEY IDENTITY(1,1),
Nombre NVARCHAR(30),
Descripcion NVARCHAR(50),
Precio DECIMAL(10,2),
Stock INT,
CategoriaID INT,
ProveedorID INT,
FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID),
FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);

CREATE TABLE Ventas (
VentaID INT PRIMARY KEY IDENTITY(1,1),
UsuarioID INT,
FechaVenta DATETIME DEFAULT GETDATE(),
Total DECIMAL(10,2),
FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

CREATE TABLE DetalleVenta (
 DetalleID INT PRIMARY KEY IDENTITY(1,1),
 VentaID INT,
 ProductoID INT,
 Cantidad INT,
 PrecioUnitario DECIMAL(10,2),
 Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
 FOREIGN KEY (VentaID) REFERENCES Ventas(VentaID),
 FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);


---- tablas con insertar datos

-- Usuarios
INSERT INTO Usuarios (Nombre, Correo, Contrasena)
VALUES 
('Daniel Roblero', 'DanRo@gmail.com', '1234'),
('Jack Daniel', 'JackDan@gmail.com', 'abcd'),
('Santa Claus', 'SaCla@gmail.com', 'qwerty');

-- Categorias
INSERT INTO Categorias (Nombre, Descripcion)
VALUES 
('Laptops', 'Computadoras portátiles'),
('Smartphones', 'Teléfonos inteligentes'),
('Accesorios', 'Cargadores, fundas, etc.');

-- Proveedores
INSERT INTO Proveedores (Nombre, Telefono, Direccion)
VALUES 
('Samsung', '555-1234', 'Av. Tecnología 101'),
('HP', '555-5678', 'Calle Movil 202'),
('Lenovo', '555-9876', 'Plaza Central 303');

-- Productos
INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaID, ProveedorID)
VALUES 
('Laptop HP', 'HP Pavilion 15"', 750.00, 10, 1, 1),
('Lapto Lenovo', 'Lenovo Thinkbook', 999.99, 4, 2, 2),
('Galaxy Watch7', 'Galaxy watch7 bluetooth 44mm', 19.99, 20, 3, 3);

-- Ventas
INSERT INTO Ventas (UsuarioID, Total)
VALUES 
(1, 769.99),
(2, 999.99),
(3, 19.99);

-- DetalleVenta
INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
VALUES 
(1, 3, 1, 19.99), 
(1, 1, 1, 750.00), 
(2, 2, 1, 999.99), 
(3, 3, 1, 19.99);  


