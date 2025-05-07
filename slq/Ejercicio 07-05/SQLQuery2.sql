--Obtener todos los candidatos con su email.
select * from Candidatos

-----Listar todos los ejercicios con su categoría y lenguaje de programación.

SELECT 
    E.IdEjercicio,
    E.Titulo,
    C.NombreCategoria,
    L.NombreLenguaje
FROM 
    Ejercicios E
INNER JOIN 
    Categorias C ON E.IdCategoria = C.IdCategoria
INNER JOIN 
    Lenguajes L ON E.IdLenguaje = L.IdLenguaje;

--	Obtener los resultados de cada candidato con el título del ejercicio y la fecha de
--ejecución.


SELECT 
    C.Nombre AS NombreCandidato,
    E.Titulo AS TituloEjercicio,
    R.FechaEjecucion
FROM 
    Resultados R
INNER JOIN 
    Candidatos C ON R.IdCandidato = C.IdCandidato
INNER JOIN 
    Ejercicios E ON R.IdEjercicio = E.IdEjercicio;



	--Listar todos los candidatos que hayan sacado más de 85 puntos en cualquier ejercicio.

	SELECT DISTINCT c.IdCandidato, c.Nombre, c.Email
FROM Candidatos c
JOIN Resultados r ON c.IdCandidato = r.IdCandidato
WHERE r.Puntaje > 85;

	

	---Obtener el promedio de puntajes por candidato.
SELECT 
    C.Nombre AS NombreCandidato,
    AVG(R.Puntaje) AS PromedioPuntaje
FROM 
    Resultados R
INNER JOIN 
    Candidatos C ON R.IdCandidato = C.IdCandidato
GROUP BY 
    C.Nombre;

	--Listar los permisos que tiene asignado el candidato con nombre 'Ana Torres'.

SELECT 
    P.NombrePermiso
FROM 
    Candidatos C
INNER JOIN 
    CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
INNER JOIN 
    RolesPermisos RP ON CR.IdRol = RP.IdRol
INNER JOIN 
    Permisos P ON RP.IdPermiso = P.IdPermiso
WHERE 
    C.Nombre = 'Ana Torres';


	--Listar los ejercicios que no han sido resueltos por ningún candidato.
	SELECT 
    E.IdEjercicio,
    E.Titulo
FROM 
    Ejercicios E
LEFT JOIN 
    Resultados R ON E.IdEjercicio = R.IdEjercicio
WHERE 
    R.IdEjercicio IS NULL;



--	SECCION 2: CREACION DE VISTAS
--8. Crear una vista llamada VistaResultadosDetallados

CREATE VIEW VistaResultadosDetallados AS
SELECT 
    C.Nombre AS NombreCandidato,
    E.Titulo AS TituloEjercicio,
    L.NombreLenguaje AS LenguajeProgramacion,
    R.Puntaje,
    R.FechaEjecucion
FROM 
    Resultados R
INNER JOIN 
    Candidatos C ON R.IdCandidato = C.IdCandidato
INNER JOIN 
    Ejercicios E ON R.IdEjercicio = E.IdEjercicio
INNER JOIN 
    Lenguajes L ON E.IdLenguaje = L.IdLenguaje;


	SELECT * FROM VistaResultadosDetallados;


--9. Crear una vista llamada VistaPermisosCandidatos que muestre: Nombre del Candidato,
--Rol, Permiso

	CREATE VIEW VistaPermisosCandidatos AS
SELECT 
    C.Nombre AS NombreCandidato,
    R.NombreRol AS Rol,
    P.NombrePermiso AS Permiso
FROM 
    Candidatos C
INNER JOIN 
    CandidatosRoles CR ON C.IdCandidato = CR.IdCandidato
INNER JOIN 
    Roles R ON CR.IdRol = R.IdRol
INNER JOIN 
    RolesPermisos RP ON R.IdRol = RP.IdRol
INNER JOIN 
    Permisos P ON RP.IdPermiso = P.IdPermiso;


	SELECT * FROM VistaPermisosCandidatos;


	--10. Crear un procedimiento almacenado llamado RegistrarResultado
	IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'LogsResultados') AND type in (N'U')
)
BEGIN
    CREATE TABLE LogsResultados (
        IdLog INT IDENTITY PRIMARY KEY,
        IdCandidato INT,
        IdEjercicio INT,
        Puntaje INT,
        FechaRegistro DATETIME DEFAULT GETDATE()
    );
END;





CREATE PROCEDURE RegistrarResultado
    @IdCandidato INT,
    @IdEjercicio INT,
    @Puntaje INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar en Resultados
        INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, GETDATE(), @Puntaje);

        -- Insertar en LogsResultados
        INSERT INTO LogsResultados (IdCandidato, IdEjercicio, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, @Puntaje);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;




CREATE PROCEDURE ObtenerResultadosPorLenguaje
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        L.NombreLenguaje,
        C.Nombre AS NombreCandidato,
        E.Titulo AS TituloEjercicio,
        R.Puntaje,
        R.FechaEjecucion
    FROM 
        Resultados R
    INNER JOIN 
        Candidatos C ON R.IdCandidato = C.IdCandidato
    INNER JOIN 
        Ejercicios E ON R.IdEjercicio = E.IdEjercicio
    INNER JOIN 
        Lenguajes L ON E.IdLenguaje = L.IdLenguaje
    ORDER BY 
        L.NombreLenguaje, C.Nombre;
END;


--12. Crear una vista que liste todos los ejercicios junto con la cantidad de veces que han
--sido resueltos.
CREATE VIEW VistaEjerciciosResueltos AS
SELECT 
    E.IdEjercicio,
    E.Titulo AS TituloEjercicio,
    COUNT(R.IdResultado) AS VecesResuelto
FROM 
    Ejercicios E
LEFT JOIN 
    Resultados R ON E.IdEjercicio = R.IdEjercicio
GROUP BY 
    E.IdEjercicio, E.Titulo;



	SELECT * FROM VistaEjerciciosResueltos;



	--13. Crear un SP que permita asignar un rol a un candidato, si no existe ya la relación.
	CREATE PROCEDURE AsignarRolACandidato
    @IdCandidato INT,
    @IdRol INT
AS
BEGIN
    SET NOCOUNT ON;

    
    IF NOT EXISTS (
        SELECT 1
        FROM CandidatosRoles CR
        WHERE CR.IdCandidato = @IdCandidato AND CR.IdRol = @IdRol
    )
    BEGIN
       
        INSERT INTO CandidatosRoles (IdCandidato, IdRol)
        VALUES (@IdCandidato, @IdRol);
    END
    ELSE
    BEGIN
       
        PRINT 'La relación ya existe. El rol ya está asignado al candidato.';
    END
END;



EXEC AsignarRolACandidato @IdCandidato = 5, @IdRol = 2;




--14. Crear un SP que retorne todos los permisos que tiene un rol dado (por nombre).

CREATE PROCEDURE ObtenerPermisosPorRol
    @NombreRol NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

   
    SELECT 
        P.NombrePermiso
    FROM 
        Roles R
    INNER JOIN 
        RolesPermisos RP ON R.IdRol = RP.IdRol
    INNER JOIN 
        Permisos P ON RP.IdPermiso = P.IdPermiso
    WHERE 
        R.NombreRol = @NombreRol;
END;



EXEC ObtenerPermisosPorRol @NombreRol = 'Administrador';






SELECT * 
FROM sys.objects
WHERE type = 'P' AND name = 'ObtenerResultadosPorLenguaje';
