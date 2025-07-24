Use DBProyectoGrupalDojoGeko
GO

CREATE TABLE DiasFestivosFijos (
    Dia INT NOT NULL CHECK (Dia BETWEEN 1 AND 31),
    Mes INT NOT NULL CHECK (Mes BETWEEN 1 AND 12),
    Descripcion NVARCHAR(100),
	TipoFeriadoId INT NOT NULL,
	ProporcionDia DECIMAL(3,2) NOT NULL DEFAULT 1.00,
	Usr_creacion NVARCHAR(25) NOT NULL,
	Fec_creacion DATETIME NOT NULL,
	Usr_modifica NVARCHAR(25),
	Fec_modifica DATETIME,
    CONSTRAINT PK_DiasFestivosFijos PRIMARY KEY (Dia, Mes, TipoFeriadoId),
    CONSTRAINT FK_DiasFestivosFijos_TipoFeriado 
        FOREIGN KEY (TipoFeriadoId) REFERENCES TipoFeriado (TipoFeriadoId)
);


/*EJEMPLO DE INSERT*/
INSERT INTO DiasFestivosFijos (Dia, Mes, TipoFeriadoId,ProporcionDia, Descripcion,Usr_creacion, Fec_creacion) VALUES
(1, 1, 1, 1.0, 'Año Nuevo','admin',GETDATE()),
(1, 5, 1, 1.0, 'Día del Trabajo','admin',GETDATE()),
(30, 6, 1, 1.0, 'Dia del Ejercito','admin',GETDATE()),
(1, 7, 2, 1.0, 'Dia del Empleado Bancario','admin',GETDATE()),

(15, 8, 3, 1.0, 'Dia de la Virgen de la Asunción','admin',GETDATE()),
(15, 9, 1, 1.0, 'Dia de la independencia','admin',GETDATE()),
(12, 10, 2, 1.0, 'Dia de la Raza','admin',GETDATE()),
(20, 10, 1, 1.0, 'Dia de la Revolucion','Admin',GETDATE()),

(1, 11, 3, 1.0, 'Dia de los Santos','admin',GETDATE()),
(24, 12, 1, 0.5, 'Noche buena','admin',GETDATE()),
(25, 12, 1, 1.0, 'Navidad','admin',GETDATE()),
(31, 12, 1, 0.5, 'Fin de año','admin',GETDATE());

