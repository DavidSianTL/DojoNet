

CREATE DATABASE TiendaElectronica;
GO

USE TiendaElectronica;
GO

CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Categorias (
    IdCategoria INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Proveedores (
    IdProveedor INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Productos (
    IdProducto INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100) NOT NULL,
    IdCategoria INT FOREIGN KEY REFERENCES Categorias(IdCategoria),
    IdProveedor INT FOREIGN KEY REFERENCES Proveedores(IdProveedor),
    Precio DECIMAL(10,2),
    Stock INT
);

CREATE TABLE Ventas (
    IdVenta INT PRIMARY KEY IDENTITY,
    IdUsuario INT FOREIGN KEY REFERENCES Usuarios(IdUsuario),
    FechaVenta DATETIME DEFAULT GETDATE()
);

CREATE TABLE DetalleVenta (
    IdDetalleVenta INT PRIMARY KEY IDENTITY,
    IdVenta INT FOREIGN KEY REFERENCES Ventas(IdVenta),
    IdProducto INT FOREIGN KEY REFERENCES Productos(IdProducto),
    Cantidad INT,
    PrecioUnitario DECIMAL(10,2)
);


INSERT INTO Usuarios (Nombre) VALUES ('Karina'), ('Daniel'), ('Diego');
INSERT INTO Categorias (Nombre) VALUES ('Smartphones'), ('Laptops'), ('Accesorios');
INSERT INTO Proveedores (Nombre) VALUES ('Samsung'), ('Dell'), ('Logitech');

INSERT INTO Productos (Nombre, IdCategoria, IdProveedor, Precio, Stock)
VALUES 
('Galaxy S22', 1, 1, 800.00, 10),
('Latitude 3280', 2, 2, 1200.00, 3),
('Mouse', 3, 3, 50.00, 15);

INSERT INTO Ventas (IdUsuario, FechaVenta)
VALUES (1, '2024-05-01'), (2, '2024-05-02'), (3, '2024-05-03');

INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
VALUES 
(1, 1, 1, 800.00),
(2, 2, 1, 1200.00),
(3, 3, 2, 50.00);



SELECT P.Nombre AS Producto, C.Nombre AS Categoria, PR.Nombre AS Proveedor
FROM Productos P
JOIN Categorias C ON P.IdCategoria = C.IdCategoria
JOIN Proveedores PR ON P.IdProveedor = PR.IdProveedor;


SELECT V.IdVenta, U.Nombre AS Usuario, V.FechaVenta,
       SUM(DV.Cantidad * DV.PrecioUnitario) AS TotalVendido
FROM Ventas V
JOIN Usuarios U ON V.IdUsuario = U.IdUsuario
JOIN DetalleVenta DV ON V.IdVenta = DV.IdVenta
GROUP BY V.IdVenta, U.Nombre, V.FechaVenta;


SELECT Nombre, Stock
FROM Productos
WHERE Stock < 5;


SELECT P.Nombre, SUM(DV.Cantidad) AS TotalUnidadesVendidas, 
       SUM(DV.Cantidad * DV.PrecioUnitario) AS TotalVendido
FROM DetalleVenta DV
JOIN Productos P ON DV.IdProducto = P.IdProducto
GROUP BY P.Nombre;


CREATE VIEW Vista_VentasResumen AS

SELECT 
    U.Nombre AS Usuario,
    V.FechaVenta,
    SUM(DV.Cantidad) AS CantidadProductos,
    SUM(DV.Cantidad * DV.PrecioUnitario) AS TotalVenta
FROM Ventas V
JOIN Usuarios U ON V.IdUsuario = U.IdUsuario
JOIN DetalleVenta DV ON V.IdVenta = DV.IdVenta
GROUP BY U.Nombre, V.FechaVenta;
