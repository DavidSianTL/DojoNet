USE UniversidadDB;
GO

-- Mostrar duplicaci�n en la tabla desnormalizada
SELECT * FROM UniversidadDesnormalizada;

-- Mostrar que al separar los datos en tablas por entidad, eliminamos repetici�n
SELECT * FROM Alumno;
SELECT * FROM Curso;
SELECT * FROM Pensum;
select * from Carrera;