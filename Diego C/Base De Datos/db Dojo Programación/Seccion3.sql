USE DojoProgramacion;
GO


CREATE OR ALTER PROCEDURE RegistrarResultado
    @IdCandidato INT,
    @IdEjercicio INT,
    @Puntaje INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, GETDATE(), @Puntaje);

        INSERT INTO LogsResultados (IdCandidato, IdEjercicio, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, @Puntaje);

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        PRINT 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO
EXEC RegistrarResultado
    @IdCandidato = 1,
    @IdEjercicio = 2,
    @Puntaje = 92;

	SELECT * FROM Resultados
WHERE IdCandidato = 1 AND IdEjercicio = 2;
GO

SELECT * FROM LogsResultados
WHERE IdCandidato = 1 AND IdEjercicio = 2;

