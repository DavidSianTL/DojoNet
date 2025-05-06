
USE TiendaElectronica;
GO

CREATE PROCEDURE RegistrarVenta
    @UsuarioID INT,
    @ProductoID INT,
    @Cantidad INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @Precio DECIMAL(10,2);
        
        
        SELECT @Precio = Precio FROM Productos WHERE ProductoID = @ProductoID;
        
        
        INSERT INTO Ventas (UsuarioID, Total)
        VALUES (@UsuarioID, @Cantidad * @Precio);
        
        
        INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
        VALUES (SCOPE_IDENTITY(), @ProductoID, @Cantidad, @Precio);
        
        
        UPDATE Productos SET Stock = Stock - @Cantidad WHERE ProductoID = @ProductoID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Llamar al SP
EXEC RegistrarVenta 
    @UsuarioID = 1, 
    @ProductoID = 1, 
    @Cantidad = 2;
GO