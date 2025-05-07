/*Seccion 1 */
-- 1. Obtener todos los candidatos con su email.
SELECT Nombre, Email FROM Candidatos;

-- 2. Listar todos los ejercicios con su categoría y lenguaje de programación.
SELECT E.Titulo, C.Nombre AS Categoria, L.Nombre AS Lenguaje
FROM Ejercicios E
JOIN Categorias C ON E.IdCategoria = C.IdCategoria
JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje;

-- 3. Obtener los resultados de cada candidato con el título del ejercicio y la fecha de ejecución.
SELECT C.Nombre, E.Titulo, R.FechaEjecucion
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio;

-- 4. Listar todos los candidatos que hayan sacado más de 85 puntos en cualquier ejercicio.
SELECT DISTINCT C.Nombre
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
WHERE R.Puntaje > 85;

-- 5. Obtener el promedio de puntajes por candidato.
SELECT C.Nombre, AVG(R.Puntaje) AS PromedioPuntaje
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
GROUP BY C.Nombre;

-- 6. Listar los permisos que tiene asignado el candidato con nombre 'Ana Torres'.
SELECT P.Nombre AS Permiso
FROM Permisos P
JOIN RolesPermisos RP ON P.IdPermiso = RP.IdPermiso
JOIN CandidatosRoles CR ON RP.IdRol = CR.IdRol
JOIN Candidatos C ON CR.IdCandidato = C.IdCandidato
WHERE C.Nombre = 'Ana Torres';

-- 7. Listar los ejercicios que no han sido resueltos por ningún candidato.
SELECT E.Titulo
FROM Ejercicios E
LEFT JOIN Resultados R ON E.IdEjercicio = R.IdEjercicio
WHERE R.IdEjercicio IS NULL;

/*SECCIÓN 2 */
-- 8. Vista que consolida los resultados de los candidatos.
CREATE VIEW VistaResultadosDetallados AS
SELECT C.Nombre AS NombreCandidato, E.Titulo AS TituloEjercicio, L.Nombre AS Lenguaje, 
       R.Puntaje, R.FechaEjecucion
FROM Resultados R
JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio
JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje;

-- 9. Vista que muestra los permisos asignados a cada candidato.
CREATE VIEW VistaPermisosCandidatos AS
SELECT C.Nombre AS NombreCandidato, R.Nombre AS Rol, P.Nombre AS Permiso
FROM Candidatos C
JOIN CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
JOIN Roles R ON CR.IdRol = R.IdRol
JOIN RolesPermisos RP ON R.IdRol = RP.IdRol
JOIN Permisos P ON RP.IdPermiso = P.IdPermiso;


SECCIÓN 3:
-- 10. Procedimiento para registrar resultados y auditoría.
CREATE PROCEDURE RegistrarResultado
    @IdCandidato INT,
    @IdEjercicio INT,
    @Puntaje INT
AS
BEGIN
    -- Si la tabla de logs no existe, créala
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='LogsResultados' AND xtype='U')
    BEGIN
        CREATE TABLE LogsResultados (
            IdLog INT PRIMARY KEY IDENTITY(1,1),
            IdCandidato INT,
            IdEjercicio INT,
            Puntaje INT,
            FechaRegistro DATETIME DEFAULT GETDATE()
        );
    END

    -- Iniciar transacción
    BEGIN TRANSACTION;

    -- Insertar en Resultados
    INSERT INTO Resultados (IdCandidato, IdEjercicio, Puntaje, FechaEjecucion)
    VALUES (@IdCandidato, @IdEjercicio, @Puntaje, GETDATE());

    -- Insertar en LogsResultados
    INSERT INTO LogsResultados (IdCandidato, IdEjercicio, Puntaje)
    VALUES (@IdCandidato, @IdEjercicio, @Puntaje);

    -- Confirmar transacción
    COMMIT TRANSACTION;
END;

/*CCIÓN 4: BONUS*/
-- 12. Vista que muestra cantidad de veces que se resolvió cada ejercicio.
CREATE VIEW VistaEjerciciosResueltos AS
SELECT E.Titulo, COUNT(R.IdEjercicio) AS VecesResuelto
FROM Ejercicios E
LEFT JOIN Resultados R ON E.IdEjercicio = R.IdEjercicio
GROUP BY E.Titulo;

-- 13. SP para asignar un rol a un candidato, si no existe.
CREATE PROCEDURE AsignarRolACandidato
    @IdCandidato INT,
    @IdRol INT
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM CandidatosRoles WHERE IdCandidato = @IdCandidato AND IdRol = @IdRol)
    BEGIN
        INSERT INTO CandidatosRoles (IdCandidato, IdRol)
        VALUES (@IdCandidato, @IdRol);
    END
END;

-- 14. SP que retorna todos los permisos que tiene un rol dado.
CREATE PROCEDURE ObtenerPermisosPorRol
    @NombreRol VARCHAR(50)
AS
BEGIN
    SELECT P.Nombre AS Permiso
    FROM Permisos P
    JOIN RolesPermisos RP ON P.IdPermiso = RP.IdPermiso
    JOIN Roles R ON RP.IdRol = R.IdRol
    WHERE R.Nombre = @NombreRol;
END;
