carga de datos
USE UniversidadDB;
GO



INSERT INTO UniversidadDesnormalizada VALUES
(1, 'Ana P�rez', 'Ingenier�a en Sistemas', 'Bases de Datos', 'BD101', 'A101', 'Laboratorio', 'Matutina', 'Ing. L�pez', 2023),
(1, 'Ana P�rez', 'Ingenier�a en Sistemas', 'Matem�tica', 'MAT101', 'B201', 'Aula Te�rica', 'Matutina', 'Lic. G�mez', 2023),
(2, 'Carlos Ruiz', 'Ingenier�a en Sistemas', 'Bases de Datos', 'BD101', 'A101', 'Laboratorio', 'Matutina', 'Ing. L�pez', 2023);


select * from UniversidadDesnormalizada