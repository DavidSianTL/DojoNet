Use DBProyectoGrupalDojoGeko
GO
IF OBJECT_ID('sp_InsertarSemanaSanta', 'P') IS NOT NULL
    DROP PROCEDURE sp_InsertarSemanaSanta;
GO


CREATE PROCEDURE sp_InsertarSemanaSanta
    @AnioInicio INT,
    @AnioFin INT,
    @Usr NVARCHAR(25) = 'admin',
    @TipoFeriadoId INT = 3
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Anio INT = @AnioInicio;
    DECLARE @Now DATETIME = GETDATE();

    WHILE @Anio <= @AnioFin
    BEGIN
        DECLARE @DomingoPascoa DATE = dbo.fnCalcularPascua(@Anio);
        DECLARE @Miercoles DATE = DATEADD(DAY, -4, @DomingoPascoa);
        DECLARE @Jueves DATE = DATEADD(DAY, -3, @DomingoPascoa);
        DECLARE @Viernes DATE = DATEADD(DAY, -2, @DomingoPascoa);
        DECLARE @Sabado DATE = DATEADD(DAY, -1, @DomingoPascoa);
        DECLARE @Domingo DATE = @DomingoPascoa;

        -- Miércoles Santo (medio día)
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Miercoles)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Miercoles, CONCAT('Miércoles Santo ', @Anio), @TipoFeriadoId, 0.50, @Usr, @Now);
        END

        -- Jueves Santo
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Jueves)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Jueves, CONCAT('Jueves Santo ', @Anio), @TipoFeriadoId, 1.00, @Usr, @Now);
        END

        -- Viernes Santo
        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Viernes)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Viernes, CONCAT('Viernes Santo ', @Anio), @TipoFeriadoId, 1.00, @Usr, @Now);
        END

      

      

        SET @Anio += 1;
    END
END;