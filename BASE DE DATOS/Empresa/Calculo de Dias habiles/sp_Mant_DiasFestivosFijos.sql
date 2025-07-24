Use DBProyectoGrupalDojoGeko
GO
IF OBJECT_ID('sp_Mant_DiasFestivosFijos', 'P') IS NOT NULL
    DROP PROCEDURE sp_Mant_DiasFestivosFijos;
GO

CREATE PROCEDURE sp_Mant_DiasFestivosFijos
    @i_op_operacion CHAR(1),  -- 'I' = Insertar, 'A' = Actualizar, 'D' = Eliminar
    @Dia INT,
    @Mes INT,
    @TipoFeriadoId INT,
    @Descripcion NVARCHAR(100) = NULL,
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
        IF EXISTS (SELECT 1 FROM DiasFestivosFijos WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId)
        BEGIN
            SET @MensajeSalida = 'Ya existe un feriado con ese día, mes y tipo.';
            RETURN;
        END

        INSERT INTO DiasFestivosFijos (
            Dia, Mes, TipoFeriadoId, Descripcion,
            ProporcionDia, Usr_creacion, Fec_creacion
        )
        VALUES (
            @Dia, @Mes, @TipoFeriadoId, @Descripcion,
            @ProporcionDia, @Usr_creacion, GETDATE()
        );

        SET @MensajeSalida = 'Feriado insertado correctamente.';
        RETURN;
    END

    ELSE IF @i_op_operacion = 'A'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosFijos WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId)
        BEGIN
            SET @MensajeSalida = 'El feriado a actualizar no existe.';
            RETURN;
        END

        UPDATE DiasFestivosFijos
        SET Descripcion = @Descripcion,
            ProporcionDia = @ProporcionDia,
            Usr_modifica = @Usr_modifica,
            Fec_modifica = GETDATE()
        WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId;

        SET @MensajeSalida = 'Feriado actualizado correctamente.';
        RETURN;
    END

    ELSE IF @i_op_operacion = 'D'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosFijos WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId)
        BEGIN
            SET @MensajeSalida = 'El feriado a eliminar no existe.';
            RETURN;
        END

        DELETE FROM DiasFestivosFijos
        WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId;

        SET @MensajeSalida = 'Feriado eliminado correctamente.';
        RETURN;
    END

    ELSE
    BEGIN
        SET @MensajeSalida = 'Operación no válida. Use I, A o D.';
    END
END;
GO