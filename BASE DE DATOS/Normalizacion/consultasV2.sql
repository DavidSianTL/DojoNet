
-- Consulta: Cursos de una Carrera por Pensum
SELECT c.NombreCarrera, cu.NombreCurso, p.A�o
FROM Pensum p
JOIN Carrera c ON p.CarreraID = c.CarreraID
JOIN Curso cu ON p.CodigoCurso = cu.CodigoCurso
WHERE c.NombreCarrera = 'Ingenier�a en Sistemas' AND p.A�o = 2023;

-- Consulta: Relaci�n alumno-curso completa
SELECT a.NombreAlumno, cr.NombreCarrera, cu.NombreCurso, cat.NombreCatedratico, s.TipoSalon, j.NombreJornada
FROM AlumnoCurso ac
JOIN Alumno a ON ac.AlumnoID = a.AlumnoID
JOIN Carrera cr ON a.CarreraID = cr.CarreraID
JOIN Curso cu ON ac.CodigoCurso = cu.CodigoCurso
JOIN Catedratico cat ON ac.CatedraticoID = cat.CatedraticoID
JOIN Salon s ON ac.SalonID = s.SalonID
JOIN Jornada j ON ac.JornadaID = j.JornadaID;


-- 
SELECT c.NombreCarrera, cu.NombreCurso, p.A�o
FROM Pensum p
JOIN Carrera c ON p.CarreraID = c.CarreraID
JOIN Curso cu ON p.CodigoCurso = cu.CodigoCurso
WHERE c.NombreCarrera = 'Ingenier�a en Sistemas' AND p.A�o = 2023;
