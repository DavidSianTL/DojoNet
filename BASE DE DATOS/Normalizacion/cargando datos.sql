carga de datos
USE UniversidadDB;
GO



INSERT INTO UniversidadDesnormalizada VALUES
(1, 'Ana Pérez', 'Ingeniería en Sistemas', 'Bases de Datos', 'BD101', 'A101', 'Laboratorio', 'Matutina', 'Ing. López', 2023),
(1, 'Ana Pérez', 'Ingeniería en Sistemas', 'Matemática', 'MAT101', 'B201', 'Aula Teórica', 'Matutina', 'Lic. Gómez', 2023),
(2, 'Carlos Ruiz', 'Ingeniería en Sistemas', 'Bases de Datos', 'BD101', 'A101', 'Laboratorio', 'Matutina', 'Ing. López', 2023);


select * from UniversidadDesnormalizada