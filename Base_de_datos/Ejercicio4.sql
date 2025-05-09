-- Crear tabla de empresas
CREATE TABLE Empresas (
    IdEmpresa INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100)
);

-- Relacionar candidatos con empresas
ALTER TABLE Candidatos ADD IdEmpresa INT;
ALTER TABLE Candidatos ADD CONSTRAINT FK_Candidatos_Empresas FOREIGN KEY (IdEmpresa) REFERENCES Empresas(IdEmpresa);

-- Agregar columna de estado
ALTER TABLE Candidatos ADD Estado VARCHAR(20) DEFAULT 'Pendiente';

-- Verificar usuarios en estado pendiente
SELECT * FROM Candidatos WHERE Estado = 'Pendiente';
-- Crear tabla de autorizaciones
CREATE TABLE Autorizaciones (
    IdAutorizacion INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50),
    Descripcion TEXT
);

-- Relacionar con roles
CREATE TABLE RolesAutorizaciones (
    IdRol INT,
    IdAutorizacion INT,
    PRIMARY KEY (IdRol, IdAutorizacion),
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol),
    FOREIGN KEY (IdAutorizacion) REFERENCES Autorizaciones(IdAutorizacion)
);
