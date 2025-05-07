--SECCION 1
--1. Obtener todos los candidatos con su email
SELECT Nombre, Email FROM Candidatos;

--2. Listar todos los ejercicios con su categoría y lenguaje de programación
SELECT E.Titulo, E.Descripcion, C.NombreCategoria, L.NombreLenguaje FROM Ejercicios E
JOIN Categorias C ON E.IdCategoria = C.IdCategoria JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje;

--3. Obtener los resultados de cada candidato con el título del ejercicio y la fecha de ejecución
SELECT C.Nombre AS Nombre_Candidato, E.Titulo AS Titulo_Ejercicio, R.FechaEjecucion, R.Puntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio;


--4. Listar todos los candidatos que hayan sacado más de 85 puntos en cualquier ejercicio
SELECT C.Nombre, C.Email, R.Puntaje FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
WHERE R.Puntaje > 85;


--5. Obtener el promedio de puntajes por candidato
SELECT C.Nombre AS NombreCandidato, AVG(R.Puntaje) AS PromedioPuntaje FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato GROUP BY C.Nombre;

--6. Listar los permisos que tiene asignado el candidato con nombre 'Ana Torres'
SELECT P.NombrePermiso FROM Candidatos C
JOIN CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
JOIN RolesPermisos RP ON CR.IdRol = RP.IdRol
JOIN Permisos P ON RP.IdPermiso = P.IdPermiso
WHERE C.Nombre = 'Ana Torres';

--7. Listar los ejercicios que no han sido resueltos por ningún candidato
SELECT E.Titulo, E.Descripcion FROM Ejercicios E LEFT JOIN Resultados R ON E.IdEjercicio = R.IdEjercicio
WHERE R.IdEjercicio IS NULL;

-- bonus
--- ver por quien ha sido resueltos los ejercicios
SELECT C.Nombre AS NombreCandidato, E.Titulo AS TituloEjercicio,R.FechaEjecucion, R.Puntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio;

