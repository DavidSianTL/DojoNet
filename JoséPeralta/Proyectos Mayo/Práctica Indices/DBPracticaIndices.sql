USE MASTER;
GO

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
IF DB_ID('DBPracticaIndices') IS NOT NULL
BEGIN
    ALTER DATABASE DBPracticaIndices SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DBPracticaIndices;
END
GO

-- Crear la base de datos
CREATE DATABASE DBPracticaIndices;
GO

-- Usar la base de datos reci�n creada
USE DBPracticaIndices;
GO

-- Crear la tabla
CREATE TABLE Alumnos(
    id_alumno INT IDENTITY(1,1),
    nombre NVARCHAR(100),
    apellido NVARCHAR(100),
    edad INT,
    genero NVARCHAR(10),
    fecha_nacimiento DATE,
    PRIMARY KEY  (id_alumno)
);
GO

-- Creamos el índice NONCLUSTERED
CREATE NONCLUSTERED INDEX id_nombre ON Alumnos(nombre);
GO

-- Insertamos datos
INSERT INTO Alumnos (nombre, apellido, edad, genero, fecha_nacimiento)
VALUES 
    ('Juan', 'Pérez', 20, 'Masculino', '2005-01-01'),
    ('María', 'García', 22, 'Femenino', '2003-02-15'),
    ('Pedro', 'Rodríguez', 21, 'Masculino', '2004-05-20'),
    ('Ana', 'Martínez', 23, 'Femenino', '2002-08-10'),
    ('Luis', 'Sánchez', 24, 'Masculino', '2001-12-25'),
    ('Sofía', 'Fernández', 19, 'Femenino', '2006-03-18'),
    ('Miguel', 'Torres', 25, 'Masculino', '2000-07-12'),
    ('Lucía', 'Ramírez', 22, 'Femenino', '2003-11-05'),
    ('Carlos', 'Morales', 21, 'Masculino', '2004-02-28'),
    ('Valentina', 'Herrera', 23, 'Femenino', '2002-09-14'),
    ('Diego', 'Flores', 20, 'Masculino', '2005-06-22'),
    ('Camila', 'Jiménez', 24, 'Femenino', '2001-10-03'),
    ('Andrés', 'Ruiz', 22, 'Masculino', '2003-04-16'),
    ('Paula', 'Mendoza', 21, 'Femenino', '2004-12-19'),
    ('Javier', 'Castro', 23, 'Masculino', '2002-05-09'),
    ('Isabella', 'Vargas', 20, 'Femenino', '2005-08-27'),
    ('Mateo', 'Silva', 24, 'Masculino', '2001-03-30'),
    ('Gabriela', 'Ortega', 22, 'Femenino', '2003-07-21'),
    ('Samuel', 'Ramos', 21, 'Masculino', '2004-11-11'),
    ('Renata', 'Cruz', 23, 'Femenino', '2002-01-25'),
    ('Emilio', 'Delgado', 19, 'Masculino', '2006-09-08'),
    ('Victoria', 'Paredes', 25, 'Femenino', '2000-12-02');
GO

-- Consulta sin índice
SELECT * FROM Alumnos WHERE nombre = 'Juan';
GO

-- Mostramos el plan de ejecución
SET STATISTICS TIME ON;
-- Consulta con índice
SELECT * FROM Alumnos WHERE nombre = 'Juan';
-- Mostramos el plan de ejecución
SET STATISTICS TIME OFF;
GO

