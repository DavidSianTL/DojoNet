-- =================================================================
-- MÓDULO DE CÁLCULO DE DÍAS HÁBILES Y GESTIÓN DE FERIADOS
-- =================================================================

-- 1. Tabla para Tipos de Feriado
-- =================================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TipoFeriado' and xtype='U')
BEGIN
    CREATE TABLE TipoFeriado (
        TipoFeriadoId INT PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL,         -- Ej: 'Nacional', 'Bancario', 'Religioso'
        Descripcion NVARCHAR(200),
        Usr_creacion NVARCHAR(25) NOT NULL,
        Fec_creacion DATETIME NOT NULL,
        Usr_modifica NVARCHAR(25),
        Fec_modifica DATETIME
    );

    INSERT INTO TipoFeriado (TipoFeriadoId, Nombre, Descripcion, Usr_creacion, Fec_creacion)
    VALUES 
    (1, 'Nacional', 'Aplica a todo el país', 'admin', GETDATE()),
    (2, 'Bancario', 'Solo para entidades financieras', 'admin', GETDATE()),
    (3, 'Religioso', 'Celebraciones religiosas no obligatorias', 'admin', GETDATE());
END
GO

-- 2. Tabla para Días Festivos Fijos
-- =================================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DiasFestivosFijos' and xtype='U')
BEGIN
    CREATE TABLE DiasFestivosFijos (
        Dia INT NOT NULL CHECK (Dia BETWEEN 1 AND 31),
        Mes INT NOT NULL CHECK (Mes BETWEEN 1 AND 12),
        Descripcion NVARCHAR(100),
        TipoFeriadoId INT NOT NULL,
        ProporcionDia DECIMAL(3,2) NOT NULL DEFAULT 1.00,
        Usr_creacion NVARCHAR(25) NOT NULL,
        Fec_creacion DATETIME NOT NULL,
        Usr_modifica NVARCHAR(25),
        Fec_modifica DATETIME,
        CONSTRAINT PK_DiasFestivosFijos PRIMARY KEY (Dia, Mes, TipoFeriadoId),
        CONSTRAINT FK_DiasFestivosFijos_TipoFeriado 
            FOREIGN KEY (TipoFeriadoId) REFERENCES TipoFeriado (TipoFeriadoId)
    );

    INSERT INTO DiasFestivosFijos (Dia, Mes, TipoFeriadoId,ProporcionDia, Descripcion,Usr_creacion, Fec_creacion) VALUES
    (1, 1, 1, 1.0, 'Año Nuevo','admin',GETDATE()),
    (1, 5, 1, 1.0, 'Día del Trabajo','admin',GETDATE()),
    (30, 6, 1, 1.0, 'Dia del Ejercito','admin',GETDATE()),
    (1, 7, 2, 1.0, 'Dia del Empleado Bancario','admin',GETDATE()),
    (15, 8, 3, 1.0, 'Dia de la Virgen de la Asunción','admin',GETDATE()),
    (15, 9, 1, 1.0, 'Dia de la independencia','admin',GETDATE()),
    (12, 10, 2, 1.0, 'Dia de la Raza','admin',GETDATE()),
    (20, 10, 1, 1.0, 'Dia de la Revolucion','Admin',GETDATE()),
    (1, 11, 3, 1.0, 'Dia de los Santos','admin',GETDATE()),
    (24, 12, 1, 0.5, 'Noche buena','admin',GETDATE()),
    (25, 12, 1, 1.0, 'Navidad','admin',GETDATE()),
    (31, 12, 1, 0.5, 'Fin de año','admin',GETDATE());
END
GO

-- 3. Tabla para Días Festivos Variables
-- =================================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DiasFestivosVariables' and xtype='U')
BEGIN
    CREATE TABLE DiasFestivosVariables (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Fecha DATE NOT NULL UNIQUE,
        Descripcion NVARCHAR(100),
        TipoFeriadoId INT NOT NULL,
        ProporcionDia DECIMAL(3,2) NOT NULL DEFAULT 1.00,
        Usr_creacion NVARCHAR(25) NOT NULL,
        Fec_creacion DATETIME NOT NULL,
        Usr_modifica NVARCHAR(25),
        Fec_modifica DATETIME,
        CONSTRAINT FK_DiasFestivosVariables_TipoFeriado 
            FOREIGN KEY (TipoFeriadoId) REFERENCES TipoFeriado (TipoFeriadoId)
    );
END
GO

-- 4. Función para Calcular la Pascua
-- =================================================================
IF OBJECT_ID('dbo.fnCalcularPascua', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fnCalcularPascua;
GO

CREATE FUNCTION dbo.fnCalcularPascua (@Anio INT)
RETURNS DATE
AS
BEGIN
    DECLARE @a INT = @Anio % 19
    DECLARE @b INT = @Anio / 100
    DECLARE @c INT = @Anio % 100
    DECLARE @d INT = @b / 4
    DECLARE @e INT = @b % 4
    DECLARE @f INT = (@b + 8) / 25
    DECLARE @g INT = (@b - @f + 1) / 3
    DECLARE @h INT = (19 * @a + @b - @d - @g + 15) % 30
    DECLARE @i INT = @c / 4
    DECLARE @k INT = @c % 4
    DECLARE @l INT = (32 + 2 * @e + 2 * @i - @h - @k) % 7
    DECLARE @m INT = (@a + 11 * @h + 22 * @l) / 451
    DECLARE @mes INT = (@h + @l - 7 * @m + 114) / 31
    DECLARE @dia INT = ((@h + @l - 7 * @m + 114) % 31) + 1

    RETURN CAST(CONCAT(@Anio, '-', @mes, '-', @dia) AS DATE)
END
GO

-- 5. SP para Mantenimiento de Días Festivos Fijos 
-- =================================================================
IF OBJECT_ID('sp_Mant_DiasFestivosFijos', 'P') IS NOT NULL
    DROP PROCEDURE sp_Mant_DiasFestivosFijos;
GO

CREATE PROCEDURE sp_Mant_DiasFestivosFijos
    @i_op_operacion CHAR(1),  -- 'I' = Insertar, 'A' = Actualizar, 'E' = Eliminar
    @Dia INT,
    @Mes INT,
    @TipoFeriadoId INT,
    @Descripcion NVARCHAR(100) = NULL,
    @ProporcionDia DECIMAL(3,2) = 1.00,
    @Usr_creacion NVARCHAR(25) = NULL,
    @Usr_modifica NVARCHAR(25) = NULL,
    -- INICIO DE CAMBIOS: Parámetros para la clave original en actualizaciones
    @Original_Dia INT = NULL,
    @Original_Mes INT = NULL,
    @Original_TipoFeriadoId INT = NULL,
    -- FIN DE CAMBIOS
    @MensajeSalida NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @MensajeSalida = '';

    IF @i_op_operacion = 'I'
    BEGIN
        IF EXISTS (SELECT 1 FROM DiasFestivosFijos WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId)
        BEGIN
            SET @MensajeSalida = 'Error: Ya existe un feriado con ese día, mes y tipo.';
            RETURN;
        END

        INSERT INTO DiasFestivosFijos (Dia, Mes, TipoFeriadoId, Descripcion, ProporcionDia, Usr_creacion, Fec_creacion)
        VALUES (@Dia, @Mes, @TipoFeriadoId, @Descripcion, @ProporcionDia, @Usr_creacion, GETDATE());

        SET @MensajeSalida = 'Feriado insertado correctamente.';
    END
    ELSE IF @i_op_operacion = 'A'
    BEGIN
        -- INICIO DE CAMBIOS: Lógica de actualización mejorada
        IF @Original_Dia IS NULL OR @Original_Mes IS NULL OR @Original_TipoFeriadoId IS NULL
        BEGIN
            SET @MensajeSalida = 'Error: Para actualizar, se requieren los valores originales de la clave.';
            RETURN;
        END

        -- Validar que la nueva clave no exista (si es que se cambió)
        IF (@Dia != @Original_Dia OR @Mes != @Original_Mes OR @TipoFeriadoId != @Original_TipoFeriadoId) AND EXISTS(SELECT 1 FROM DiasFestivosFijos WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId)
        BEGIN
            SET @MensajeSalida = 'Error: La nueva combinación de día, mes y tipo ya existe para otro feriado.';
            RETURN;
        END

        UPDATE DiasFestivosFijos
        SET 
            Dia = @Dia,
            Mes = @Mes,
            TipoFeriadoId = @TipoFeriadoId,
            Descripcion = @Descripcion, 
            ProporcionDia = @ProporcionDia, 
            Usr_modifica = @Usr_modifica, 
            Fec_modifica = GETDATE()
        WHERE Dia = @Original_Dia AND Mes = @Original_Mes AND TipoFeriadoId = @Original_TipoFeriadoId;
        
        IF @@ROWCOUNT > 0
            SET @MensajeSalida = 'Feriado actualizado correctamente.'
        ELSE
            SET @MensajeSalida = 'Error: No se encontró el feriado original para actualizar.'
        -- FIN DE CAMBIOS
    END
    ELSE IF @i_op_operacion = 'E' -- Cambiado de 'D' a 'E' para consistencia con el proyecto
    BEGIN
        DELETE FROM DiasFestivosFijos
        WHERE Dia = @Dia AND Mes = @Mes AND TipoFeriadoId = @TipoFeriadoId;
        SET @MensajeSalida = 'Feriado eliminado correctamente.';
    END
    ELSE
    BEGIN
        SET @MensajeSalida = 'Operación no válida. Use I, A o E.';
    END
END;
GO

-- 6. SP para Mantenimiento de Días Festivos Variables
-- =================================================================
IF OBJECT_ID('sp_Mant_DiasFestivosVariables', 'P') IS NOT NULL
    DROP PROCEDURE sp_Mant_DiasFestivosVariables;
GO

CREATE PROCEDURE sp_Mant_DiasFestivosVariables
    @i_op_operacion CHAR(1),  -- 'I' = Insertar, 'A' = Actualizar, 'E' = Eliminar
    @Id INT = NULL, -- Se usa para Actualizar y Eliminar
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
            SET @MensajeSalida = 'Error: Ya existe un feriado en esa fecha.';
            RETURN;
        END

        INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
        VALUES (@Fecha, @Descripcion, @TipoFeriadoId, @ProporcionDia, @Usr_creacion, GETDATE());
        SET @MensajeSalida = 'Feriado variable insertado correctamente.';
    END
    ELSE IF @i_op_operacion = 'A'
    BEGIN
        IF @Id IS NULL
        BEGIN
            SET @MensajeSalida = 'Error: Se requiere el ID para actualizar un feriado variable.';
            RETURN;
        END

        -- Validar que la nueva fecha no exista en otro registro
        IF EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Fecha AND Id != @Id)
        BEGIN
            SET @MensajeSalida = 'Error: La nueva fecha ya está asignada a otro feriado.';
            RETURN;
        END

        UPDATE DiasFestivosVariables
        SET Fecha = @Fecha, Descripcion = @Descripcion, TipoFeriadoId = @TipoFeriadoId, ProporcionDia = @ProporcionDia, Usr_modifica = @Usr_modifica, Fec_modifica = GETDATE()
        WHERE Id = @Id;

        IF @@ROWCOUNT > 0
            SET @MensajeSalida = 'Feriado variable actualizado correctamente.'
        ELSE
            SET @MensajeSalida = 'Error: No se encontró el feriado variable para actualizar.'
    END
    ELSE IF @i_op_operacion = 'E' -- Cambiado de 'D' a 'E'
    BEGIN
         IF @Id IS NULL
        BEGIN
            SET @MensajeSalida = 'Error: Se requiere el ID para eliminar un feriado variable.';
            RETURN;
        END

        DELETE FROM DiasFestivosVariables
        WHERE Id = @Id;
        SET @MensajeSalida = 'Feriado variable eliminado correctamente.';
    END
    ELSE
    BEGIN
        SET @MensajeSalida = 'Operación no válida. Use I, A o E.';
    END
END;
GO

-- 7. SP para Insertar Feriados de Semana Santa
-- =================================================================
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

        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Miercoles)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Miercoles, CONCAT('Miércoles Santo ', @Anio), @TipoFeriadoId, 0.50, @Usr, @Now);
        END

        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Jueves)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Jueves, CONCAT('Jueves Santo ', @Anio), @TipoFeriadoId, 1.00, @Usr, @Now);
        END

        IF NOT EXISTS (SELECT 1 FROM DiasFestivosVariables WHERE Fecha = @Viernes)
        BEGIN
            INSERT INTO DiasFestivosVariables (Fecha, Descripcion, TipoFeriadoId, ProporcionDia, Usr_creacion, Fec_creacion)
            VALUES (@Viernes, CONCAT('Viernes Santo ', @Anio), @TipoFeriadoId, 1.00, @Usr, @Now);
        END

        SET @Anio += 1;
    END
END;
GO

-- 8. SP para Obtener Días Hábiles
-- =================================================================
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

    IF @FechaInicio >= @FechaFin 
    BEGIN
        SET @TotalDiasHabiles = 0;
        SET @MensajeSalida = 'La fecha de inicio debe ser menor a la fecha fin.';
        RETURN;
    END;

    IF DATEDIFF(DAY, @FechaInicio, @FechaFin) > 366
    BEGIN
        SET @TotalDiasHabiles = NULL;
        SET @MensajeSalida = 'El rango de fechas no puede exceder un año.';
        RETURN;
    END;

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
        LEFT JOIN DiasFestivosFijos FF ON DAY(D.Fecha) = FF.Dia AND MONTH(D.Fecha) = FF.Mes
    )
    SELECT @TotalDiasHabiles = SUM(1 - ProporcionDia)
    FROM Feriados
    OPTION (MAXRECURSION 1000);

    SET @MensajeSalida = 'Cálculo exitoso.';
END;
GO

-- 9. Población Inicial de Feriados Variables (Ej: Semana Santa)
-- =================================================================
PRINT 'Poblando feriados de Semana Santa para los próximos años...';
EXEC sp_InsertarSemanaSanta @AnioInicio = 2024, @AnioFin = 2030, @Usr = 'System', @TipoFeriadoId = 3;
GO