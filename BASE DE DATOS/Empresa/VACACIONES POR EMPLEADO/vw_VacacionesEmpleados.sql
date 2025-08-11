use DBProyectoGrupalDojoGeko
GO
/*
IdEstadoSolicitud	NombreEstado
2	Autorizada
4	Cancelada
5	Finalizada	
1	Ingresada
3	Vigente*/

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

    -- Días Solicitados Autorizados (estado aprobado)
    ISNULL((
        SELECT SUM(DiasSolicitadosTotal)
        FROM SolicitudEncabezado
        WHERE FK_IdEmpleado = E.IdEmpleado
          AND FK_IdEstadoSolicitud = 2
    ), 0) AS DiasPendientes_Autorizados,

    -- Solicitudes Ingresadas 
    ISNULL((
        SELECT SUM(DiasSolicitadosTotal)
        FROM SolicitudEncabezado
        WHERE FK_IdEmpleado = E.IdEmpleado
          AND FK_IdEstadoSolicitud = 1
    ), 0) AS DiasPendientes_Ingresados,

	-- Solicitudes  Vigentes 
    ISNULL((
        SELECT SUM(DiasSolicitadosTotal)
        FROM SolicitudEncabezado
        WHERE FK_IdEmpleado = E.IdEmpleado
          AND FK_IdEstadoSolicitud = 3
    ), 0) AS DiasPendientes_Vigentes,

	-- Solicitudes  Finalizada 
    ISNULL((
        SELECT SUM(DiasSolicitadosTotal)
        FROM SolicitudEncabezado
        WHERE FK_IdEmpleado = E.IdEmpleado
          AND FK_IdEstadoSolicitud = 5
    ), 0) AS Dias_Finalizados,

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
          AND (FK_IdEstadoSolicitud != 4)
    ), 0)
    AS SaldoVacaciones

FROM Empleados E;