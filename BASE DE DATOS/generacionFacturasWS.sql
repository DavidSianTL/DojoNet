CREATE TABLE FacturasPendientes (
    Id INT PRIMARY KEY,
    Cliente NVARCHAR(100),
    Fecha DATETIME,
    Estado NVARCHAR(20),
    RutaPDF NVARCHAR(200)
);
GO
CREATE TABLE DetalleFactura (
    IdFactura INT,
    Producto NVARCHAR(100),
    Cantidad INT,
    PrecioUnitario DECIMAL(10, 2)
);
GO


INSERT INTO FacturasPendientes (Id, Cliente, Fecha, Estado, RutaPDF)
VALUES
(1, 'Cliente A', '2025-07-07', 'Pendiente', NULL),
(2, 'Cliente B', '2025-06-30', 'Facturada', 'C:\Facturas\Factura_2.pdf'),
(3, 'Cliente C', '2025-07-01', 'Pendiente', NULL);

INSERT INTO DetalleFactura (IdFactura, Producto, Cantidad, PrecioUnitario)
VALUES
(1, 'Producto X', 2, 15.00),
(1, 'Producto Y', 1, 20.00),
(3, 'Producto Z', 5, 10.00);

INSERT INTO DetalleFactura (IdFactura, Producto, Cantidad, PrecioUnitario)
VALUES
(2, 'Producto A', 2, 15.00),
(2, 'Producto B', 1, 20.00),
(2, 'Producto C', 5, 10.00);


select * from FacturasPendientes
select * from DetalleFactura

USE [EmpresaDB]
GO

UPDATE [dbo].[FacturasPendientes]
   SET [Estado] = 'Pendiente'
      ,[RutaPDF] = ''
	where Id = 1

GO