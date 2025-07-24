DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosFijos
    @i_op_operacion = 'I',
    @Dia = 24, @Mes = 12, @TipoFeriadoId = 1,
    @Descripcion = 'Nochebuena (medio día)',
    @ProporcionDia = 0.5,
    @Usr_creacion = 'admin', 
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;


DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosFijos
    @i_op_operacion = 'A',
    @Dia = 24, @Mes = 12, @TipoFeriadoId = 1,
    @Descripcion = 'Nochebuena Actualizada',
    @ProporcionDia = 0.5,
    @Usr_modifica = 'admin', 
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;


DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosFijos
    @i_op_operacion = 'D',
    @Dia = 24, @Mes = 12, @TipoFeriadoId = 1,
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;
