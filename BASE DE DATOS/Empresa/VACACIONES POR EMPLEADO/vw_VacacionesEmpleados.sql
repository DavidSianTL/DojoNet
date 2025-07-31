use DBProyectoGrupalDojoGeko
GO

CREATE VIEW vw_VacacionesEmpleados
AS
SELECT 
    E.IdEmpleado,
    E.NombresEmpleado,
    E.ApellidosEmpleado,
    E.FechaIngreso,

    -- Días generados
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
          AND FK_IdEstadoSolicitud = 2
    ), 0) AS DiasTomados,

    -- Días pendientes (estado pendiente)
    ISNULL((
        SELECT SUM(DiasSolicitadosTotal)
        FROM SolicitudEncabezado
        WHERE FK_IdEmpleado = E.IdEmpleado
          AND FK_IdEstadoSolicitud = 1
    ), 0) AS DiasPendientes,

    -- Saldo disponible
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
    AS SaldoVacaciones

FROM Empleados E;