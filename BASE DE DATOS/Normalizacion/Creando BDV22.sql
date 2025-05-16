
-- Eliminar BD si existe
IF DB_ID('UniversidadDB') IS NOT NULL
    DROP DATABASE UniversidadDB;
GO

-- Crear Base de Datos
CREATE DATABASE UniversidadDB;
GO

USE UniversidadDB;
GO

-- Crear tablas normalizadas

-- Tabla de Alumnos
CREATE TABLE Alumno (
    AlumnoID INT PRIMARY KEY,
    NombreAlumno VARCHAR(100),
    Carrera VARCHAR(100)
);

-- Tabla de Cursos
CREATE TABLE Curso (
    CodigoCurso VARCHAR(20) PRIMARY KEY,
    NombreCurso VARCHAR(100),
    PensumAño INT
);

-- Tabla de Catedráticos
CREATE TABLE Catedratico (
    CatedraticoID INT PRIMARY KEY,
    NombreCatedratico VARCHAR(100)
);

-- Tabla de Salones
CREATE TABLE Salon (
    SalonID VARCHAR(20) PRIMARY KEY,
    TipoSalon VARCHAR(50)
);

-- Tabla de Jornadas
CREATE TABLE Jornada (
    JornadaID INT PRIMARY KEY,
    NombreJornada VARCHAR(50)
);

-- Tabla relación Alumno-Curso (con datos de catedrático, salón y jornada)
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

-- Insertar datos en Alumno
INSERT INTO Alumno VALUES (1, 'Ana Pérez', 'Ingeniería en Sistemas');
INSERT INTO Alumno VALUES (2, 'Carlos Ruiz', 'Ingeniería en Sistemas');

-- Insertar datos en Curso
INSERT INTO Curso VALUES ('BD101', 'Bases de Datos', 2023);
INSERT INTO Curso VALUES ('MAT101', 'Matemática', 2023);

-- Insertar datos en Catedratico
INSERT INTO Catedratico VALUES (1, 'Ing. López');
INSERT INTO Catedratico VALUES (2, 'Lic. Gómez');

-- Insertar datos en Salon
INSERT INTO Salon VALUES ('A101', 'Laboratorio');
INSERT INTO Salon VALUES ('B201', 'Aula Teórica');

-- Insertar datos en Jornada
INSERT INTO Jornada VALUES (1, 'Matutina');
INSERT INTO Jornada VALUES (2, 'Vespertina');

-- Insertar datos en AlumnoCurso
INSERT INTO AlumnoCurso VALUES (1, 'BD101', 1, 'A101', 1);
INSERT INTO AlumnoCurso VALUES (1, 'MAT101', 2, 'B201', 1);
INSERT INTO AlumnoCurso VALUES (2, 'BD101', 1, 'A101', 1);

-- Consulta ejemplo para ver relación alumno-curso
SELECT a.NombreAlumno, c.NombreCurso, cat.NombreCatedratico, s.TipoSalon, j.NombreJornada
FROM AlumnoCurso ac
JOIN Alumno a ON ac.AlumnoID = a.AlumnoID
JOIN Curso c ON ac.CodigoCurso = c.CodigoCurso
JOIN Catedratico cat ON ac.CatedraticoID = cat.CatedraticoID
JOIN Salon s ON ac.SalonID = s.SalonID
JOIN Jornada j ON ac.JornadaID = j.JornadaID;
