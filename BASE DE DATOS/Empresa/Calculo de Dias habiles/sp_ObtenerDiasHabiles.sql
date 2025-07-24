Use DBProyectoGrupalDojoGeko
GO
IF OBJECT_ID('sp_ObtenerDiasHabiles', 'P') IS NOT NULL
    DROP PROCEDURE sp_ObtenerDiasHabiles;
GO


CREATE PROCEDURE sp_ObtenerDiasHabiles
    @FechaInicio DATE,
    @FechaFin DATE,
    @TotalDiasHabiles DECIMAL(5,2) OUTPUT,
    @MensajeSalida NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
	SET @MensajeSalida = '';
	-- Validar que el rango no exceda un a�o (366 d�as por a�o bisiesto)
    IF DATEDIFF(DAY, @FechaInicio, @FechaFin) > 366
    BEGIN
        SET @TotalDiasHabiles = NULL;
        SET @MensajeSalida = 'El rango de fechas no puede exceder un a�o. Por favor, seleccione un rango menor.';
        RETURN;
    END;

	IF (@FechaInicio >= @FechaFin) 
    BEGIN
        SET @TotalDiasHabiles = NULL;
        SET @MensajeSalida = 'La fecha de inicio tiene que ser menor a la fecha fin.';
        RETURN;
    END;
    -- Tabla de d�as dentro del rango
    ;WITH Dias AS (
        SELECT @FechaInicio AS Fecha
        UNION ALL
        SELECT DATEADD(DAY, 1, Fecha)
        FROM Dias
        WHERE Fecha < @FechaFin
    ),
    DiasHabiles AS (
        SELECT Fecha
        FROM Dias
        WHERE DATENAME(WEEKDAY, Fecha) NOT IN ('Saturday', 'Sunday')
    ),
    Feriados AS (
        SELECT 
            D.Fecha,
            ISNULL(FV.ProporcionDia, ISNULL(FF.ProporcionDia, 0)) AS ProporcionDia
        FROM DiasHabiles D
        LEFT JOIN DiasFestivosVariables FV ON D.Fecha = FV.Fecha
        LEFT JOIN DiasFestivosFijos FF 
            ON DAY(D.Fecha) = FF.Dia AND MONTH(D.Fecha) = FF.Mes
    )
    SELECT @TotalDiasHabiles = SUM(1 - ProporcionDia)
    FROM Feriados
	OPTION (MAXRECURSION 1000);--hasta 3 a�os laborales

	-- operaci�n fue exitosa
    SET @MensajeSalida = 'C�lculo exitoso.';
END;
GO