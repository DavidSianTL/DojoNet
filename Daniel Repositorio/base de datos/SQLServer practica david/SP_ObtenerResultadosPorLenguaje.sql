--11. Crear un SP llamado ObtenerResultadosPorLenguaje

CREATE PROCEDURE ObtenerResultadosPorLenguaje
    @NombreLenguaje NVARCHAR(100)
AS
BEGIN
    SELECT  C.Nombre AS NombreCandidato, E.Titulo AS TituloEjercicio, L.NombreLenguaje, R.Puntaje, R.FechaEjecucion
    FROM Resultados R
    JOIN Candidatos C ON R.IdCandidato = C.IdCandidato
    JOIN Ejercicios E ON R.IdEjercicio = E.IdEjercicio
    JOIN Lenguajes L ON E.IdLenguaje = L.IdLenguaje
    WHERE L.NombreLenguaje = @NombreLenguaje;
END;
