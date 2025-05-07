--Pruebas para ver si funciona el SP de Registrar Resultado

EXEC RegistrarResultado @IdCandidato = 1, @IdEjercicio = 2, @Puntaje = 61;

--
SELECT * FROM Resultados WHERE IdCandidato = 1 AND IdEjercicio = 2;

--
SELECT * FROM LogsResultados WHERE IdCandidato = 1 AND IdEjercicio = 2;
