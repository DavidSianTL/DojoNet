
use DBProyectoGrupalDojoGeko
GO
DECLARE @Dias INT;

EXEC sp_calculardiasVacacionesDisponibles 
	@FechaIngreso = '2020-01-01',
	@DiasDisponibles = @Dias OUTPUT;
select @Dias as DiasVacacionesDisponibles;