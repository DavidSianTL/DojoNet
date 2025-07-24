DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosVariables
    @i_op_operacion = 'I',
    @Fecha = '2024-03-27',
    @Descripcion = 'Mi�rcoles Santo (medio d�a)',
    @TipoFeriadoId = 2,
    @ProporcionDia = 0.5,
    @Usr_creacion = 'admin',
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;


DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosVariables
    @i_op_operacion = 'A',
    @Fecha = '2024-03-27',
    @Descripcion = 'Mi�rcoles Santo Actualizado',
    @TipoFeriadoId = 2,
    @ProporcionDia = 0.5,
    @Usr_modifica = 'admin',
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;

DECLARE @msg NVARCHAR(200);
EXEC sp_Mant_DiasFestivosVariables
    @i_op_operacion = 'D',
    @Fecha = '2024-03-27',
    @MensajeSalida = @msg OUTPUT;
SELECT @msg;