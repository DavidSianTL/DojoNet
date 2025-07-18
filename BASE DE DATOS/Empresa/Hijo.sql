CREATE TABLE Hijo
(
    Id INT PRIMARY KEY IDENTITY(1,1),

    PersonaId INT NOT NULL,  -- FK hacia Persona

    NombreCompleto NVARCHAR(200) NOT NULL,
    FechaNacimiento DATE NOT NULL,

    Sexo CHAR(1) NOT NULL CHECK (Sexo IN ('M', 'F')),

    Ocupacion NVARCHAR(100) NULL,

    EstaVivo BIT NOT NULL DEFAULT 1, -- 1 = vivo, 0 = fallecido

    CONSTRAINT FK_Hijo_Persona FOREIGN KEY (PersonaId)
        REFERENCES PersonaM(Id)
        ON DELETE CASCADE
);