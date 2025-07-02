-- 1. Crear la base de datos
CREATE DATABASE AutoExpressDB;
GO

-- 2. Usar la base de datos
USE AutoExpressDB;
GO

-- 3. Crear tabla 'carros' con campos en minúscula
CREATE TABLE carros (
    id INT IDENTITY(1,1) PRIMARY KEY,
    marca NVARCHAR(100) NOT NULL,
    modelo NVARCHAR(100) NOT NULL,
    anio INT NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    disponible BIT NOT NULL DEFAULT 1
);
GO

-- 4. SP: Obtener todos los carros (solo disponibles)
CREATE PROCEDURE sp_GetCarros
AS
BEGIN
    SELECT * FROM carros WHERE disponible = 1;
END
GO

-- 5. SP: Obtener un carro por ID
CREATE PROCEDURE sp_GetCarroById
    @id INT
AS
BEGIN
    SELECT * FROM carros WHERE id = @id;
END
GO

-- 6. SP: Insertar carro
CREATE PROCEDURE sp_InsertCarro
    @marca NVARCHAR(100),
    @modelo NVARCHAR(100),
    @anio INT,
    @precio DECIMAL(10,2)
AS
BEGIN
    INSERT INTO carros (marca, modelo, anio, precio, disponible)
    VALUES (@marca, @modelo, @anio, @precio, 1);
END
GO

-- 7. SP: Actualizar carro
CREATE PROCEDURE sp_UpdateCarro
    @id INT,
    @marca NVARCHAR(100),
    @modelo NVARCHAR(100),
    @anio INT,
    @precio DECIMAL(10,2),
    @disponible BIT
AS
BEGIN
    UPDATE carros
    SET marca = @marca,
        modelo = @modelo,
        anio = @anio,
        precio = @precio,
        disponible = @disponible
    WHERE id = @id;
END
GO

-- 8. SP: Eliminación lógica de carro (soft delete)
CREATE PROCEDURE sp_DeleteCarro
    @id INT
AS
BEGIN
    UPDATE carros
    SET disponible = 0
    WHERE id = @id;
END
GO
