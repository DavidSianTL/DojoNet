-- 8. Crear la vista con la info completa de los resultados
CREATE VIEW VistaResultadosDetallados AS
SELECT 
    C.Nombre AS Candidato,
    C.Email,
    E.Titulo AS Ejercicio,
    L.NombreLenguaje AS Lenguaje,
    R.Puntaje,
    R.FechaEjecucion
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio
JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje;
GO
SELECT * FROM VistaResultadosDetallados;
GO

CREATE VIEW VistaPermisosCandidatos AS
SELECT 
    C.Nombre AS NombreCandidato,
    R.NombreRol,
    P.NombrePermiso
FROM Candidatos C
JOIN CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
JOIN Roles R ON CR.IdRol = R.IdRol
JOIN RolesPermisos RP ON R.IdRol = RP.IdRol
JOIN Permisos P ON RP.IdPermiso = P.IdPermiso;
GO
SELECT * FROM VistaPermisosCandidatos;

GO


CREATE OR ALTER VIEW VistaEjerciciosResueltos AS
SELECT 
    e.IdEjercicio,
    e.Titulo,
    COUNT(r.IdResultado) AS VecesResuelto
FROM Ejercicios e
LEFT JOIN Resultados r ON e.IdEjercicio = r.IdEjercicio
GROUP BY e.IdEjercicio, e.Titulo;
GO

SELECT * FROM VistaEjerciciosResueltos;
GO