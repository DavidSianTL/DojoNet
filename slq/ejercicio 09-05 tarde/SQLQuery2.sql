use EmpresaDB
go
CREATE PROCEDURE sp_RegistrarPago
    @EmpleadoID INT,
    @Mes INT,
    @Anio INT,
    @MetodoPago NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        DECLARE @PlanillaID INT, @Pago DECIMAL(10,2);

        -- Obtener planilla
        SELECT TOP 1 @PlanillaID = PlanillaID,
                     @Pago = (E.SalarioBase + P.Bonos - P.Deducciones)
        FROM Planilla P
        INNER JOIN Empleados E ON P.EmpleadoID = E.EmpleadoID
        WHERE P.EmpleadoID = @EmpleadoID AND Mes = @Mes AND Anio = @Anio;

        IF @PlanillaID IS NULL
        BEGIN
            THROW 50001, 'No se encontró la planilla para el empleado en ese mes.', 1;
        END

        -- Insertar pago
        INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
        VALUES (@PlanillaID, GETDATE(), @Pago, @MetodoPago);

        -- Log éxito
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', CONCAT('Pago registrado exitosamente para empleado ID: ', @EmpleadoID), 0);
    END TRY
    BEGIN CATCH
        INSERT INTO Logs (Procedimiento, Mensaje, Error)
        VALUES ('sp_RegistrarPago', ERROR_MESSAGE(), 1);
    END CATCH
END;