
use DBProyectoGrupalDojoGeko
GO

IF OBJECT_ID('sp_calculardiasVacacionesDisponibles', 'P') IS NOT NULL
    DROP PROCEDURE sp_calculardiasVacacionesDisponibles;
GO

CREATE PROCEDURE sp_calculardiasVacacionesDisponibles
	@FechaIngreso DATE,
	@IdEmpleado INT,
	@DiasDisponibles INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE
	@FechaActual DATE = GETDATE(),
	@AniosCompletos INT = 0,
	@MesesRestantes INT = 0,
	@DiasporAnio INT = 15,
	@DiasProporcionales DECIMAL(5,2);
	print @FechaActual;
	SET @AniosCompletos = DATEDIFF(YEAR, @FechaIngreso, @FechaActual)
	 - CASE WHEN 
		(MONTH(@FechaIngreso) > MONTH(@FechaActual) OR 
			(MONTH(@FechaIngreso) = MONTH(@FechaActual) 
				AND (DAY(@FechaIngreso) > DAY(@FechaActual))))  THEN 1 ELSE 0 END;
   print @AniosCompletos;

	SET @MesesRestantes = DATEDIFF(MONTH, DATEADD(YEAR, @AniosCompletos, @FechaIngreso),@FechaActual);
	print  @MesesRestantes;
	IF (@MesesRestantes < 0) SET @MesesRestantes = 0;

	SET @DiasProporcionales = (@MesesRestantes * @DiasporAnio)/12.0;
	print  @DiasProporcionales;
	SET @DiasDisponibles = FLOOR((@AniosCompletos * @DiasporAnio) +@DiasProporcionales);
	print @DiasDisponibles;
END;

