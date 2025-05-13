--- B. Procedimientos Almacenados (Productos)(SP):

---1. sp_ingresar_producto:
CREATE PROCEDURE sp_ingresar_producto
    @Nombre NVARCHAR(25),
    @Costo DECIMAL(10,2),
    @Tipo NVARCHAR(25),
    @Stock INT,
    @FechaRegistro DATE
AS
BEGIN
    INSERT INTO productos (Nombre, Costo, Tipo, Stock, FechaRegistro)
    VALUES (@Nombre, @Costo, @Tipo, @Stock, @FechaRegistro);

    DECLARE @mensaje NVARCHAR(100)
    SET @mensaje = 'Producto "' + @Nombre + '" ingresado correctamente.'

    INSERT INTO logs (Mensaje) VALUES (@mensaje)
END;

--- prueba de ejecucion:
EXEC sp_ingresar_producto 
    @Nombre = 'Audifonos Razer',
    @Costo = 3500.00,
    @Tipo = 'Importados',
    @Stock = 4,
    @FechaRegistro = '2025-05-13'; 


-- Verificar en logs
SELECT * FROM logs --WHERE Mensaje LIKE '%Audifonos Razer%';





---------2. sp_notificar_stock_bajo

CREATE PROCEDURE sp_notificar_stock_bajo
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(25), @Stock INT, @mensaje NVARCHAR(100)

    DECLARE cursor_stock CURSOR FAST_FORWARD FOR
        SELECT Id, Nombre, Stock FROM productos WHERE Stock < 5

    OPEN cursor_stock
    FETCH NEXT FROM cursor_stock INTO @Id, @Nombre, @Stock

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @mensaje = 'Alerta: Stock bajo de "' + @Nombre + '" (ID: ' + CAST(@Id AS NVARCHAR) + ')'
        INSERT INTO logs (Mensaje) VALUES (@mensaje)

        FETCH NEXT FROM cursor_stock INTO @Id, @Nombre, @Stock
    END

    CLOSE cursor_stock
    DEALLOCATE cursor_stock
END;


----- prueba
-- Verifica primero que haya productos con stock bajo:
SELECT * FROM productos WHERE Stock < 5;

EXEC sp_notificar_stock_bajo;

SELECT * FROM logs WHERE Mensaje LIKE 'Alerta: Stock bajo%';





----3. sp_aumento_precio_tipo_producto
CREATE PROCEDURE sp_aumento_precio_tipo_producto
AS
BEGIN
    DECLARE @Id INT, @Nombre NVARCHAR(25), @Tipo NVARCHAR(25), @Costo DECIMAL(10,2)
    DECLARE @nuevoCosto DECIMAL(10,2), @mensaje NVARCHAR(100), @porcentaje DECIMAL(5,2)

    DECLARE cursor_precio CURSOR FAST_FORWARD FOR
        SELECT Id, Nombre, Tipo, Costo FROM productos

    OPEN cursor_precio
    FETCH NEXT FROM cursor_precio INTO @Id, @Nombre, @Tipo, @Costo

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @porcentaje = 
            CASE 
                WHEN LOWER(@Tipo) = 'importados' THEN 0.44
                WHEN LOWER(@Tipo) = 'nacionales' THEN 0.12
                ELSE 0
            END

        SET @nuevoCosto = @Costo + (@Costo * @porcentaje)

        IF @porcentaje > 0
        BEGIN
            UPDATE productos SET Costo = @nuevoCosto WHERE Id = @Id

            SET @mensaje = 'Aumento aplicado al producto "' + @Nombre + '" de tipo ' + @Tipo
            INSERT INTO logs (Mensaje) VALUES (@mensaje)
        END

        FETCH NEXT FROM cursor_precio INTO @Id, @Nombre, @Tipo, @Costo
    END

    CLOSE cursor_precio
    DEALLOCATE cursor_precio
END;


--prueba
--verificacion de productos antes de cambiar.
SELECT * FROM productos;

EXEC sp_aumento_precio_tipo_producto;

-- Ver productos con la actualizacion.
SELECT * FROM productos;

-- Ver logs
SELECT * FROM logs 

