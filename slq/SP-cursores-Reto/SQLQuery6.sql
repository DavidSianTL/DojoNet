CREATE PROCEDURE sp_ingresar_producto
    @Nombre NVARCHAR(100),
    @Costo DECIMAL(10,2),
    @Tipo NVARCHAR(50),
    @Stock INT,
    @FechaRegistro DATE
AS
BEGIN
    INSERT INTO productos (Nombre, Costo, Tipo, Stock, FechaRegistro)
    VALUES (@Nombre, @Costo, @Tipo, @Stock, @FechaRegistro);

    INSERT INTO logs (Mensaje, FechaLog)
    VALUES ('Producto ingresado: ' + @Nombre, GETDATE());
END;
