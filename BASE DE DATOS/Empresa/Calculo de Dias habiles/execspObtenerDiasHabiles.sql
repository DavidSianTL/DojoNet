Use DBProyectoGrupalDojoGeko
GO
DECLARE @DiasHabiles DECIMAL(5,2);
DECLARE @Mensaje NVARCHAR(200);

EXEC sp_ObtenerDiasHabiles 
    @FechaInicio = '2025-01-01', 
    @FechaFin = '2025-12-31',
    @TotalDiasHabiles = @DiasHabiles OUTPUT,
    @MensajeSalida = @Mensaje OUTPUT;

SELECT @DiasHabiles AS DiasHabiles, @Mensaje AS Mensaje;

