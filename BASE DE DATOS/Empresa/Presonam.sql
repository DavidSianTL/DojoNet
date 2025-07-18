-- Tabla principal Persona
CREATE TABLE PersonaM
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    ApellidoDeCasada NVARCHAR(100) NULL,
    Sexo CHAR(1) NOT NULL CHECK (Sexo IN ('M', 'F')),
    FechaNacimiento DATE NOT NULL,

    TipoDocumentoId INT NOT NULL,
    DocIdentificacion NVARCHAR(25) NOT NULL,

    CorreoPersonal NVARCHAR(100) NULL,
    CorreoEmpresa NVARCHAR(100) NULL,
    Telefono NVARCHAR(20) NULL,
    Direccion NVARCHAR(200) NULL,

    EstadoCivil NVARCHAR(50) NULL,
    Nacionalidad NVARCHAR(50) NULL,
    NumeroHijos INT NOT NULL DEFAULT 0,

    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    UsuarioCreacion NVARCHAR(50) NULL,
    FechaModificacion DATETIME NULL,
    UsuarioModificacion NVARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Persona_TipoDocumento FOREIGN KEY (TipoDocumentoId)
        REFERENCES TipoDocumento(Id)
);