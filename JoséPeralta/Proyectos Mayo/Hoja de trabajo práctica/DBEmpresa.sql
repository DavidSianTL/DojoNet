use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBEmpresa') is not null
begin
    alter database DBEmpresa set single_user with rollback immediate;
    drop database DBEmpresa;
end
go

CREATE DATABASE DBEmpresa;
GO

USE DBEmpresa;
GO

-- Tabla de Empleados
CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100),
    Apellido NVARCHAR(100),
    FechaNacimiento DATE,
    FechaIngreso DATE,
    Puesto NVARCHAR(100),
    SalarioBase DECIMAL(10,2),
    Activo BIT DEFAULT 1
);

-- Tabla de Planilla
CREATE TABLE Planilla (
    PlanillaID INT PRIMARY KEY IDENTITY(1,1),
    EmpleadoID INT FOREIGN KEY REFERENCES Empleados(EmpleadoID),
    Mes INT,
    Anio INT,
    HorasTrabajadas INT,
    Bonos DECIMAL(10,2),
    Deducciones DECIMAL(10,2),
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Tabla de Pagos
CREATE TABLE Pagos (
    PagoID INT PRIMARY KEY IDENTITY(1,1),
    PlanillaID INT FOREIGN KEY REFERENCES Planilla(PlanillaID),
    FechaPago DATE,
    MontoPagado DECIMAL(10,2),
    MetodoPago NVARCHAR(50)
);

-- Tabla de Logs
CREATE TABLE Logs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME DEFAULT GETDATE(),
    Procedimiento NVARCHAR(100),
    Mensaje NVARCHAR(MAX),
    Error BIT
);

-- Insertar empleados
INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase)
VALUES 
('Ana', 'Martínez', '1990-04-12', '2015-01-10', 'Analista', 800.00),
('Luis', 'Pérez', '1985-07-19', '2012-03-15', 'Supervisor', 1200.00),
('José', 'Peralta', '2004-05-19', '2025-03-15', 'Dev JR', 3500.00),
('Carla', 'Gómez', '1992-11-05', '2018-06-20', 'Asistente', 650.00);

-- Insertar planillas
INSERT INTO Planilla (EmpleadoID, Mes, Anio, HorasTrabajadas, Bonos, Deducciones)
VALUES 
(1, 4, 2025, 160, 100.00, 50.00),
(2, 4, 2025, 160, 200.00, 70.00),
(3, 4, 2025, 160, 80.00, 80.00),
(4, 4, 2025, 160, 50.00, 30.00);

-- Insertar pagos
INSERT INTO Pagos (PlanillaID, FechaPago, MontoPagado, MetodoPago)
VALUES
(1, '2025-05-01', 850.00, 'Transferencia'),
(2, '2025-05-01', 1330.00, 'Cheque'),
(3, '2025-05-01', 670.00, 'Efectivo');
go

CREATE FUNCTION fn_CalcularEdad (@FechaNacimiento DATE)
RETURNS INT
AS
BEGIN
    RETURN DATEDIFF(YEAR, @FechaNacimiento, GETDATE()) -
           CASE WHEN MONTH(@FechaNacimiento) > MONTH(GETDATE()) 
                 OR (MONTH(@FechaNacimiento) = MONTH(GETDATE()) AND DAY(@FechaNacimiento) > DAY(GETDATE()))
                THEN 1 ELSE 0 END;
END;
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
go

-- Parte A: Consultas básicas 
-- 1. Consulta todos los empleados activos.
select * from Empleados e where e.Activo = 1;

-- 2. Muestra los pagos realizados en mayo de 2025.
select * from Pagos p where p.FechaPago between '2025-05-01' and '2025-06-01';

-- 3. Muestra el total pagado a cada empleado.
select e.empleadoid, e.nombre, e.apellido, sum(pa.montopagado) as TotalPagos 
	from Empleados e join Planilla p on e.EmpleadoID = p.EmpleadoID join Pagos pa on pa.PlanillaID  = p.EmpleadoID
		group by e.EmpleadoID, e.nombre, e.apellido;
go

-- Parte B: Funciones y procedimientos
-- 4. Usa fn_CalcularEdad para listar empleados con sus edades.
select e.EmpleadoID, e.Nombre, e.Apellido, e.FechaIngreso, dbo.fn_CalcularEdad(e.FechaNacimiento) as Edad from Empleados e;

-- 5. Ejecuta sp_RegistrarPago para un empleado que aún no tenga pago.
exec sp_RegistrarPago 3,4,2025,'Transferencia';

-- 6. Revisa la tabla de Logs.
select * from Logs;
go

-- Parte C: Manejo de errores y logs 
-- 7. Ejecuta sp_RegistrarPago con un mes o año sin planilla registrada (para disparar el error).
exec sp_RegistrarPago 3,5,2028,'Transferencia';
-- 8. Verifica que el error se registró correctamente en la tabla Logs.
select * from Logs;

-- Parte D: Bonus 
-- 9. Crea una vista que resuma por empleado su salario base, bonos, deducciones y monto pagado.
select * from Empleados;
select * from Planilla;
select * from pagos;
go

-- Hay que hacerlo manual, no sirve solo ejecutando por alguna razón
/*create view vw_ResumenEmpleado as
	select 
		e.empleadoid, 
		e.Nombre, 
		e.apellido, 
		e.SalarioBase, 
		sum(p.Bonos) as TotalBonos, 
		sum(p.Deducciones) as TotalDeducciones,
		sum(pa.montopagado) as TotalMongoPagado
	from Empleados e
	join Planilla p on e.EmpleadoID = p.EmpleadoID
	join Pagos pa on pa.PlanillaID = p.PlanillaID
	group by
		e.EmpleadoID,
		e.Nombre, 
		e.Apellido, 
		e.SalarioBase;

select * from vw_ResumenEmpleado;
go*/

-- 10. Agrega un trigger para registrar cambios en la tabla Empleados.
create trigger tr_CambiosEmpleado
on Empleados
after insert, update
as
begin
	set nocount on;

	insert into logs(Procedimiento, Mensaje, Error)
		select 
			'tr_CambiosEmpleado', 
			concat('Se ha ingresado el nuevo puesto de: ', i.Puesto,', también se ha registrado un nuevo salario base de: ', i.SalarioBase, ', el estado ahora es: ',i.Activo)
			, 0
		from inserted i;
end;
go

insert into Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase)
values ('Pedro', 'López', '1995-09-10', '2023-01-01', 'Contador', 950.00);

select * from Logs;
go

/* SP Para Empleados */
CREATE PROCEDURE sp_InsertarEmpleado
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @FechaNacimiento DATE,
    @FechaIngreso DATE,
    @Puesto NVARCHAR(100),
    @SalarioBase DECIMAL(10, 2)
AS
BEGIN
    INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase)
    VALUES (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase);
END;
GO

CREATE PROCEDURE sp_ActualizarEmpleado
    @EmpleadoID INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @FechaNacimiento DATE,
    @FechaIngreso DATE,
    @Puesto NVARCHAR(100),
    @SalarioBase DECIMAL(10, 2),
    @Activo BIT
AS
BEGIN
    UPDATE Empleados
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        FechaNacimiento = @FechaNacimiento,
        FechaIngreso = @FechaIngreso,
        Puesto = @Puesto,
        SalarioBase = @SalarioBase,
        Activo = @Activo
    WHERE EmpleadoID = @EmpleadoID;
END;
GO

CREATE PROCEDURE sp_EliminarEmpleado
    @EmpleadoID INT
AS
BEGIN
    DELETE FROM Empleados
    WHERE EmpleadoID = @EmpleadoID;
END;
GO

