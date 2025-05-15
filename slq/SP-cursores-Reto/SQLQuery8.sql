CREATE PROCEDURE sp_aumento_precio_tipo_producto
AS
BEGIN
    UPDATE productos
    SET Costo = 
        CASE Tipo
            WHEN 'importados' THEN Costo * 1.44
            WHEN 'nacionales' THEN Costo * 1.12
            ELSE Costo
        END;

    INSERT INTO logs (Mensaje, FechaLog)
    VALUES ('Precios actualizados según tipo de producto', GETDATE());
END;
