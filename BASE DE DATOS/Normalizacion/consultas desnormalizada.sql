USE UniversidadDB;
GO

-- Mostrar duplicación en la tabla desnormalizada
SELECT * FROM UniversidadDesnormalizada;

-- Mostrar que al separar los datos en tablas por entidad, eliminamos repetición
SELECT * FROM Alumno;
SELECT * FROM Curso;
SELECT * FROM Pensum;
select * from Carrera;