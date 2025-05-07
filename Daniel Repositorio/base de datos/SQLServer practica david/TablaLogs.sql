-- creacion de tabla logs
    CREATE TABLE LogsResultados (
        IdLog INT PRIMARY KEY IDENTITY,
        IdCandidato INT,
        IdEjercicio INT,
        Puntaje INT,
        FechaRegistro DATETIME DEFAULT GETDATE()
    );