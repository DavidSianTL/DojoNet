Use DBProyectoGrupalDojoGeko
GO

CREATE TABLE TipoFeriado (
    TipoFeriadoId INT PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,         -- Ej: 'Nacional', 'Bancario', 'Religioso'
    Descripcion NVARCHAR(200),
    Usr_creacion NVARCHAR(25) NOT NULL,
    Fec_creacion DATETIME NOT NULL,
    Usr_modifica NVARCHAR(25),
    Fec_modifica DATETIME
);
/*Ejemplo*/
INSERT INTO TipoFeriado (TipoFeriadoId, Nombre, Descripcion, Usr_creacion, Fec_creacion)
VALUES 
(1, 'Nacional', 'Aplica a todo el país', 'admin', GETDATE()),
(2, 'Bancario', 'Solo para entidades financieras', 'admin', GETDATE()),
(3, 'Religioso', 'Celebraciones religiosas no obligatorias', 'admin', GETDATE());