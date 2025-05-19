

USE UniversidadDB;
GO

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

-- Tabla Jornada
CREATE TABLE Jornada (
    JornadaID INT PRIMARY KEY,
    NombreJornada VARCHAR(50)
);

-- Relación Alumno - Curso
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
