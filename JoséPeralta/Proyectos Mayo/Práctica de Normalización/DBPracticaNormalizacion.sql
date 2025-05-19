USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('DBPracticaNormalizacion') IS NOT NULL
BEGIN
    ALTER DATABASE DBPracticaNormalizacion SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBPracticaNormalizacion;
END
GO

CREATE DATABASE DBPracticaNormalizacion;
GO

USE DBPracticaNormalizacion;
GO

-- Tabla de Carreras
CREATE TABLE Carrera (
    CarreraID INT PRIMARY KEY,
    NombreCarrera VARCHAR(100)
);

-- Insertar Carreras
INSERT INTO Carrera VALUES (1, 'Ingeniería en Sistemas');
INSERT INTO Carrera VALUES (2, 'Ingeniería Industrial');

-- Tabla de Alumnos
CREATE TABLE Alumno (
    AlumnoID INT PRIMARY KEY,
    NombreAlumno VARCHAR(100),
    CarreraID INT,
    FOREIGN KEY (CarreraID) REFERENCES Carrera(CarreraID)
);

-- Insertar Alumnos
INSERT INTO Alumno VALUES (1, 'Ana Pérez', 1);
INSERT INTO Alumno VALUES (2, 'Carlos Ruiz', 1);

-- Tabla de Cursos
CREATE TABLE Curso (
    CodigoCurso VARCHAR(20) PRIMARY KEY,
    NombreCurso VARCHAR(100)
);

-- Insertar Cursos
INSERT INTO Curso VALUES ('BD101', 'Bases de Datos');
INSERT INTO Curso VALUES ('MAT101', 'Matemática');

-- Tabla Pensum
CREATE TABLE Pensum (
    PensumID INT PRIMARY KEY,
    CarreraID INT,
    CodigoCurso VARCHAR(20),
    Año INT,
    FOREIGN KEY (CarreraID) REFERENCES Carrera(CarreraID),
    FOREIGN KEY (CodigoCurso) REFERENCES Curso(CodigoCurso)
);

-- Insertar Pensum
INSERT INTO Pensum VALUES (1, 1, 'BD101', 2023);
INSERT INTO Pensum VALUES (2, 1, 'MAT101', 2023);

-- Tabla de Catedráticos
CREATE TABLE Catedratico (
    CatedraticoID INT PRIMARY KEY,
    NombreCatedratico VARCHAR(100)
);

-- Insertar Catedráticos
INSERT INTO Catedratico VALUES (1, 'Ing. López');
INSERT INTO Catedratico VALUES (2, 'Lic. Gómez');

-- Tabla de Salones
CREATE TABLE Salon (
    SalonID VARCHAR(20) PRIMARY KEY,
    TipoSalon VARCHAR(50)
);

-- Insertar Salones
INSERT INTO Salon VALUES ('A101', 'Laboratorio');
INSERT INTO Salon VALUES ('B201', 'Aula Teórica');

-- Tabla de Jornadas
CREATE TABLE Jornada (
    JornadaID INT PRIMARY KEY,
    NombreJornada VARCHAR(50)
);

-- Insertar Jornadas
INSERT INTO Jornada VALUES (1, 'Matutina');
INSERT INTO Jornada VALUES (2, 'Vespertina');

-- Tabla relación Alumno-Curso
CREATE TABLE AlumnoCurso (
    AlumnoID INT,
    CodigoCurso VARCHAR(20),
    CatedraticoID INT,
    SalonID VARCHAR(20),
    JornadaID INT,
    PRIMARY KEY (AlumnoID, CodigoCurso),
    FOREIGN KEY (AlumnoID) REFERENCES Alumno(AlumnoID),
    FOREIGN KEY (CodigoCurso) REFERENCES Curso(CodigoCurso),
    FOREIGN KEY (CatedraticoID) REFERENCES Catedratico(CatedraticoID),
    FOREIGN KEY (SalonID) REFERENCES Salon(SalonID),
    FOREIGN KEY (JornadaID) REFERENCES Jornada(JornadaID)
);

-- Insertar datos en AlumnoCurso
INSERT INTO AlumnoCurso VALUES (1, 'BD101', 1, 'A101', 1);
INSERT INTO AlumnoCurso VALUES (1, 'MAT101', 2, 'B201', 1);
INSERT INTO AlumnoCurso VALUES (2, 'BD101', 1, 'A101', 1);

-- Qué catedraticos dan cursos en la Ing. de Sistemaas
SELECT DISTINCT CA.NombreCatedratico FROM Carrera C 
	JOIN Pensum P ON P.CarreraID = C.CarreraID
	JOIN Curso CU ON cu.CodigoCurso = p.CodigoCurso
	JOIN AlumnoCurso AC ON AC.CodigoCurso = CU.CodigoCurso
	JOIN Catedratico CA ON CA.CatedraticoID = AC.CatedraticoID
	WHERE C.NombreCarrera LIKE '%Ingeniería%';