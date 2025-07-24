Use DBProyectoGrupalDojoGeko
GO


CREATE TABLE DiasFestivosVariables (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL UNIQUE,
    Descripcion NVARCHAR(100),
    TipoFeriadoId INT NOT NULL,
    ProporcionDia DECIMAL(3,2) NOT NULL DEFAULT 1.00,
    Usr_creacion NVARCHAR(25) NOT NULL,
    Fec_creacion DATETIME NOT NULL,
    Usr_modifica NVARCHAR(25),
    Fec_modifica DATETIME,
    CONSTRAINT FK_DiasFestivosVariables_TipoFeriado 
        FOREIGN KEY (TipoFeriadoId) REFERENCES TipoFeriado (TipoFeriadoId)
);