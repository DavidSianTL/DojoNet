use SistemaSeguridad
go

CREATE TABLE Tokens (
    IdToken INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdSistema INT NOT NULL,
    Token NVARCHAR(MAX) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaExpiracion DATETIME NOT NULL,
    Estado BIT NOT NULL DEFAULT 1, -- 1: activo, 0: inactivo

    CONSTRAINT FK_Tokens_Usuarios FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(id_usuario),

    CONSTRAINT FK_Tokens_Sistemas FOREIGN KEY (IdSistema)
        REFERENCES Sistemas(id_sistema)
);

