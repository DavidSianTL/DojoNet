-- =============================================
-- Tabla para almacenar PDFs de solicitudes comprimidos con Brotli
-- =============================================

use DBProyectoGrupalDojoGeko; 

CREATE TABLE SolicitudPDF (
    IdSolicitudPDF INT IDENTITY(1,1) PRIMARY KEY,
    FK_IdSolicitud INT NOT NULL,
    NombreArchivo NVARCHAR(255) NOT NULL,
    ContenidoPDFComprimido VARBINARY(MAX) NOT NULL, -- PDF comprimido con Brotli
    TamanoOriginal BIGINT NOT NULL, -- Tamaño antes de comprimir
    TamanoComprimido BIGINT NOT NULL, -- Tamaño después de comprimir
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FK_IdEstado INT DEFAULT 1, -- 1=Disponible, 4=Restringido
    
    CONSTRAINT FK_SolicitudPDF_Solicitud 
        FOREIGN KEY (FK_IdSolicitud) 
            REFERENCES SolicitudEncabezado(IdSolicitud) 
                ON DELETE CASCADE,
    CONSTRAINT FK_SolicitudPDF_Estado 
        FOREIGN KEY (FK_IdEstado) 
            REFERENCES Estados(IdEstado)
);
GO

-- SP para insertar PDF de solicitud
CREATE PROCEDURE sp_InsertarSolicitudPDF
    @FK_IdSolicitud INT,
    @NombreArchivo NVARCHAR(255),
    @ContenidoPDFComprimido VARBINARY(MAX),
    @TamanoOriginal BIGINT,
    @TamanoComprimido BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Eliminar PDF anterior si existe
    DELETE FROM SolicitudPDF WHERE FK_IdSolicitud = @FK_IdSolicitud;
    
    -- Insertar nuevo PDF
    INSERT INTO SolicitudPDF (
        FK_IdSolicitud, 
        NombreArchivo, 
        ContenidoPDFComprimido, 
        TamanoOriginal, 
        TamanoComprimido
    )
    VALUES (
        @FK_IdSolicitud, 
        @NombreArchivo, 
        @ContenidoPDFComprimido, 
        @TamanoOriginal, 
        @TamanoComprimido
    );
    
    SELECT SCOPE_IDENTITY() AS IdSolicitudPDF;
END;
GO

-- SP para obtener PDF de solicitud
CREATE PROCEDURE sp_ObtenerSolicitudPDF
    @FK_IdSolicitud INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        sp.IdSolicitudPDF,
        sp.FK_IdSolicitud,
        sp.NombreArchivo,
        sp.ContenidoPDFComprimido,
        sp.TamanoOriginal,
        sp.TamanoComprimido,
        sp.FechaCreacion,
        sp.FK_IdEstado,
        se.FK_IdEstadoSolicitud
    FROM SolicitudPDF sp
    INNER JOIN SolicitudEncabezado se ON sp.FK_IdSolicitud = se.IdSolicitud
    WHERE sp.FK_IdSolicitud = @FK_IdSolicitud;
END;
GO

-- SP para restringir descarga cuando se aprueba la solicitud
CREATE PROCEDURE sp_RestringirDescargaPDF
    @FK_IdSolicitud INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE SolicitudPDF 
    SET FK_IdEstado = 4 -- Estado restringido
    WHERE FK_IdSolicitud = @FK_IdSolicitud;
END;
GO
