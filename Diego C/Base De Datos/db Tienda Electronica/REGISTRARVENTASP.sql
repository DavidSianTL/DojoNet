USE [TiendaElectronica]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegistrarVenta]
    @IdUsuario INT,
    @IdProducto INT,
    @Cantidad INT
AS
BEGIN
  
       
        INSERT INTO Ventas (IdUsuario)
        VALUES (@IdUsuario);

        DECLARE @IdVenta INT = SCOPE_IDENTITY();

        
        DECLARE @Precio DECIMAL(10, 2);
        SELECT @Precio = Precio FROM Productos WHERE IdProducto = @IdProducto;

        INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario)
        VALUES (@IdVenta, @IdProducto, @Cantidad, @Precio);

        UPDATE Productos
        SET Stock = Stock - @Cantidad
        WHERE IdProducto = @IdProducto;
  
END;
