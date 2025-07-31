use DBProyectoGrupalDojoGeko
GO

CREATE FUNCTION fn_GetDetalleVacacionesEmpleado (@IdEmpleado INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        E.IdEmpleado,
        E.NombresEmpleado,
        E.ApellidosEmpleado,
        E.FechaIngreso,
        
        -- Días generados (a la fecha actual)
        FLOOR(
            (DATEDIFF(YEAR, E.FechaIngreso, GETDATE())
             - CASE WHEN MONTH(E.FechaIngreso) > MONTH(GETDATE())
                        OR (MONTH(E.FechaIngreso) = MONTH(GETDATE()) AND DAY(E.FechaIngreso) > DAY(GETDATE()))
                    THEN 1 ELSE 0 END
            ) * 15
            +
            (DATEDIFF(MONTH, DATEADD(YEAR, 
                     (DATEDIFF(YEAR, E.FechaIngreso, GETDATE())
                      - CASE WHEN MONTH(E.FechaIngreso) > MONTH(GETDATE())
                                 OR (MONTH(E.FechaIngreso) = MONTH(GETDATE()) AND DAY(E.FechaIngreso) > DAY(GETDATE()))
                            THEN 1 ELSE 0 END
                    ), E.FechaIngreso), GETDATE()) * 15.0 / 12.0)
        ) AS DiasGenerados,

        -- Días tomados (estado aprobado)
        ISNULL((
            SELECT SUM(DiasSolicitadosTotal)
            FROM SolicitudEncabezado
            WHERE FK_IdEmpleado = E.IdEmpleado
              AND FK_IdEstadoSolicitud = 2 -- Aprobadas
        ), 0) AS DiasTomados,

        -- Días en solicitudes pendientes (estado pendiente)
        ISNULL((
            SELECT SUM(DiasSolicitadosTotal)
            FROM SolicitudEncabezado
            WHERE FK_IdEmpleado = E.IdEmpleado
              AND FK_IdEstadoSolicitud = 1 -- Pendientes (ajustá el valor)
        ), 0) AS DiasPendientes,

        -- Saldo total disponible
        FLOOR(
            (DATEDIFF(YEAR, E.FechaIngreso, GETDATE())
             - CASE WHEN MONTH(E.FechaIngreso) > MONTH(GETDATE())
                        OR (MONTH(E.FechaIngreso) = MONTH(GETDATE()) AND DAY(E.FechaIngreso) > DAY(GETDATE()))
                    THEN 1 ELSE 0 END
            ) * 15
            +
            (DATEDIFF(MONTH, DATEADD(YEAR, 
                     (DATEDIFF(YEAR, E.FechaIngreso, GETDATE())
                      - CASE WHEN MONTH(E.FechaIngreso) > MONTH(GETDATE())
                                 OR (MONTH(E.FechaIngreso) = MONTH(GETDATE()) AND DAY(E.FechaIngreso) > DAY(GETDATE()))
                            THEN 1 ELSE 0 END
                    ), E.FechaIngreso), GETDATE()) * 15.0 / 12.0)
        )
        -
        ISNULL((
            SELECT SUM(DiasSolicitadosTotal)
            FROM SolicitudEncabezado
            WHERE FK_IdEmpleado = E.IdEmpleado
              AND FK_IdEstadoSolicitud = 2
        ), 0)
        AS SaldoDisponible

    FROM Empleados E
    WHERE E.IdEmpleado = @IdEmpleado
);