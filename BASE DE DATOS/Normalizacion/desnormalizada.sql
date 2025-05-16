CREATE DATABASE UniversidadDB;
GO

USE UniversidadDB;
GO



CREATE TABLE UniversidadDesnormalizada (
    AlumnoID INT,
    NombreAlumno VARCHAR(100),
    Carrera VARCHAR(100),
    Curso VARCHAR(100),
    CodigoCurso VARCHAR(20),
    Salon VARCHAR(20),
    TipoSalon VARCHAR(50),
    Jornada VARCHAR(50),
    Catedratico VARCHAR(100),
    PensumAño INT
);


