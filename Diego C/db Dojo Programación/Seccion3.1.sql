CREATE OR ALTER PROCEDURE ObtenerResultadosPorLenguaje
    @NombreLenguaje NVARCHAR(100)
AS
BEGIN
    SELECT 
        c.Nombre AS NombreCandidato,
        e.Titulo AS Ejercicio,
        l.NombreLenguaje,
        r.Puntaje,
        r.FechaEjecucion
    FROM Resultados r
    INNER JOIN Candidatos c ON r.IdCandidato = c.IdCandidato
    INNER JOIN Ejercicios e ON r.IdEjercicio = e.IdEjercicio
    INNER JOIN Lenguajes l ON e.IdLenguaje = l.IdLenguaje
    WHERE l.NombreLenguaje = @NombreLenguaje;
END;
GO

EXEC ObtenerResultadosPorLenguaje @NombreLenguaje = 'C#';