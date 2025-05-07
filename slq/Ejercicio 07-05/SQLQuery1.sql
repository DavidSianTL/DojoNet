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










//Insercion de datos 

-- Candidatos
INSERT INTO Candidatos (Nombre, Email) VALUES
('Ana Torres', 'ana.torres@email.com'),
('Carlos Pérez', 'carlos.perez@email.com'),
('Luis Gómez', 'luis.gomez@email.com'),
('María López', 'maria.lopez@email.com'),
('Pedro Martínez', 'pedro.martinez@email.com');

-- Categorías
INSERT INTO Categorias (NombreCategoria) VALUES
('Lógica'),
('Algoritmos'),
('Bases de Datos'),
('Frontend'),
('Backend');

-- Lenguajes
INSERT INTO Lenguajes (NombreLenguaje) VALUES
('C#'),
('JavaScript'),
('Python'),
('SQL'),
('Java');

-- Ejercicios
INSERT INTO Ejercicios (Titulo, Descripcion, IdCategoria, IdLenguaje) VALUES
('Suma de números', 'Escriba una función que sume dos números', 1, 1),
('Buscar en arreglo', 'Buscar un número en un arreglo ordenado', 2, 3),
('Consulta JOIN', 'Escriba una consulta JOIN entre dos tablas', 3, 4),
('Formulario simple', 'Cree un formulario con validaciones', 4, 2),
('API REST básica', 'Cree un controlador que devuelva datos JSON', 5, 1);

-- Resultados
INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje) VALUES
(1, 1, '2024-05-01', 90),
(1, 2, '2024-05-02', 85),
(2, 3, '2024-05-01', 70),
(3, 4, '2024-05-03', 95),
(4, 5, '2024-05-01', 88);

-- Roles
INSERT INTO Roles (NombreRol) VALUES
('Administrador'),
('Instructor'),
('Candidato'),
('Evaluador');

-- Permisos
INSERT INTO Permisos (NombrePermiso) VALUES
('Ver Resultados'),
('Editar Ejercicios'),
('Asignar Pruebas'),
('Acceder Panel'),
('Enviar Ejercicio');

-- CandidatosRoles
INSERT INTO CandidatosRoles (IdCandidato, IdRol) VALUES
(1, 3),
(2, 3),
(3, 3),
(4, 2),
(4, 1),
(5, 4);

-- RolesPermisos
INSERT INTO RolesPermisos (IdRol, IdPermiso) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
(2, 1), (2, 2), (2, 3), (2, 5),
(3, 1), (3, 5),
(4, 1), (4, 3);