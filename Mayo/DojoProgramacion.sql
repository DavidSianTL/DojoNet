
CREATE DATABASE DojoProgramacion;
GO

USE DojoProgramacion;
GO


CREATE TABLE Candidatos (
    IdCandidato INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(100),
    Email NVARCHAR(100)
);
CREATE TABLE Categorias (
    IdCategoria INT PRIMARY KEY IDENTITY,
    NombreCategoria NVARCHAR(100)
);
CREATE TABLE Lenguajes (
    IdLenguaje INT PRIMARY KEY IDENTITY,
    NombreLenguaje NVARCHAR(100)
);
CREATE TABLE Ejercicios (
    IdEjercicio INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(100),
    Descripcion NVARCHAR(MAX),
    IdCategoria INT FOREIGN KEY REFERENCES Categorias(IdCategoria),
    IdLenguaje INT FOREIGN KEY REFERENCES Lenguajes(IdLenguaje)
);
CREATE TABLE Resultados (
    IdResultado INT PRIMARY KEY IDENTITY,
    IdCandidato INT FOREIGN KEY REFERENCES Candidatos(IdCandidato),
    IdEjercicio INT FOREIGN KEY REFERENCES Ejercicios(IdEjercicio),
    FechaEjecucion DATE,
    Puntaje INT
);
CREATE TABLE Roles (
    IdRol INT PRIMARY KEY IDENTITY,
    NombreRol NVARCHAR(50)
);
CREATE TABLE Permisos (
    IdPermiso INT PRIMARY KEY IDENTITY,
    NombrePermiso NVARCHAR(100)
);
CREATE TABLE CandidatosRoles (
    IdCandidato INT FOREIGN KEY REFERENCES Candidatos(IdCandidato),
    IdRol INT FOREIGN KEY REFERENCES Roles(IdRol),
    PRIMARY KEY (IdCandidato, IdRol)
);
CREATE TABLE RolesPermisos (
    IdRol INT FOREIGN KEY REFERENCES Roles(IdRol),
    IdPermiso INT FOREIGN KEY REFERENCES Permisos(IdPermiso),
    PRIMARY KEY (IdRol, IdPermiso)
);
GO


INSERT INTO Candidatos (Nombre, Email) VALUES
('Ana Torres', 'ana.torres@email.com'),
('Carlos Pérez', 'carlos.perez@email.com'),
('Luis Gómez', 'luis.gomez@email.com'),
('María López', 'maria.lopez@email.com'),
('Pedro Martínez', 'pedro.martinez@email.com');
INSERT INTO Categorias (NombreCategoria) VALUES
('Lógica'),('Algoritmos'),('Bases de Datos'),('Frontend'),('Backend');
INSERT INTO Lenguajes (NombreLenguaje) VALUES
('C#'),('JavaScript'),('Python'),('SQL'),('Java');
INSERT INTO Ejercicios (Titulo, Descripcion, IdCategoria, IdLenguaje) VALUES
('Suma de números','Escriba una función que sume dos números',1,1),
('Buscar en arreglo','Buscar un número en un arreglo ordenado',2,3),
('Consulta JOIN','Escriba una consulta JOIN entre dos tablas',3,4),
('Formulario simple','Cree un formulario con validaciones',4,2),
('API REST básica','Cree un controlador que devuelva datos JSON',5,1);
INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje) VALUES
(1,1,'2024-05-01',90),
(1,2,'2024-05-02',85),
(2,3,'2024-05-01',70),
(3,4,'2024-05-03',95),
(4,5,'2024-05-01',88);
INSERT INTO Roles (NombreRol) VALUES
('Administrador'),('Instructor'),('Candidato'),('Evaluador');
INSERT INTO Permisos (NombrePermiso) VALUES
('Ver Resultados'),('Editar Ejercicios'),
('Asignar Pruebas'),('Acceder Panel'),('Enviar Ejercicio');
INSERT INTO CandidatosRoles (IdCandidato, IdRol) VALUES
(1,3),(2,3),(3,3),(4,2),(4,1),(5,4);
INSERT INTO RolesPermisos (IdRol, IdPermiso) VALUES
(1,1),(1,2),(1,3),(1,4),(1,5),
(2,1),(2,2),(2,3),(2,5),
(3,1),(3,5),
(4,1),(4,3);
GO

IF OBJECT_ID('dbo.LogsResultados','U') IS NULL
BEGIN
  CREATE TABLE LogsResultados (
    IdLog INT PRIMARY KEY IDENTITY,
    IdCandidato INT,
    IdEjercicio INT,
    Puntaje INT,
    FechaRegistro DATETIME DEFAULT GETDATE()
  );
END;
GO


CREATE VIEW VistaResultadosDetallados AS
SELECT
  ca.Nombre       AS NombreCandidato,
  e.Titulo        AS TituloEjercicio,
  l.NombreLenguaje AS Lenguaje,
  r.Puntaje,
  r.FechaEjecucion
FROM Resultados r
JOIN Candidatos ca ON r.IdCandidato = ca.IdCandidato
JOIN Ejercicios e ON r.IdEjercicio = e.IdEjercicio
JOIN Lenguajes l ON e.IdLenguaje = l.IdLenguaje;
GO
CREATE VIEW VistaPermisosCandidatos AS
SELECT
  ca.Nombre       AS NombreCandidato,
  ro.NombreRol    AS Rol,
  per.NombrePermiso AS Permiso
FROM Candidatos ca
JOIN CandidatosRoles cr ON ca.IdCandidato = cr.IdCandidato
JOIN Roles ro ON cr.IdRol = ro.IdRol
JOIN RolesPermisos rp ON ro.IdRol = rp.IdRol
JOIN Permisos per ON rp.IdPermiso = per.IdPermiso;
GO
CREATE VIEW VistaConteoResueltos AS
SELECT
  e.Titulo,
  COUNT(r.IdResultado) AS VecesResuelto
FROM Ejercicios e
LEFT JOIN Resultados r ON e.IdEjercicio = r.IdEjercicio
GROUP BY e.Titulo;
GO

CREATE PROCEDURE RegistrarResultado
  @IdCandidato INT,
  @IdEjercicio INT,
  @Puntaje     INT
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRANSACTION;
    INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje)
    VALUES (@IdCandidato, @IdEjercicio, GETDATE(), @Puntaje);
    INSERT INTO LogsResultados (IdCandidato, IdEjercicio, Puntaje)
    VALUES (@IdCandidato, @IdEjercicio, @Puntaje);
  COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE ObtenerResultadosPorLenguaje
  @NombreLenguaje NVARCHAR(100)
AS
BEGIN
  SELECT
    ca.Nombre       AS Candidato,
    e.Titulo        AS Ejercicio,
    r.Puntaje,
    r.FechaEjecucion
  FROM Resultados r
  JOIN Candidatos ca ON r.IdCandidato = ca.IdCandidato
  JOIN Ejercicios e ON r.IdEjercicio = e.IdEjercicio
  JOIN Lenguajes l ON e.IdLenguaje = l.IdLenguaje
  WHERE l.NombreLenguaje = @NombreLenguaje;
END;
GO

CREATE PROCEDURE AsignarRolCandidato
  @IdCandidato INT,
  @IdRol       INT
AS
BEGIN
  IF NOT EXISTS (
    SELECT 1 FROM CandidatosRoles
    WHERE IdCandidato = @IdCandidato
      AND IdRol       = @IdRol
  )
  BEGIN
    INSERT INTO CandidatosRoles (IdCandidato, IdRol)
    VALUES (@IdCandidato, @IdRol);
  END
END;
GO

CREATE PROCEDURE ObtenerPermisosPorRol
  @NombreRol NVARCHAR(50)
AS
BEGIN
  SELECT
    ro.NombreRol,
    per.NombrePermiso
  FROM Roles ro
  JOIN RolesPermisos rp ON ro.IdRol = rp.IdRol
  JOIN Permisos per ON rp.IdPermiso = per.IdPermiso
  WHERE ro.NombreRol = @NombreRol;
END;
GO
