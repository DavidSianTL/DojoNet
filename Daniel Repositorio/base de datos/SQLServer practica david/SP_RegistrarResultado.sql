CREATE PROCEDURE RegistrarResultado
    @IdCandidato INT,
    @IdEjercicio INT,
    @Puntaje INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Resultados (IdCandidato, IdEjercicio, FechaEjecucion, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, GETDATE(), @Puntaje);

        INSERT INTO LogsResultados (IdCandidato, IdEjercicio, Puntaje)
        VALUES (@IdCandidato, @IdEjercicio, @Puntaje);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; 
    END CATCH
END;
