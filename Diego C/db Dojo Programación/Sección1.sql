USE DojoProgramacion;
GO

-- 1. Mostrar todos los candidatos con su correo
SELECT Nombre, Email
FROM Candidatos;

-- 2. Ver todos los ejercicios junto con su categoría y lenguaje
SELECT 
    E.Titulo,
    E.Descripcion,
    C.NombreCategoria,
    L.NombreLenguaje
FROM Ejercicios E
JOIN Categorias C ON E.IdCategoria = C.IdCategoria
JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje;

-- 3. Mostrar los resultados de cada candidato con el nombre del ejercicio y la fecha
SELECT 
    C.Nombre AS Candidato,
    E.Titulo AS Ejercicio,
    R.FechaEjecucion,
    R.Puntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio;

-- 4. Ver los candidatos que sacaron más de 85 puntos en algún ejercicio
SELECT DISTINCT C.Nombre, R.Puntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
WHERE R.Puntaje > 85;

-- 5. Calcular el promedio de puntos de cada candidato
SELECT 
    C.Nombre,
    AVG(R.Puntaje) AS PromedioPuntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
GROUP BY C.Nombre;

-- 6. Ver los permisos que tiene 'Ana Torres'
SELECT DISTINCT P.NombrePermiso
FROM Candidatos C
JOIN CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
JOIN RolesPermisos RP ON CR.IdRol = RP.IdRol
JOIN Permisos P ON RP.IdPermiso = P.IdPermiso
WHERE C.Nombre = 'Ana Torres';

-- 7. Mostrar los ejercicios que nadie ha resuelto
SELECT E.Titulo
FROM Ejercicios E
LEFT JOIN Resultados R ON E.IdEjercicio = R.IdEjercicio
WHERE R.IdEjercicio IS NULL;

