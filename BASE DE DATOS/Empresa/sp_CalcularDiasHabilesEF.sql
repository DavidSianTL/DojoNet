USE [DBProyectoGrupalDojoGeko]
GO

/****** Object:  StoredProcedure [dbo].[sp_CalcularDiasHabilesEF]    Script Date: 24/07/2025 11:18:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--SP CAMBIAR ESTADO EMPLEADO
CREATE PROCEDURE [dbo].[sp_CalcularDiasHabilesEF]
    @IdEmpleado INT
AS
BEGIN
    UPDATE Empleados
    SET FK_IdEstado = 4 -- 4 es inactivo y 1 es activo.
    WHERE IdEmpleado = @IdEmpleado;
END;
GO


