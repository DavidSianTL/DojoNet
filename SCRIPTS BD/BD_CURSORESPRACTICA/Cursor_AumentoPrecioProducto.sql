DECLARE @id_producto INT, 
        @nombre VARCHAR(100), 
        @tipo VARCHAR(40), 
        @porcentaje DECIMAL(5,2) = 12.00;

DECLARE cursor_aumento_precio CURSOR FOR
SELECT Id_Producto, Nombre, Tipo 
FROM Productos
WHERE Tipo = 'Importado'; 

OPEN cursor_aumento_precio;

FETCH NEXT FROM cursor_aumento_precio INTO @id_producto, @nombre, @tipo;

WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC SP_Aumento_Precio_Tipo_Producto
        @p_id_producto = @id_producto,
        @p_nombre = @nombre,
        @p_tipo_producto = @tipo,
        @p_porcentaje = @porcentaje;

    FETCH NEXT FROM cursor_aumento_precio INTO @id_producto, @nombre, @tipo;
END

CLOSE cursor_aumento_precio;
DEALLOCATE cursor_aumento_precio;
