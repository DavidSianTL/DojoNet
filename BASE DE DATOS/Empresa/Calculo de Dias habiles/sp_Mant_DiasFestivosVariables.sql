Use DBProyectoGrupalDojoGeko
GO
IF OBJECT_ID('sp_Mant_DiasFestivosVariables', 'P') IS NOT NULL
    DROP PROCEDURE sp_Mant_DiasFestivosVariables;
GO

CREATE PROCEDURE sp_Mant_DiasFestivosVariables
    @i_op_operacion CHAR(1),  -- 'I' = Insertar, 'A' = Actualizar, 'D' = Eliminar
    @Fecha DATE,
    @Descripcion NVARCHAR(100) = NULL,
    @TipoFeriadoId INT = NULL,
    @ProporcionDia DECIMAL(3,2) = 1.00,
    @Usr_creacion NVARCHAR(25) = NULL,
    @Usr_modifica NVARCHAR(25) = NULL,
    @MensajeSalida NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @MensajeSalida = '';

    IF @i_op_operacion = 'I'
    BEGIN
        IF EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Fecha)
        BEGIN
            SET @MensajeSalida = 'Ya existe un feriado en esa fecha.';
            RETURN;
        END

        INSERT INTO DiasFestivosVariables (
            Fecha, Descripcion, TipoFeriadoId,
            ProporcionDia, Usr_creacion, Fec_creacion
        )
        VALUES (
            @Fecha, @Descripcion, @TipoFeriadoId,
            @ProporcionDia, @Usr_creacion, GETDATE()
        );

        SET @MensajeSalida = 'Feriado variable insertado correctamente.';
        RETURN;
    END

    ELSE IF @i_op_operacion = 'A'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Fecha)
        BEGIN
            SET @MensajeSalida = 'El feriado variable a actualizar no existe.';
            RETURN;
        END

        UPDATE DiasFestivosVariables
        SET Descripcion = @Descripcion,
            TipoFeriadoId = @TipoFeriadoId,
            ProporcionDia = @ProporcionDia,
            Usr_modifica = @Usr_modifica,
            Fec_modifica = GETDATE()
        WHERE Fecha = @Fecha;

        SET @MensajeSalida = 'Feriado variable actualizado correctamente.';
        RETURN;
    END

    ELSE IF @i_op_operacion = 'D'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Fecha)
        BEGIN
            SET @MensajeSalida = 'El feriado variable a eliminar no existe.';
            RETURN;
        END

        DELETE FROM DiasFestivosVariables
        WHERE Fecha = @Fecha;

        SET @MensajeSalida = 'Feriado variable eliminado correctamente.';
        RETURN;
    END

    ELSE
    BEGIN
        SET @MensajeSalida = 'Operación no válida. Use I, A o D.';
    END
END;
GO