-- 1. Crear la base de datos
CREATE DATABASE DojoProgramacion;
GO

USE DojoProgramacion;
GO

-- 2. Crear tabla Candidatos
CREATE TABLE Candidatos (
    IdCandidato INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100)
);

-- 3. Crear tabla Categorias
CREATE TABLE Categorias (
    IdCategoria INT PRIMARY KEY IDENTITY,
    NombreCategoria NVARCHAR(100)
);

-- 4. Crear tabla Lenguajes
CREATE TABLE Lenguajes (
    IdLenguaje INT PRIMARY KEY IDENTITY,
    NombreLenguaje NVARCHAR(100)
);

-- 5. Crear tabla Ejercicios
CREATE TABLE Ejercicios (
    IdEjercicio INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(100),
    Descripcion NVARCHAR(MAX),
    IdCategoria INT,
    IdLenguaje INT,
    FOREIGN KEY (IdCategoria) REFERENCES Categorias(IdCategoria),
    FOREIGN KEY (IdLenguaje) REFERENCES Lenguajes(IdLenguaje)
);

-- 6. Crear tabla Resultados
CREATE TABLE Resultados (
    IdResultado INT PRIMARY KEY IDENTITY,
    IdCandidato INT,
    IdEjercicio INT,
    FechaEjecucion DATE,
    Puntaje INT,
    FOREIGN KEY (IdCandidato) REFERENCES Candidatos(IdCandidato),
    FOREIGN KEY (IdEjercicio) REFERENCES Ejercicios(IdEjercicio)
);

-- 7. Crear tabla Roles
CREATE TABLE Roles (
    IdRol INT PRIMARY KEY IDENTITY,
    NombreRol NVARCHAR(50)
);

-- 8. Crear tabla Permisos
CREATE TABLE Permisos (
    IdPermiso INT PRIMARY KEY IDENTITY,
    NombrePermiso NVARCHAR(100)
);

-- 9. Crear tabla CandidatosRoles
CREATE TABLE CandidatosRoles (
    IdCandidato INT,
    IdRol INT,
    PRIMARY KEY (IdCandidato, IdRol),
    FOREIGN KEY (IdCandidato) REFERENCES Candidatos(IdCandidato),
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);

-- 10. Crear tabla RolesPermisos
CREATE TABLE RolesPermisos (
    IdRol INT,
    IdPermiso INT,
    PRIMARY KEY (IdRol, IdPermiso),
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol),
    FOREIGN KEY (IdPermiso) REFERENCES Permisos(IdPermiso)
);