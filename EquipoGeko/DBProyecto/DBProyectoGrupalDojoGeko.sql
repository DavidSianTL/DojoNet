-- Usamos la master para eliminar la DB que ocupamos
use master;
go

-- Si la base de datos existe, ponerla en modo SINGLE_USER y eliminarla
if DB_ID('DBProyectoGrupalDojoGeko') is not null
begin
    alter database DBProyectoGrupalDojoGeko set single_user with rollback immediate;
    drop database DBProyectoGrupalDojoGeko;
end
go


-- Creamos la DB
CREATE DATABASE DBProyectoGrupalDojoGeko;
GO

-- Usamos nuestra DB
USE DBProyectoGrupalDojoGeko;
GO

-----------------------@José----------------------------------------------------
-- Tabla de Estados
CREATE TABLE Estados(
	IdEstado INT IDENTITY(1,1),
	Estado NVARCHAR(25) NOT NULL,
	Descripcion NVARCHAR(75),
    Activo BIT DEFAULT 1,
	PRIMARY KEY (IdEstado)
);
GO

-- SP para insertar Estado
CREATE PROCEDURE sp_InsertarEstado
    @Estado NVARCHAR(25),
    @Descripcion NVARCHAR(75)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Estados (Estado, Descripcion)
    VALUES (@Estado, @Descripcion);
END;
GO

-- Listar todos los estados
CREATE PROCEDURE sp_ListarEstados
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Estados;
END;
GO

-- Listar estado por ID
CREATE PROCEDURE sp_ListarEstadoId
    @IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Estados WHERE IdEstado = @IdEstado;
END;
GO

-- Actualizar un estado
CREATE PROCEDURE sp_ActualizarEstado
    @IdEstado INT,
    @Estado NVARCHAR(25),
    @Descripcion NVARCHAR(75),
	@Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Estados
    SET Estado = @Estado,
        Descripcion = @Descripcion,
		Activo = @Activo
    WHERE IdEstado = @IdEstado;
END;
GO

-- Eliminar lógicamente un estado (desactivarlo)
CREATE PROCEDURE sp_EliminarEstado
    @IdEstado INT,
	@Activo BIT
AS
BEGIN
    UPDATE Estados
    SET Activo = @Activo
    WHERE IdEstado = @IdEstado;
END;
GO

-- Disparador que actualiza cada registro posible con el estado que se eliminó
/*CREATE TRIGGER tr_ReasignarEstadoAntesDeEliminar
ON Estados
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdEstadoEliminado INT = 999;

    -- Si el estado especial no existe, se cancela todo
    IF NOT EXISTS (SELECT 1 FROM Estados WHERE IdEstado = @IdEstadoEliminado)
    BEGIN
        RAISERROR('El estado "Eliminado" (Id = 999) no existe.', 16, 1);
        RETURN;
    END

    -- Para cada estado que se desea eliminar
    DECLARE cur CURSOR FOR SELECT IdEstado FROM deleted;
    DECLARE @IdEstado INT;

    OPEN cur;
    FETCH NEXT FROM cur INTO @IdEstado;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Aquí actualizas las tablas relacionadas que usan IdEstado
        -- Reemplaza estas con tus tablas reales y columnas

        -- Ejemplo:
        -- UPDATE Empleados SET IdEstado = @IdEstadoEliminado WHERE IdEstado = @IdEstado;

        -- Finalmente elimina el estado original
        DELETE FROM Estados WHERE IdEstado = @IdEstado;

        FETCH NEXT FROM cur INTO @IdEstado;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;
GO*/

-----------------------@José----------------------------------------------------
-- Tabla de Logs
CREATE TABLE Logs(
	IdLog INT IDENTITY(1,1),
	FechaEntrada DATETIME DEFAULT CURRENT_TIMESTAMP,
	Accion NVARCHAR(75),
	Descripcion NVARCHAR(255),
	Estado BIT,
	PRIMARY KEY(IdLog)
);
GO

-- SP para insertar Log by erick-
-- Elimina el procedimiento si ya existe
DROP PROCEDURE IF EXISTS sp_InsertarLog;
GO

-- Crea el procedimiento con lógica de inserción en Logs
CREATE PROCEDURE sp_InsertarLog
    @Accion NVARCHAR(100),
    @Descripcion NVARCHAR(MAX),
    @Estado BIT,
    @FechaEntrada DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaEntrada IS NULL
        SET @FechaEntrada = GETDATE();

    INSERT INTO Logs (
        Accion,
        Descripcion,
        Estado,
        FechaEntrada
    )
    VALUES (
        @Accion,
        @Descripcion,
        @Estado,
        @FechaEntrada
    );
END;
GO


-- SP para listar los Logs
CREATE PROCEDURE sp_ListarLogs
AS
BEGIN
    SELECT * FROM Logs;
END;
GO

-- Sección de pruebas
/*SELECT IdUsuario, Username, contrasenia, Estado, FK_IdEmpleado
FROM Usuarios
WHERE Username = 'AdminDev' AND Estado = 1;
GO*/

/*EXEC sp_ListarLogs
GO*/

/* EXEC sp_ListarBitacoras
GO	

SELECT * FROM TokenUsuario;
GO

SELECT * FROM Usuarios;
GO*/

-- SP para listar por Log
CREATE PROCEDURE sp_ListarLogId
    @IdLog INT
AS
BEGIN
    SELECT * FROM Logs WHERE IdLog = @IdLog;
END;
GO

-- SP para actualizar un Log
CREATE PROCEDURE sp_ActualizarLog
    @IdLog INT,
    @Accion NVARCHAR(75),
    @Descripcion NVARCHAR(255),
    @Estado BIT
AS
BEGIN
    UPDATE Logs
    SET Accion = @Accion,
        Descripcion = @Descripcion,
        Estado = @Estado
    WHERE IdLog = @IdLog;
END;
GO

-- SP para "eliminar" un Log
CREATE PROCEDURE sp_EliminarLog
    @IdLog INT
AS
BEGIN
    DELETE FROM Logs WHERE IdLog = @IdLog;
END;
GO

-----------------------@Carlos----------------------------------------------------
--------------------- Tabla de Empresas
CREATE TABLE Empresas (
    IdEmpresa INT IDENTITY(1,1),       
    Nombre NVARCHAR(100) NOT NULL,     
    Descripcion NVARCHAR(255),         
    Codigo NVARCHAR(50) NOT NULL,
	Logo VARCHAR(100),
    FK_IdEstado INT DEFAULT 1,
	CONSTRAINT FK_Empresas_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado),              
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP, 
    PRIMARY KEY (IdEmpresa)            
);
GO


---Insertar Empresa
CREATE PROCEDURE sp_InsertarEmpresa
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
	@FK_IdEstado INT
AS
BEGIN
    INSERT INTO Empresas (Nombre, Descripcion, Codigo, FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @Codigo, @FK_IdEstado);
END;
GO

--listar todas las empresas 
CREATE PROCEDURE sp_ListarEmpresas
AS
BEGIN
    SELECT * FROM Empresas;
END;
GO

--listar empresa por id 
CREATE PROCEDURE sp_ListarEmpresaId
    @IdEmpresa INT
AS
BEGIN
    SELECT * FROM Empresas WHERE IdEmpresa = @IdEmpresa;
END;
GO

--Actualizar una empresa
CREATE PROCEDURE sp_ActualizarEmpresa
    @IdEmpresa INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Empresas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        FK_IdEstado = @FK_IdEstado
    WHERE IdEmpresa = @IdEmpresa;
END;
GO

--Eliminar una Empresa 
CREATE PROCEDURE sp_EliminarEmpresa
    @IdEmpresa INT
AS
BEGIN
    UPDATE Empresas
    SET FK_IdEstado = 4
    WHERE IdEmpresa = @IdEmpresa;
END;
GO

-----------Tabla Sistemas 
CREATE TABLE Sistemas (
    IdSistema INT IDENTITY(1,1),               
    Nombre NVARCHAR(100) NOT NULL,             
    Descripcion NVARCHAR(255),                 
    Codigo NVARCHAR(50) NOT NULL,              
    FK_IdEstado INT DEFAULT 1,
	CONSTRAINT FK_Sistemas_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado),                      
    FK_IdEmpresa INT,                          
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP, 
    PRIMARY KEY (IdSistema),
    FOREIGN KEY (FK_IdEmpresa) REFERENCES Empresas(IdEmpresa) 
);
GO

--Insertar un Sistema
CREATE PROCEDURE sp_InsertarSistema
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
	@FK_IdEstado INT
AS
BEGIN
    INSERT INTO Sistemas (Nombre, Descripcion, Codigo, FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @Codigo, @FK_IdEstado);
END;
GO

--Listar todos los sistemas 
CREATE PROCEDURE sp_ListarSistemas
AS
BEGIN
    SELECT * FROM Sistemas;
END;
GO

--Listar Sistemas por ID
CREATE PROCEDURE sp_ListarSistemaId
    @IdSistema INT
AS
BEGIN
    SELECT * FROM Sistemas WHERE IdSistema = @IdSistema;
END;
GO

--Actualizar Sistema
CREATE PROCEDURE sp_ActualizarSistema
    @IdSistema INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Sistemas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        FK_IdEstado = @FK_IdEstado
    WHERE IdSistema = @IdSistema;
END;
GO

--Eliminar Sistema
CREATE PROCEDURE sp_EliminarSistema
    @IdSistema INT
AS
BEGIN
    UPDATE Sistemas
    SET FK_IdEstado = 4 -- Cambiar al estado "Inactivo"
    WHERE IdSistema = @IdSistema;
END;
GO

---------------------@José-----------------------------------
-- Tabla de Módulos
CREATE TABLE Modulos (
    IdModulo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FK_IdEstado INT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Modulos_Estados
        FOREIGN KEY (FK_IdEstado)
            REFERENCES Estados (IdEstado)
);
GO


-- Insertar Módulo
CREATE PROCEDURE sp_InsertarModulo
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FK_IdEstado INT
AS
BEGIN
    INSERT INTO Modulos (Nombre, Descripcion,FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @FK_IdEstado);
END;
GO

-- Listar todos los módulos
CREATE PROCEDURE sp_ListarModulos
AS
BEGIN
    SELECT * FROM Modulos;
END;
GO

-- Listar módulo por Id
CREATE PROCEDURE sp_ListarModuloId
    @IdModulo INT
AS
BEGIN
    SELECT * FROM Modulos WHERE IdModulo = @IdModulo;
END;
GO

-- Actualizar módulo
CREATE PROCEDURE sp_ActualizarModulo
    @IdModulo INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Modulos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        FK_IdEstado = @FK_IdEstado
    WHERE IdModulo = @IdModulo;
END;
GO

-- Eliminar (lógico) módulo
CREATE PROCEDURE sp_EliminarModulo
    @IdModulo INT
AS
BEGIN
    UPDATE Modulos
    SET FK_IdEstado = 4 -- Estado "Inactivo"
    WHERE IdModulo = @IdModulo;
END;
GO

---------------------@José-----------------------------------
-- Tabla de relación: Módulos por Sistema (asignación N:N)
CREATE TABLE ModulosSistema (
    IdModuloSistema INT IDENTITY(1,1) PRIMARY KEY,
    FK_IdSistema INT NOT NULL,
    FK_IdModulo INT NOT NULL,
    CONSTRAINT FK_ModulosSistema_Sistema
        FOREIGN KEY (FK_IdSistema)
            REFERENCES Sistemas (IdSistema),
    CONSTRAINT FK_ModulosSistema_Modulo
        FOREIGN KEY (FK_IdModulo)
            REFERENCES Modulos (IdModulo)
);
GO

-- Asignar módulo a sistema
CREATE PROCEDURE sp_AsignarModuloASistema
    @FK_IdSistema INT,
    @FK_IdModulo INT
AS
BEGIN
    INSERT INTO ModulosSistema (FK_IdSistema, FK_IdModulo)
    VALUES (@FK_IdSistema, @FK_IdModulo);
END;
GO

-- Listar todos los módulos asignados a un sistema
CREATE PROCEDURE sp_ListarModulosPorSistema
    @FK_IdSistema INT
AS
BEGIN
    SELECT m.*
    FROM Modulos m
    INNER JOIN ModulosSistema ms ON ms.FK_IdModulo = m.IdModulo
    WHERE ms.FK_IdSistema = @FK_IdSistema;
END;
GO

-- Eliminar asignación de módulo a sistema
CREATE PROCEDURE sp_EliminarModuloDeSistema
    @FK_IdSistema INT,
    @FK_IdModulo INT
AS
BEGIN
    DELETE FROM ModulosSistema
    WHERE FK_IdSistema = @FK_IdSistema AND FK_IdModulo = @FK_IdModulo;
END;
GO

-- Listar todas las asignaciones
CREATE PROCEDURE sp_ListarModulosSistema
AS
BEGIN
    SELECT ms.IdModuloSistema, s.Nombre AS NombreSistema, m.Nombre AS NombreModulo, ms.FK_IdSistema, ms.FK_IdModulo
    FROM ModulosSistema ms
    INNER JOIN Sistemas s ON s.IdSistema = ms.FK_IdSistema
    INNER JOIN Modulos m ON m.IdModulo = ms.FK_IdModulo;
END;
GO

---------------------@Carlos-----------------------------------
------------------Tabla Departamentos
CREATE TABLE Departamentos (
    IdDepartamento INT PRIMARY KEY IDENTITY(1,1),              
    Nombre NVARCHAR(100) NOT NULL,                 
    Descripcion NVARCHAR(255),                    
    Codigo NVARCHAR(50) NOT NULL,                  
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP, 
    FK_IdEstado INT,
	CONSTRAINT FK_Departamentos_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado)
);
GO


--Insertar Departamento 
CREATE PROCEDURE sp_InsertarDepartamento
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
	@FK_IdEstado INT
AS
BEGIN
    INSERT INTO Departamentos (Nombre, Descripcion, Codigo, FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @Codigo, @FK_IdEstado);
END;
GO

--Listar Departamentos
CREATE PROCEDURE sp_ListarDepartamentos
AS
BEGIN
    SELECT * FROM Departamentos;
END;
GO

--Listar Departamento por ID
CREATE PROCEDURE sp_ListarDepartamentoId
    @IdDepartamento INT
AS
BEGIN
    SELECT * FROM Departamentos WHERE IdDepartamento = @IdDepartamento;
END;
GO

--Actualizar un Departamento 
CREATE PROCEDURE sp_ActualizarDepartamento
    @IdDepartamento INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Departamentos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        FK_IdEstado = @FK_IdEstado
    WHERE IdDepartamento = @IdDepartamento;
END;
GO

--Elimar Departamento 
CREATE PROCEDURE sp_EliminarDepartamento
    @IdDepartamento INT
AS
BEGIN
    UPDATE Departamentos
    SET FK_IdEstado = 4 -- Cambiar al estado "Inactivo"
    WHERE IdDepartamento = @IdDepartamento;
END;
GO

---------------------@José-----------------------------------
---Creacion tabla DepartamentosEmpresa-----
CREATE TABLE DepartamentosEmpresa (
    IdDepartamentoEmpresa INT PRIMARY KEY IDENTITY(1,1),
    FK_IdEmpresa INT NOT NULL,
    FK_IdDepartamento INT NOT NULL,
    CONSTRAINT FK_DepartamentosEmpresa_Empresa
        FOREIGN KEY (FK_IdEmpresa)
        REFERENCES Empresas(IdEmpresa),
    CONSTRAINT FK_DepartamentosEmpresa_Departamentosp_EliminarEmpresa
        FOREIGN KEY (FK_IdDepartamento)
        REFERENCES Departamentos(IdDepartamento)
);
GO

--Insertar Departamentos Empresa 
CREATE PROCEDURE sp_InsertarDepartamentoEmpresa
    @FK_IdEmpresa INT,
    @FK_IdDepartamento INT
AS
BEGIN
    INSERT INTO DepartamentosEmpresa (FK_IdEmpresa, FK_IdDepartamento)
    VALUES (@FK_IdEmpresa, @FK_IdDepartamento);
END;
GO

--Listar Departamentos Empresa 
CREATE PROCEDURE sp_ListarDepartamentosEmpresa
AS
BEGIN
    SELECT * FROM DepartamentosEmpresa;
END;
GO

--Listar Departamentos Empresa por ID
CREATE PROCEDURE sp_ListarDepartamentoEmpresaId
    @IdDepartamentoEmpresa INT
AS
BEGIN
    SELECT * FROM DepartamentosEmpresa WHERE IdDepartamentoEmpresa = @IdDepartamentoEmpresa;
END;
GO

--Actualizar Departamentos Empresa 
CREATE PROCEDURE sp_ActualizarDepartamentoEmpresa
    @IdDepartamentoEmpresa INT,
    @FK_IdEmpresa INT,
    @FK_IdDepartamento INT
AS
BEGIN
    UPDATE DepartamentosEmpresa
    SET FK_IdEmpresa = @FK_IdEmpresa,
        FK_IdDepartamento = @FK_IdDepartamento
    WHERE IdDepartamentoEmpresa = @IdDepartamentoEmpresa;
END;
GO

--Eliminar(este si es directo) Departamentos Empresa 
CREATE PROCEDURE sp_EliminarDepartamentoEmpresa
    @IdDepartamentoEmpresa INT
AS
BEGIN
    DELETE FROM DepartamentosEmpresa
    WHERE IdDepartamentoEmpresa = @IdDepartamentoEmpresa;
END;
GO


---------------------@Daniel-----------------------------------
---Creacion tabla Empleados-----
CREATE TABLE Empleados (
	IdEmpleado INT IDENTITY (1,1) PRIMARY KEY,
	TipoContrato VARCHAR(25),
	Pais VARCHAR(50),
	Departamento VARCHAR(50),
	Municipio VARCHAR(50),
	Direccion VARCHAR(255),
	Puesto VARCHAR(25),
	Codigo VARCHAR(20),
	DPI VARCHAR(15),
	Pasaporte VARCHAR(20),
	NombresEmpleado NVARCHAR (50),
	ApellidosEmpleado NVARCHAR (50),
	CorreoPersonal NVARCHAR (50),
	CorreoInstitucional NVARCHAR (50),
	FechaIngreso DATETIME DEFAULT CURRENT_TIMESTAMP,
	DiasVacacionesAcumulados DECIMAL(4, 2) DEFAULT 0,
	FechaNacimiento DATE,
	Telefono VARCHAR(20),
	NIT VARCHAR(15),
	Genero NVARCHAR (10),
	Salario DECIMAL(10, 2),
	Foto VARCHAR(100),
	FK_IdEstado INT,
	CONSTRAINT FK_Empleados_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado)
);
GO

-----PROCEDIMIENTO EMPLEADOS--
--INSETAR EMPLEADO--
CREATE PROCEDURE sp_InsertarEmpleado
    @TipoContrato VARCHAR(25) = NULL,
    @Pais VARCHAR(50),
    @Departamento VARCHAR(50) = NULL,
    @Municipio VARCHAR(50) = NULL,
    @Direccion VARCHAR(255) = NULL,
    @Puesto VARCHAR(25) = NULL,
    @Codigo VARCHAR(20) = NULL,
    @DPI VARCHAR(15) = NULL,
    @Pasaporte VARCHAR(20) = NULL,
    @NombresEmpleado NVARCHAR(50),
    @ApellidosEmpleado NVARCHAR(50),
    @CorreoPersonal NVARCHAR(50),
    @CorreoInstitucional NVARCHAR(50),
    @FechaIngreso DATETIME = NULL,
	@DiasVacacionesAcumulados DECIMAL(4, 2),
    @FechaNacimiento DATE,
    @Telefono VARCHAR(20),
    @NIT VARCHAR(15) = NULL,
    @Genero NVARCHAR(10) = NULL,
    @Salario DECIMAL(10, 2),
    @FK_IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Empleados (
        TipoContrato,
        Pais,
        Departamento,
        Municipio,
        Direccion,
        Puesto,
        Codigo,
        DPI,
        Pasaporte,
        NombresEmpleado,
        ApellidosEmpleado,
        CorreoPersonal,
        CorreoInstitucional,
        FechaIngreso,
		DiasVacacionesAcumulados,
        FechaNacimiento,
        Telefono,
        NIT,
        Genero,
        Salario,
        FK_IdEstado
    )
    VALUES (
        @TipoContrato,
        @Pais,
        @Departamento,
        @Municipio,
        @Direccion,
        @Puesto,
        @Codigo,
        @DPI,
        @Pasaporte,
        @NombresEmpleado,
        @ApellidosEmpleado,
        @CorreoPersonal,
        @CorreoInstitucional,
        @FechaIngreso,
		@DiasVacacionesAcumulados,
        @FechaNacimiento,
        @Telefono,
        @NIT,
        @Genero,
        @Salario,
        @FK_IdEstado
    );
END;
GO

---SP LISTAR EMPLEADO--
CREATE PROCEDURE sp_ListarEmpleados
AS
BEGIN
    SELECT * FROM Empleados;
END;
GO

-- Ingresamos un empleado de prueba
EXEC sp_InsertarEmpleado 
    @TipoContrato = 'Tiempo Completo',
    @Pais = 'Guatemala',
    @Departamento = 'Guatemala',
    @Municipio = 'Guatemala',
    @Direccion = 'Zona 10, Ciudad de Guatemala',
    @Puesto = 'Desarrollador',
    @Codigo = 'EMP001',
    @DPI = '123456789101112',
    @Pasaporte = '123456789101155',
    @NombresEmpleado = 'AdminPrueba',
    @ApellidosEmpleado = 'AdminPrueba',
    @CorreoPersonal = 'adminprueba@gmail.com',
    @CorreoInstitucional = 'adminprueba@geko.com',
    @FechaIngreso = '2023-01-01',
	@DiasVacacionesAcumulados = 0.00,
    @FechaNacimiento = '2000-05-05',
    @Telefono = '12121212',
    @NIT = '1234567891011',
    @Genero = 'Masculino',
    @Salario = 3500.00,
    @FK_IdEstado = null;
GO

-- Revisamos el insert
EXEC sp_ListarEmpleados;
GO

---SP LISTAR EMPLEADO POR ID--
CREATE PROCEDURE sp_ListarEmpleadoId
    @IdEmpleado INT
AS
BEGIN
    SELECT * FROM Empleados WHERE IdEmpleado = @IdEmpleado;
END;
GO

--SP ACTUALIZAR EMPLEADO
CREATE PROCEDURE sp_ActualizarEmpleado
    @IdEmpleado INT,
    @TipoContrato VARCHAR(25) = NULL,
    @Pais VARCHAR(50),
    @Departamento VARCHAR(50) = NULL,
    @Municipio VARCHAR(50) = NULL,
    @Direccion VARCHAR(255) = NULL,
    @Puesto VARCHAR(25) = NULL,
    @Codigo VARCHAR(20) = NULL,
    @DPI VARCHAR(15) = NULL,
    @Pasaporte VARCHAR(20) = NULL,
    @NombresEmpleado NVARCHAR(50),
    @ApellidosEmpleado NVARCHAR(50),
    @CorreoPersonal NVARCHAR(50),
    @CorreoInstitucional NVARCHAR(50),
    @FechaNacimiento DATE,
    @Telefono VARCHAR(20),
    @NIT VARCHAR(15) = NULL,
    @Genero NVARCHAR(10) = NULL,
    @Salario DECIMAL(10, 2),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Empleados
    SET 
        TipoContrato = @TipoContrato,
        Pais = @Pais,
        Departamento = @Departamento,
        Municipio = @Municipio,
        Direccion = @Direccion,
        Puesto = @Puesto,
        Codigo = @Codigo,
        DPI = @DPI,
        Pasaporte = @Pasaporte,
        NombresEmpleado = @NombresEmpleado,
        ApellidosEmpleado = @ApellidosEmpleado,
        CorreoPersonal = @CorreoPersonal,
        CorreoInstitucional = @CorreoInstitucional,
        FechaNacimiento = @FechaNacimiento,
        Telefono = @Telefono,
        NIT = @NIT,
        Genero = @Genero,
        Salario = @Salario,
        FK_IdEstado = @FK_IdEstado
    WHERE IdEmpleado = @IdEmpleado;
END;
GO

--SP CAMBIAR ESTADO EMPLEADO
CREATE PROCEDURE sp_CambiarEstadoEmpleado
    @IdEmpleado INT
AS
BEGIN
    UPDATE Empleados
    SET FK_IdEstado = 4 -- 4 es inactivo y 1 es activo.
    WHERE IdEmpleado = @IdEmpleado;
END;
GO

-- Función para calcular días de vacaciones acumulados
CREATE FUNCTION fn_CalcularDiasVacacionesAcumulados(
    @FechaIngreso DATE
)
RETURNS DECIMAL(5, 2)
AS
BEGIN
    DECLARE @DiasAcumulados DECIMAL(5, 2);
    DECLARE @MesesTrabajados INT;
    
    -- Calcular meses trabajados desde la fecha de ingreso
    SET @MesesTrabajados = DATEDIFF(MONTH, @FechaIngreso, GETDATE());
    
    -- Si es negativo (fecha futura), retornar 0
    IF @MesesTrabajados < 0
        SET @MesesTrabajados = 0;
    
    -- Calcular días acumulados: 1.25 días por mes
    SET @DiasAcumulados = @MesesTrabajados * 1.25;
    
    RETURN @DiasAcumulados;
END;
GO

-- Procedimiento para actualizar días de vacaciones de todos los empleados
CREATE PROCEDURE sp_ActualizarDiasVacacionesEmpleados
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Empleados
    SET DiasVacacionesAcumulados = dbo.fn_CalcularDiasVacacionesAcumulados(FechaIngreso)
    WHERE FK_IdEstado = 1; -- Solo empleados activos
END;
GO

-----------------------@José----------------------------------------------------
-- Tabla de Usuarios
CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(50) NOT NULL,
	Contrasenia VARCHAR(255) NOT NULL,
	FechaExpiracionContrasenia DATETIME,
	FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	FK_IdEstado INT,
	CONSTRAINT FK_Usuarios_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado),
	FK_IdEmpleado INT NOT NULL,
	CONSTRAINT FK_Usuarios_Empleados
		FOREIGN KEY (FK_IdEmpleado)
			REFERENCES Empleados(IdEmpleado)
);
GO

-- SP para insertar Usuarios
CREATE PROCEDURE sp_InsertarUsuario
    @Username NVARCHAR(50),
    @Contrasenia NVARCHAR(255),
	@FK_IdEstado INT,
    @FK_IdEmpleado INT,
	@FechaExpiracionContrasenia DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoId INT;

    INSERT INTO Usuarios (Username, Contrasenia, FechaExpiracionContrasenia, FK_IdEstado, FK_IdEmpleado)
    VALUES (@Username, @Contrasenia, @FechaExpiracionContrasenia, @FK_IdEstado, @FK_IdEmpleado);

    SET @NuevoId = SCOPE_IDENTITY();

    -- Retornamos el ID como resultado de la consulta
    SELECT @NuevoId AS IdUsuario;

    RETURN 0; -- Código de éxito
END
GO

-- SP para listar los Usuarios
CREATE PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;
GO

-- Datos de prueba
-- Ejecución correcta del procedimiento almacenado
EXEC sp_InsertarUsuario 
    @Username = 'AdminDev', 
    @Contrasenia = '12345678',
	@FK_IdEstado = null,
    @FK_IdEmpleado = 1,
    @FechaExpiracionContrasenia = '2025-07-22 03:18:08';
GO

-- Revisamos el insert
EXEC sp_ListarUsuarios;
GO

-- SP para listar por Usuario
CREATE PROCEDURE sp_ListarUsuarioId
	@IdUsuario INT
AS
BEGIN
    SELECT * FROM Usuarios U WHERE U.IdUsuario = @IdUsuario;
END;
GO

-- SP para actualizar un Usuario
CREATE PROCEDURE sp_ActualizarUsuario
    @IdUsuario INT,
    @Username VARCHAR(50),
    @Contrasenia VARCHAR(255),
    @FK_IdEstado INT,
	@FK_IdEmpleado INT
AS
BEGIN
    UPDATE Usuarios
    SET Username = @Username,
        Contrasenia = @Contrasenia,
        FK_IdEstado = @FK_IdEstado,
		FK_IdEmpleado = @FK_IdEmpleado
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- SP para actualizar un la contraseña expirada de un nuevo Usuario
CREATE PROCEDURE sp_ActualizarContraseniaExpiracion
    @IdUsuario INT,
    @Contrasenia NVARCHAR(255),
    @FechaExpiracionContrasenia DATETIME
AS
BEGIN
    UPDATE Usuarios
    SET Contrasenia = @Contrasenia,
        FechaExpiracionContrasenia = @FechaExpiracionContrasenia
    WHERE IdUsuario = @IdUsuario
END
GO

-- SP para "eliminar" un Usuario
-- Solo le cambiamos el estado para decir que se ha "eliminado"
CREATE PROCEDURE sp_EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    UPDATE Usuarios
    SET FK_IdEstado = 4
    WHERE IdUsuario = @IdUsuario;
END;
GO


-- Tabla de Token por Usuario
CREATE TABLE TokenUsuario(
	IdTokenUsuario INT IDENTITY(1,1),
	FechaCreacion DATETIME NOT NULL,
	Token NVARCHAR(MAX) NOT NULL,
	TiempoExpira DATETIME NOT NULL,
	FK_IdUsuario INT NOT NULL,
	PRIMARY KEY(IdTokenUsuario),
	CONSTRAINT FK_TokenUsuario_Usuario
		FOREIGN KEY(FK_IdUsuario)
			REFERENCES Usuarios(IdUsuario)
);
GO

select * from TokenUsuario;
GO

-- SP que valida el token
CREATE PROCEDURE sp_ValidarToken
    @Token NVARCHAR(MAX)
AS
BEGIN
    SELECT COUNT(*) AS TokenValido
    FROM TokenUsuario
    WHERE Token = @Token AND TiempoExpira > CURRENT_TIMESTAMP;
END
GO

-- SP que retira el token
CREATE PROCEDURE sp_RevocarToken
    @FK_IdUsuario INT
AS
BEGIN
    DELETE
    FROM TokenUsuario
    WHERE FK_IdUsuario = @FK_IdUsuario;
END
GO
-----------------@Daniel--------------------
---Listar Usuarios pendiente SP
CREATE PROCEDURE sp_ListarUsuariosPendientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdUsuario,
        Username,
        Contrasenia,
        FechaCreacion,
        FK_IdEstado,
        FK_IdEmpleado,
        FechaExpiracionContrasenia
    FROM 
        Usuarios
    WHERE 
        FK_IdEstado = 2; -- 2 = Pendiente
END
GO

---Actualizar usuario Estado SP
CREATE PROCEDURE sp_ActualizarEstadoUsuario
    @IdUsuario INT,
    @FK_IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET 
        FK_IdEstado = @FK_IdEstado
    WHERE 
        IdUsuario = @IdUsuario;
END
GO

---------------------@Daniel-----------------------------------
---Creacion tabla Roles-----
CREATE TABLE Roles (
	IdRol INT IDENTITY (1,1) PRIMARY KEY,
	NombreRol NVARCHAR (50),
	FK_IdEstado INT DEFAULT 1,
	CONSTRAINT FK_Roles_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado)
);
GO

--PROCEDIMIENTOS ALMACENADOS ROLES--
--SP Insertar Rol
CREATE PROCEDURE sp_InsertarRol
    @NombreRol NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    INSERT INTO Roles (NombreRol, FK_IdEstado)
    VALUES (@NombreRol, @FK_IdEstado);
END
GO

--SP Listar Roles
CREATE PROCEDURE sp_ListarRoles
AS
BEGIN
    SELECT * FROM Roles;
END
GO

--SP Listar Roles por Id
CREATE PROCEDURE sp_ListarRolId
	@IdRol int
AS
BEGIN
    SELECT * FROM Roles R WHERE R.IdRol = @IdRol;
END
GO

--SP Actualizar Rol
CREATE PROCEDURE sp_ActualizarRol
    @IdRol INT,
    @NombreRol NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Roles
    SET NombreRol = @NombreRol,
        FK_IdEstado = @FK_IdEstado
    WHERE IdRol = @IdRol;
END
GO

--SP Eliminar Rol
CREATE PROCEDURE sp_EliminarRol
    @IdRol INT
AS
BEGIN
    DELETE FROM Roles WHERE IdRol = @IdRol;
END
GO
	
		
---Creacion tabla Permisos-----
CREATE TABLE Permisos (
	IdPermiso INT IDENTITY (1,1) PRIMARY KEY,
	NombrePermiso NVARCHAR (50),
	Descripcion NVARCHAR (50),
	FK_IdEstado INT DEFAULT 1,
	CONSTRAINT FK_Permisos_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado)
);
GO

----PROCEDIMIENTO ALMACENADO PERMISO--
--SP Insertar Permiso
CREATE PROCEDURE sp_InsertarPermiso
    @NombrePermiso NVARCHAR(50),
    @Descripcion NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    INSERT INTO Permisos (NombrePermiso, Descripcion, FK_IdEstado)
    VALUES (@NombrePermiso, @Descripcion, @FK_IdEstado);
END
GO


--SP Listar Permisos
CREATE PROCEDURE sp_ListarPermisos
AS
BEGIN
    SELECT * FROM Permisos;
END
GO

--SP Listar Permiso por Id
CREATE PROCEDURE sp_ListarPermisoId
    @IdPermiso INT
AS
BEGIN
    SELECT * FROM Permisos
    WHERE IdPermiso = @IdPermiso;
END
GO


--SP Actualizar Permiso
CREATE PROCEDURE sp_ActualizarPermiso
    @IdPermiso INT,
    @NombrePermiso NVARCHAR(50),
    @Descripcion NVARCHAR(50),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Permisos
    SET NombrePermiso = @NombrePermiso,
        Descripcion = @Descripcion,
        FK_IdEstado = @FK_IdEstado
    WHERE IdPermiso = @IdPermiso;
END
GO

-- sp Eliminar Permiso--- CAMBIA EL ESTADO
CREATE PROCEDURE sp_EliminarPermiso
    @IdPermiso INT
AS
BEGIN
    UPDATE Permisos
    SET FK_IdEstado = 4 -- Cambiar al estado "Inactivo"
	WHERE IdPermiso = @IdPermiso;
END
GO


-----------------------@José----------------------------------------------------
-- Tabla de Bítacora
CREATE TABLE Bitacora(
	IdBitacora INT IDENTITY(1,1),
	FechaEntrada DATETIME DEFAULT CURRENT_TIMESTAMP,
	FK_IdUsuario INT, 
	FK_IdSistema INT, -- Así se deben de crear las foraneas empezando con el "FK_"
	Accion NVARCHAR(75),
	Descripcion NVARCHAR(255),
	PRIMARY KEY(IdBitacora)
);
GO

-- SP para insertar Bítacora
CREATE PROCEDURE sp_InsertarBitacora
    @FK_IdUsuario INT,
    @FK_IdSistema INT,
    @Accion NVARCHAR(75),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    INSERT INTO Bitacora (FK_IdUsuario, FK_IdSistema, Accion, Descripcion)
    VALUES (@FK_IdUsuario, @FK_IdSistema, @Accion, @Descripcion);
END;
GO

-- SP para listar las Bítacora
CREATE PROCEDURE sp_ListarBitacoras
AS
BEGIN
    SELECT * FROM Bitacora;
END;
GO

-- SP para listar por Bítacora
CREATE PROCEDURE sp_ListarBitacoraId
	@IdBitacora INT
AS
BEGIN
    SELECT * FROM Bitacora B WHERE B.IdBitacora = @IdBitacora;
END;
GO

-- SP para actualizar una Bítacora
CREATE PROCEDURE sp_ActualizarBitacora
    @IdBitacora INT,
    @FK_IdUsuario INT,
    @FK_IdSistema INT,
    @Accion NVARCHAR(75),
    @Descripcion NVARCHAR(255)
AS
BEGIN
    UPDATE Bitacora
    SET FK_IdUsuario = @FK_IdUsuario,
        FK_IdSistema = @FK_IdSistema,
        Accion = @Accion,
        Descripcion = @Descripcion
    WHERE IdBitacora = @IdBitacora;
END;
GO

-- SP para "eliminar" una Bítacora
CREATE PROCEDURE sp_EliminarBitacora
    @IdBitacora INT
AS
BEGIN
    DELETE FROM Bitacora WHERE IdBitacora = @IdBitacora;
END;
GO


------------- José -----------------------------
-- Relación de Empleados con Departamento
CREATE TABLE EmpleadosDepartamento(
	IdEmpleadosDepartamento INT PRIMARY KEY IDENTITY(1,1),
	FK_IdDepartamento INT NOT NULL,
	FK_IdEmpleado INT NOT NULL,
		FOREIGN KEY (FK_IdDepartamento)
			REFERENCES Departamentos (IdDepartamento),
		FOREIGN KEY (FK_IdEmpleado)
			REFERENCES Empleados (IdEmpleado)
);
GO

-- SP para buscar las relaciones existentes
CREATE PROCEDURE sp_ListarEmpleadosDepartamento
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM EmpleadosDepartamento;
END;
GO

-- SP para buscar una relación por ID
CREATE PROCEDURE sp_ObtenerEmpleadoDepartamentoPorId
	@IdEmpleadosDepartamento INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM EmpleadosDepartamento WHERE IdEmpleadosDepartamento = @IdEmpleadosDepartamento;
END;
GO

-- SP para insertar la relación
CREATE PROCEDURE sp_InsertarEmpleadosDepartamento
	@FK_IdDepartamento INT,
	@FK_IdEmpleado INT
AS
BEGIN
	INSERT INTO EmpleadosDepartamento (FK_IdDepartamento, FK_IdEmpleado)
		VALUES (@FK_IdDepartamento, @FK_IdEmpleado)
END;
GO

-- SP para editar una relación
CREATE PROCEDURE sp_ActualizarEmpleadosDepartamento
	@IdSistemasEmpresa INT,
	@FK_IdEmpresa INT,
	@FK_IdSistema INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE SistemasEmpresa 
		SET FK_IdEmpresa = @FK_IdEmpresa,
			FK_IdSistema = @FK_IdSistema
		WHERE IdSistemasEmpresa = @IdSistemasEmpresa
END;
GO

-- SP para eliminar una relación
CREATE PROCEDURE sp_EliminarEmpleadosDepartamento
	@IdSistemasEmpresa INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SistemasEmpresa WHERE IdSistemasEmpresa = @IdSistemasEmpresa;
END;
GO


-------------------------------------- José ---------------------------
-- Relación de Empresa con Sistemas
CREATE TABLE SistemasEmpresa(
	IdSistemasEmpresa INT PRIMARY KEY IDENTITY(1,1),
	FK_IdEmpresa INT NOT NULL,
	FK_IdSistema INT NOT NULL,
	CONSTRAINT FK_SistemasEmpresa_Empresa
		FOREIGN KEY (FK_IdEmpresa)
			REFERENCES Empresas (IdEmpresa),
	CONSTRAINT FK_SistemasEmpresa_Sistema
		FOREIGN KEY (FK_IdSistema)
			REFERENCES Sistemas (IdSistema)
);
GO

-- SP para buscar las relaciones existentes
CREATE PROCEDURE sp_ListarSistemasEmpresa
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM SistemasEmpresa;
END;
GO

-- SP para buscar una relación por ID
CREATE PROCEDURE sp_BuscarSistemasEmpresaPorId
	@IdSistemasEmpresa INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM SistemasEmpresa WHERE IdSistemasEmpresa = @IdSistemasEmpresa;
END;
GO

-- SP para insertar la relación
CREATE PROCEDURE sp_InsertarSistemasEmpresa
	@FK_IdEmpresa INT,
	@FK_IdSistema INT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO SistemasEmpresa (FK_IdEmpresa, FK_IdSistema)
	VALUES (@FK_IdEmpresa, @FK_IdSistema);
END;
GO

-- SP para editar una relación
CREATE PROCEDURE sp_ActualizarSistemasEmpresa
	@IdSistemasEmpresa INT,
	@FK_IdEmpresa INT,
	@FK_IdSistema INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE SistemasEmpresa 
		SET FK_IdEmpresa = @FK_IdEmpresa,
			FK_IdSistema = @FK_IdSistema
		WHERE IdSistemasEmpresa = @IdSistemasEmpresa
END;
GO

-- SP para eliminar una relación
CREATE PROCEDURE sp_EliminarSistemasEmpresa
	@IdSistemasEmpresa INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM SistemasEmpresa WHERE IdSistemasEmpresa = @IdSistemasEmpresa;
END;
GO

-------------@Junior------------------------------- RELACIONES N:N -----------------------------------------------------------------------------------------------
CREATE TABLE EmpleadosEmpresa(
    IdEmpleadoEmpresa INT PRIMARY KEY IDENTITY(1,1),
	FK_IdEmpresa INT,
    FK_IdEmpleado INT,
	CONSTRAINT FK_EmpleadosEmpresa_Empresa
		FOREIGN KEY (FK_IdEmpresa)
			REFERENCES Empresas (IdEmpresa),
	CONSTRAINT FK_EmpleadosEmpresa_Empleados
		FOREIGN KEY (FK_IdEmpleado)
			REFERENCES Empleados (IdEmpleado)
);
GO

    ---STORAGE PROCEDURES---

    -----------------------------------------------INSERT
    CREATE PROCEDURE sp_InsertarEmpleadosEmpresa
	    @FK_IdEmpresa INT,
        @FK_IdEmpleado INT
    AS 
    BEGIN 

        INSERT INTO EmpleadosEmpresa (FK_IdEmpleado, FK_IdEmpresa)
        VALUES (@FK_IdEmpleado, @FK_IdEmpresa);
        
    END;
    GO 

    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ObtenerEmpleadosEmpresa
    AS
    BEGIN 
        SELECT * FROM EmpleadosEmpresa;
    END;
    GO	

	-----------------------------------------------SELECT for IdEmpleadoEmpresa
	CREATE PROCEDURE sp_ObtenerEmpleadoEmpresaPorId
        @IdEmpleadoEmpresa INT
    AS
    BEGIN
        SELECT * FROM EmpleadosEmpresa WHERE IdEmpleadoEmpresa = @IdEmpleadoEmpresa;
    END;
    GO

    -----------------------------------------------SELECT for IdEmpelado

    CREATE PROCEDURE sp_ObtenerEmpleadoEmpresaPorIdEmpleado
        @FK_IdEmpleado INT
    AS
    BEGIN
        SELECT * FROM EmpleadosEmpresa WHERE FK_IdEmpleado = @FK_IdEmpleado;
    END;
    GO
    -----------------------------------------------SELECT for IdEmpresa

    CREATE PROCEDURE sp_ListarEmpleadoEmpresaPorEmpresa
        @FK_IdEmpresa INT
    AS
    BEGIN
        SELECT * FROM EmpleadosEmpresa WHERE FK_IdEmpresa = @FK_IdEmpresa;
    END;
    GO

	-----------------------------------------------UPDATE
	
    CREATE PROCEDURE sp_ActualizarEmpleadoEmpresa
		@IdEmpleadoEmpresa INT,
        @FK_IdEmpleado INT,
        @FK_IdEmpresa INT
    AS 
    BEGIN 
        UPDATE EmpleadosEmpresa
        SET 
            FK_IdEmpleado = @FK_IdEmpleado,
            FK_IdEmpresa = @FK_IdEmpresa
        WHERE IdEmpleadoEmpresa = @IdEmpleadoEmpresa;
    END;
    GO

    -----------------------------------------------DELETE

    CREATE PROCEDURE sp_EliminarEmpleadoEmpresa
        @FK_IdEmpleado INT,
        @FK_IdEmpresa INT
    AS 
    BEGIN 
        DELETE FROM EmpleadosEmpresa
        WHERE FK_IdEmpleado = @FK_IdEmpleado AND FK_IdEmpresa = @FK_IdEmpresa;
    END;
    GO

	------------------- Vista --------------------------
	CREATE VIEW SistemasEmpresaView AS
		SELECT
			es.IdSistemasEmpresa,
			e.IdEmpresa as IdEmpresa,
			e.Nombre AS NombreEmpresa,
			s.IdSistema AS IdSistema,
			s.Nombre AS NombreSistema
		FROM
			SistemasEmpresa es
			INNER JOIN Empresas e ON e.IdEmpresa = es.FK_IdEmpresa
			INNER JOIN Sistemas s ON s.IdSistema = es.FK_IdSistema;
	GO

-- Tabla de Usuarios y su Rol
CREATE TABLE UsuariosRol(
    IdUsuariosRol INT PRIMARY KEY IDENTITY(1,1),
    FK_IdUsuario INT NOT NULL,
    FK_IdRol INT NOT NULL,
    FOREIGN KEY (FK_IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (FK_IdRol) REFERENCES Roles(IdRol)
);
GO
    ------------------STORAGE PROCEDURES---------------------------

    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarUsuariosRol
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol;
    END;
    GO


    -----------------------------------------------SELECT for IdUsuario

    CREATE PROCEDURE sp_ListarUsuariosRolPorIdUsuario
        @FK_IdUsuario INT
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol
        WHERE FK_IdUsuario = @FK_IdUsuario;
    END;
    GO

    -----------------------------------------------SELECT for IdUsuariosRol ---un registro en especifico

    CREATE PROCEDURE sp_ListarUsuariosRolPorId
        @IdUsuariosRol INT
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol
        WHERE IdUsuariosRol = @IdUsuariosRol;
    END;
    GO


    -----------------------------------------------SELECT for IdRol

    CREATE PROCEDURE sp_ListarUsuariosRolPorIdRol
        @FK_IdRol INT
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol
        WHERE FK_IdRol = @FK_IdRol;
    END;
    GO

    -----------------------------------------------INSERT
    CREATE PROCEDURE sp_InsertarUsuariosRol
        @FK_IdUsuario INT,
        @FK_IdRol INT
    AS
    BEGIN 
        IF NOT EXISTS(
            SELECT 1 FROM UsuariosRol
            WHERE FK_IdUsuario = @FK_IdUsuario AND FK_IdRol = @FK_IdRol
        )    
        BEGIN
            INSERT INTO UsuariosRol (FK_IdUsuario, FK_IdRol)
            VALUES( @FK_IdUsuario, @FK_IdRol);
        END

        ELSE 
        BEGIN
            PRINT 'LA RELACION YA EXISTE.';
        END
    END;
    GO

    -----------------------------------------------UPDATE
    CREATE PROCEDURE sp_ActualizarUsuariosRol
        @IdUsuariosRol INT,
        @FK_IdUsuario INT,
        @FK_IdRol INT
    AS 
    BEGIN 
        UPDATE UsuariosRol
        SET 
            FK_IdUsuario = @FK_IdUsuario,
            FK_IdRol = @FK_IdRol
        WHERE IdUsuariosRol = @IdUsuariosRol;
    END;
    GO


    -----------------------------------------------DELETE 
    CREATE PROCEDURE sp_EliminarUsuariosRol
       @IdUsuarioRol INT
    AS 
    BEGIN 
        DELETE FROM UsuariosRol
        WHERE IdUsuariosRol = @IdUsuarioRol;
    END;
    GO


-- Tabla de Rol y Permisos
CREATE TABLE RolPermisos(
    IdRolPermiso INT PRIMARY KEY IDENTITY(1,1),
    FK_IdRol INT NOT NULL, 
    FK_IdPermiso INT NOT NULL,
    FK_IdSistema INT NOT NULL,
    FOREIGN KEY (FK_IdRol) REFERENCES  Roles(IdRol),
    FOREIGN KEY (FK_IdPermiso) REFERENCES  Permisos(IdPermiso),
    FOREIGN KEY (FK_IdSistema) REFERENCES Sistemas(IdSistema)
);
GO

    --------------STORAGE PROCEDURES------------------------------------
    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarRolPermiso
    AS 
    BEGIN 
        SELECT * FROM RolPermisos; 
    END;
    GO

    -----------------------------------------------SELECT for IdRolPermiso
    CREATE PROCEDURE sp_ListarRolPermisoPorId
        @IdRolPermiso INT
    AS
    BEGIN
        SELECT * FROM RolPermisos 
        WHERE IdRolPermiso = @IdRolPermiso;
    END;
    GO

    -----------------------------------------------SELECT for IdRol
    CREATE PROCEDURE sp_ListarRolPermisoPorIdRol
        @FK_IdRol INT
    AS 
    BEGIN 
        SELECT * FROM RolPermisos
        WHERE FK_IdRol = @FK_IdRol;
    END;
    GO


    -----------------------------------------------SELECT for IdPermiso
    CREATE PROCEDURE sp_ListarRolPermisoPorIdPermiso
        @FK_IdPermiso INT
    AS 
    BEGIN 
        SELECT * FROM RolPermisos
        WHERE FK_IdPermiso = @FK_IdPermiso;
    END;
    GO
    -----------------------------------------------SELECT for IdSistema
    CREATE PROCEDURE sp_ListarRolPermisoPorIdSistema
        @FK_IdSistema INT
    AS 
    BEGIN 
        SELECT * FROM RolPermisos
        WHERE FK_IdSistema = @FK_IdSistema;
    END;
    GO

    -----------------------------------------------INSERT
    CREATE PROCEDURE sp_InsertarRolPermiso
        @FK_IdRol INT,
        @FK_IdPermiso INT,
        @FK_IdSistema INT
    AS 
    BEGIN 
        IF NOT EXISTS(
            SELECT 1 FROM RolPermisos
            WHERE FK_IdRol = @FK_IdRol AND FK_IdPermiso = @FK_IdPermiso AND @FK_IdSistema = FK_IdSistema
        )
        BEGIN
            INSERT INTO RolPermisos (FK_IdRol, FK_IdPermiso, FK_IdSistema)
            VALUES (@FK_IdRol, @FK_IdPermiso, @FK_IdSistema);
        END

        ELSE 
        BEGIN 
            PRINT 'LA RELACION YA EXISTE. ';
        END
    END;
    GO

    -----------------------------------------------UPDATE

    CREATE PROCEDURE sp_ActualizarRolPermisos
        @IdRolPermiso INT, 
        @FK_IdRol INT, 
        @FK_IdPermiso INT,
        @FK_IdSistema INT
    AS 
    BEGIN
        UPDATE RolPermisos
        SET 
            FK_IdRol = @FK_IdRol,
            FK_IdPermiso = @FK_IdPermiso,
            FK_IdSistema = @FK_IdSistema
        WHERE IdRolPermiso = @IdRolPermiso;
    END;
    GO


    -----------------------------------------------DELETE
    CREATE PROCEDURE sp_EliminarRolPermiso
        @IdRolPermiso INT
    AS 
    BEGIN 
        DELETE FROM RolPermisos 
        WHERE IdRolPermiso = @IdRolPermiso;
    END;
    GO

	SELECT * FROM USUARIOS;
	GO


----------------SECCIÓN VACACIONAL--------------------
-- 1. Crear la tabla EstadoSolicitud
CREATE TABLE EstadoSolicitud
(
    IdEstadoSolicitud INT PRIMARY KEY IDENTITY(1,1),
    NombreEstado VARCHAR(50) NOT NULL UNIQUE
);
GO

-- 2. Insert Datos
INSERT INTO EstadoSolicitud (NombreEstado) 
VALUES 
('Ingresada'),
('Autorizada'),
('Vigente'),
('Cancelada'),
('Finalizada');
GO


--- tabla solicitud--
-- 1. Crear la tabla de Encabezado de Solicitud
-- Almacena la información general de cada solicitud.
CREATE TABLE SolicitudEncabezado
(
    IdSolicitud INT PRIMARY KEY IDENTITY(1,1),
    FK_IdEmpleado INT NOT NULL,
	NombresEmpleado NVARCHAR(100) NULL,
    DiasSolicitadosTotal DECIMAL(4,2) NULL,
    FechaIngresoSolicitud DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
	SolicitudLider VARCHAR(5) NULL CHECK (SolicitudLider IN ('Sí', 'No')),
	Observaciones NVARCHAR(100) NULL,
    FK_IdEstadoSolicitud INT NOT NULL,
    FK_IdAutorizador INT NULL,
    FechaAutorizacion DATETIME NULL,
    MotivoRechazo NVARCHAR(500) NULL,

    CONSTRAINT FK_Solicitud_Empleado 
		FOREIGN KEY (FK_IdEmpleado) 
			REFERENCES Empleados(IdEmpleado),
    CONSTRAINT FK_Solicitud_Estado 
		FOREIGN KEY (FK_IdEstadoSolicitud) 
			REFERENCES EstadoSolicitud(IdEstadoSolicitud),
    CONSTRAINT FK_Solicitud_Encabezado_Autorizador 
		FOREIGN KEY (FK_IdAutorizador) REFERENCES Usuarios(IdUsuario)
);
GO

------------- PROCEDIMIENTOS ALMACENADOS


-- SPs Filtros SolicitudEncabezado

-- Filtro para autorizador
CREATE PROCEDURE sp_ListarSolicitudEncabezado_Autorizador
    @FK_IdAutorizador INT
AS 
BEGIN 
   SELECT 
    IdSolicitud,
    FK_IdEmpleado,
    NombresEmpleado,
    DiasSolicitadosTotal,
    FechaIngresoSolicitud

FROM SolicitudEncabezado
WHERE FK_IdAutorizador = 1 AND FK_IdEstadoSolicitud = 1; -- 'Ingresada'
END;
GO

-- Sin filtro para RRHH o Administración
CREATE PROCEDURE sp_ListarSolicitudEncabezado 
AS 
BEGIN 
    SELECT 
        IdSolicitud,
        FK_IdEmpleado,
        NombresEmpleado,
        DiasSolicitadosTotal,
        FechaIngresoSolicitud

    FROM SolicitudEncabezado;
END;
GO

-- SP con campos para filtrar en backend
CREATE PROCEDURE sp_ListarSolicitudEncabezado_Campos
AS 
BEGIN
    SELECT 
        sl.IdSolicitud,
        sl.FK_IdEmpleado,
        sl.NombresEmpleado,
        sl.DiasSolicitadosTotal,
        sl.FechaIngresoSolicitud,
        emp.Nombre AS NombreEmpresa,
        est.NombreEstado,
        sld.FechaInicio,
        sld.FechaFin

    FROM 
        SolicitudEncabezado AS sl
        INNER JOIN EmpleadosEmpresa AS eme ON eme.FK_IdEmpleado = sl.FK_IdEmpleado
        INNER JOIN Empresas AS emp ON emp.IdEmpresa = eme.FK_IdEmpresa
        INNER JOIN EstadoSolicitud AS est ON est.IdEstadoSolicitud = sl.FK_IdEstadoSolicitud
        INNER JOIN SolicitudDetalle AS sld ON sld.FK_IdSolicitud = sl.IdSolicitud
END;
GO 



-- 2. Crear la tabla de Detalle de Solicitud
-- Almacena los períodos de vacaciones específicos para cada solicitud.
CREATE TABLE SolicitudDetalle
(
    IdSolicitudDetalle INT PRIMARY KEY IDENTITY(1,1),
    FK_IdSolicitud INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    DiasHabilesTomados DECIMAL(4,2) NOT NULL,
    CONSTRAINT FK_Detalle_Encabezado 
		FOREIGN KEY (FK_IdSolicitud) 
			REFERENCES SolicitudEncabezado(IdSolicitud) 
				-- El One Delete Cascade funciona para que si se elimina
				-- el registro padre, se elimine en casada los hijos
				ON DELETE CASCADE
);
GO

-- Obtener detalles
CREATE PROCEDURE sp_ObtenerDetalleSolicitud
    @IdSolicitud INT
AS
BEGIN
    -- Primer resultset: Encabezado de la solicitud (con datos básicos del empleado)
    SELECT 
        se.IdSolicitud,
        se.FK_IdEmpleado AS IdEmpleado,
        se.DiasSolicitadosTotal,
        se.FechaIngresoSolicitud,
        es.NombreEstado AS Estado
    FROM SolicitudEncabezado se
    INNER JOIN Empleados e ON se.FK_IdEmpleado = e.IdEmpleado
    INNER JOIN EstadoSolicitud es ON se.FK_IdEstadoSolicitud = es.IdEstadoSolicitud
    WHERE se.IdSolicitud = @IdSolicitud;

    -- Segundo resultset: Detalle de la solicitud
    SELECT 
        sd.IdSolicitudDetalle,
        sd.FechaInicio,
        sd.FechaFin,
        sd.DiasHabilesTomados
    FROM SolicitudDetalle sd
    WHERE sd.FK_IdSolicitud = @IdSolicitud;
END
GO

-- Obtener Encabezado por medio del IdEmpleado
CREATE PROCEDURE sp_ObtenerSolicitudesPorEmpleado
    @IdEmpleado INT
AS
BEGIN
    SELECT 
        se.IdSolicitud,
        se.FK_IdEmpleado AS IdEmpleado,
        se.DiasSolicitadosTotal,
        se.FechaIngresoSolicitud,
        es.IdEstadoSolicitud AS Estado
    FROM SolicitudEncabezado se
    INNER JOIN EstadoSolicitud es ON se.FK_IdEstadoSolicitud = es.IdEstadoSolicitud
    WHERE se.FK_IdEmpleado = @IdEmpleado;
END
GO

-- Obtener Detalle por medio del IdSolicitud
CREATE PROCEDURE sp_ObtenerDetallesPorSolicitud
    @IdSolicitud INT
AS
BEGIN
    SELECT 
        sd.IdSolicitudDetalle,
        sd.FK_IdSolicitud AS IdSolicitud,
        sd.FechaInicio,
        sd.FechaFin,
        sd.DiasHabilesTomados
    FROM SolicitudDetalle sd
    WHERE sd.FK_IdSolicitud = @IdSolicitud;
END
GO


CREATE PROCEDURE sp_InsertarSolicitudEncabezado
    @IdEmpleado INT,
	@NombresEmpleado VARCHAR(100),
    @DiasSolicitadosTotal DECIMAL(4,2),
    @FechaIngresoSolicitud DATETIME,
	@SolicitudLider NVARCHAR(5),
	@Observaciones NVARCHAR(100),
    @Estado INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO SolicitudEncabezado
        (FK_IdEmpleado, NombresEmpleado, DiasSolicitadosTotal, FechaIngresoSolicitud, SolicitudLider, Observaciones, FK_IdEstadoSolicitud)
    VALUES
        (@IdEmpleado, @NombresEmpleado, @DiasSolicitadosTotal, @FechaIngresoSolicitud, @SolicitudLider, @Observaciones, @Estado);

    -- Retornar el ID generado
    SELECT SCOPE_IDENTITY() AS IdSolicitud;

	-- Restamos la cantidad de fechas ingresadas a los días disponibles
	UPDATE Empleados
		SET DiasVacacionesAcumulados = DiasVacacionesAcumulados - @DiasSolicitadosTotal
		WHERE IdEmpleado = @IdEmpleado;

END
GO
-------------

CREATE PROCEDURE sp_InsertarSolicitudDetalle
    @IdSolicitud INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @DiasHabiles DECIMAL(4,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO SolicitudDetalle
        (FK_IdSolicitud, FechaInicio, FechaFin, DiasHabilesTomados)
    VALUES
        (@IdSolicitud, @FechaInicio, @FechaFin, @DiasHabiles);
END
GO


-------- SECCION DE PROYECTO Y EQUIPOS-------
-- --------------------- Tabla de Proyectos ---------------------
CREATE TABLE Proyectos (
    IdProyecto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FechaInicio DATE,
    FK_IdEstado INT DEFAULT 1,
    CONSTRAINT FK_Proyectos_Estados
        FOREIGN KEY (FK_IdEstado)
            REFERENCES Estados (IdEstado)
);
GO

-- --------------------- Tabla de Equipos ---------------------
CREATE TABLE Equipos (
    IdEquipo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255),
    FK_IdEstado INT DEFAULT 1,
    CONSTRAINT FK_Equipos_Estados
        FOREIGN KEY (FK_IdEstado)
            REFERENCES Estados (IdEstado)
);
GO

-- --------------------- Tabla Intermedia: EmpleadosEquipo ---------------------
-- Relaciona los empleados con los equipos y utiliza la tabla existente de Roles
CREATE TABLE EmpleadosEquipo (
    IdEmpleadoEquipo INT IDENTITY(1,1) PRIMARY KEY,
    FK_IdEquipo INT NOT NULL,
    FK_IdEmpleado INT NOT NULL,
    FK_IdRol INT NOT NULL, -- Usamos la FK de la tabla Roles
    CONSTRAINT FK_EmpleadosEquipo_Equipos
        FOREIGN KEY (FK_IdEquipo)
            REFERENCES Equipos (IdEquipo),
    CONSTRAINT FK_EmpleadosEquipo_Empleados
        FOREIGN KEY (FK_IdEmpleado)
            REFERENCES Empleados (IdEmpleado),
    CONSTRAINT FK_EmpleadosEquipo_Roles -- La FK ahora apunta a la tabla Roles
        FOREIGN KEY (FK_IdRol)
            REFERENCES Roles (IdRol)
);
GO

-- --------------------- Tabla Intermedia: EquiposProyecto ---------------------
-- Relaciona los equipos con los proyectos
CREATE TABLE EquiposProyecto (
    IdEquipoProyecto INT IDENTITY(1,1) PRIMARY KEY,
    FK_IdProyecto INT NOT NULL,
    FK_IdEquipo INT NOT NULL,
    CONSTRAINT FK_EquiposProyecto_Proyectos
        FOREIGN KEY (FK_IdProyecto)
            REFERENCES Proyectos (IdProyecto),
    CONSTRAINT FK_EquiposProyecto_Equipos
        FOREIGN KEY (FK_IdEquipo)
            REFERENCES Equipos (IdEquipo)
);
GO

-- =================================================================
-- PROCEDIMIENTOS ALMACENADOS 
-- =================================================================

-- --------------------- SPs para Proyectos ---------------------

-- Insertar Proyecto
CREATE PROCEDURE sp_InsertarProyecto
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FechaInicio DATE,
    @FechaFin DATE,
    @FK_IdEstado INT
AS
BEGIN
    INSERT INTO Proyectos (Nombre, Descripcion, FechaInicio, FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @FechaInicio, @FK_IdEstado);
END;
GO

-- Listar todos los proyectos
CREATE PROCEDURE sp_ListarProyectos
AS
BEGIN
    SELECT * FROM Proyectos;
END;
GO

-- --------------------- SPs para Equipos ---------------------

-- Insertar Equipo
CREATE PROCEDURE sp_InsertarEquipo
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @FK_IdEstado INT
AS
BEGIN
    INSERT INTO Equipos (Nombre, Descripcion, FK_IdEstado)
    VALUES (@Nombre, @Descripcion, @FK_IdEstado);
END;
GO

-- Listar todos los equipos
CREATE PROCEDURE sp_ListarEquipos
AS
BEGIN
    SELECT * FROM Equipos;
END;
GO

-- --------------------- SPs para EmpleadosEquipo  ---------------------

-- Asignar empleado a equipo con un rol existente
CREATE PROCEDURE sp_AsignarEmpleadoAEquipo
    @FK_IdEquipo INT,
    @FK_IdEmpleado INT,
    @FK_IdRol INT -- Parámetro actualizado para usar FK_IdRol
AS
BEGIN
    INSERT INTO EmpleadosEquipo (FK_IdEquipo, FK_IdEmpleado, FK_IdRol)
    VALUES (@FK_IdEquipo, @FK_IdEmpleado, @FK_IdRol);
END;
GO

-- Listar empleados de un equipo con su rol
CREATE PROCEDURE sp_ListarEmpleadosPorEquipo
    @FK_IdEquipo INT
AS
BEGIN
    SELECT e.*, r.NombreRol
    FROM Empleados e
    INNER JOIN EmpleadosEquipo ee ON ee.FK_IdEmpleado = e.IdEmpleado
    INNER JOIN Roles r ON r.IdRol = ee.FK_IdRol -- Join actualizado a la tabla Roles
    WHERE ee.FK_IdEquipo = @FK_IdEquipo;
END;
GO

-- --------------------- SPs para EquiposProyecto ---------------------

-- Asignar equipo a proyecto
CREATE PROCEDURE sp_AsignarEquipoAProyecto
    @FK_IdProyecto INT,
    @FK_IdEquipo INT
AS
BEGIN
    INSERT INTO EquiposProyecto (FK_IdProyecto, FK_IdEquipo)
    VALUES (@FK_IdProyecto, @FK_IdEquipo);
END;
GO

-- Listar equipos de un proyecto
CREATE PROCEDURE sp_ListarEquiposPorProyecto
    @FK_IdProyecto INT
AS
BEGIN
    SELECT eq.*
    FROM Equipos eq
    INNER JOIN EquiposProyecto ep ON ep.FK_IdEquipo = eq.IdEquipo
    WHERE ep.FK_IdProyecto = @FK_IdProyecto;
END;
GO

----------- Sección de Inserts -----------------------


-- Inserciones de prueba para la tabla Estados
INSERT INTO Estados (Estado, Descripcion)
VALUES 
('Activo', 'Empleado laborando normalmente'),
('Pendiente', 'Solicitud en espera de aprobación'),
('De Vacaciones', 'Empleado en período de vacaciones'),
('Inactivo', 'Empleado no activo en el sistema'),
('Rechazado', 'Solicitud de vacaciones denegada'),
('Aprobado', 'Solicitud de vacaciones aceptada'),
('En Proceso', 'Solicitud de vacaciones en revisión'),
('Suspendido', 'Empleado suspendido temporalmente'),
('Eliminado', 'El estado original fue eliminado y se reasignó este por defecto');
GO

-- Inserciones de prueba para la tabla Empresas
INSERT INTO Empresas (Nombre, Descripcion, Codigo, FK_IdEstado)
VALUES ('DigitalGeko, S.A.', 'Empresa de soluciones tecnológicas y desarrollo de software.', 'DG001', 1);
GO

-- Inserciones de prueba para la tabla Sistemas
INSERT INTO Sistemas (Nombre, Descripcion, Codigo, FK_IdEmpresa, FK_IdEstado)
VALUES 
('Sistema de Seguridad', 'Sistema para la administración de usuarios, roles y accesos, garantizando la seguridad y control dentro de la organización.', 'SS001', 1, 1),
('Sistema Vacacional', 'Plataforma para administrar y controlar las solicitudes, aprobaciones y el historial de vacaciones de los empleados de la empresa.', 'SV001', 1, 1);
GO

-- Inserciones de prueba para la tabla Modulos
INSERT INTO Modulos (Nombre, Descripcion, FK_IdEstado)
VALUES 
('Departamentos', 'Gestión de departamentos de la empresa', 1),
('Empleados', 'Gestión de empleados', 1),
('Usuarios', 'Gestión de usuarios', 1),
('Permisos', 'Gestión de permisos del usuario', 1),
('Roles', 'Gestión de roles del usuario según sus permisos', 1);
GO

-- Sistema de Seguridad
EXEC sp_AsignarModuloASistema @FK_IdSistema = 1, @FK_IdModulo = 1; -- Departamentos
EXEC sp_AsignarModuloASistema @FK_IdSistema = 1, @FK_IdModulo = 2; -- Empleados
EXEC sp_AsignarModuloASistema @FK_IdSistema = 1, @FK_IdModulo = 3; -- Usuarios
EXEC sp_AsignarModuloASistema @FK_IdSistema = 1, @FK_IdModulo = 4; -- Permisos
EXEC sp_AsignarModuloASistema @FK_IdSistema = 1, @FK_IdModulo = 5; -- Roles

-- Sistema Vacacional
EXEC sp_AsignarModuloASistema @FK_IdSistema = 2, @FK_IdModulo = 2; -- Empleados
GO

-- Inserciones de prueba para la tabla Departamentos
INSERT INTO Departamentos (Nombre, Descripcion, Codigo, FK_IdEstado)
VALUES 
('Recursos Humanos', 'Departamento encargado de la gestión del personal.', 'RH001', 1),
('Tecnología de la Información', 'Departamento responsable de sistemas y tecnología.', 'TI001', 1),
('Finanzas', 'Departamento de contabilidad y finanzas.', 'FIN001', 1),
('Marketing', 'Departamento encargado de la publicidad y el marketing.', 'MK001', 1),
('Operaciones', 'Departamento encargado de la logística y operaciones diarias.', 'OP001', 1);
GO

-- Inserciones de prueba para la tabla DepartamentosEmpresa
INSERT INTO DepartamentosEmpresa (FK_IdDepartamento,FK_IdEmpresa)
VALUES (1, 1);
GO

-- Inserciones de prueba para la tabla Empleados
INSERT INTO Empleados (Pais, DPI, NombresEmpleado, ApellidosEmpleado, CorreoPersonal, CorreoInstitucional, FechaNacimiento, Telefono, NIT, Genero, Salario, FK_IdEstado)
VALUES ('Guatemala','1234567890123', 'Juan', 'Pérez', 'juanperez@gmail.com', 'juan.perez@empresa.com', '1990-01-15', 5551234, '1234567-8', 'Masculino', 4500.00, 1);
GO

UPDATE Empleados SET FK_IdEstado = 1 WHERE IdEmpleado = 1;
GO

UPDATE Usuarios SET FK_IdEstado = 1 WHERE IdUsuario = 1;
GO

-- Inserciones de prueba para la asignación de Empleados y Empresa
INSERT INTO EmpleadosEmpresa (FK_IdEmpresa, FK_IdEmpleado) 
VALUES (1,1);
GO

-- 1 Insertar el encabezado para la solicitud inicial
INSERT INTO SolicitudEncabezado (FK_IdEmpleado, DiasSolicitadosTotal, FK_IdEstadoSolicitud)
VALUES (1, 5.00, 1);  -- 1 = Empleado, 5.00 = días solicitados, 1 = Estado 'Pendiente'
GO

-- 2 Insertar varios detalles para la misma solicitud
INSERT INTO SolicitudDetalle (FK_IdSolicitud, FechaInicio, FechaFin, DiasHabilesTomados)
VALUES
    (1, '2025-08-01', '2025-08-05', 3.00),  -- Primer período: 3 días hábiles
    (1, '2025-08-10', '2025-08-12', 2.00);  -- Segundo período: 2 días hábiles
GO

-- Inserciones de prueba para la tabla Roles
INSERT INTO Roles (NombreRol, FK_IdEstado)
VALUES ('SuperAdministrador', 1), ('Visualizador', 1), ('Autorizador', 1), ('TeamLider', 1), ('SubTeamLider', 1), ('Empleado', 1);
GO


----PROYECTOS
-- Insert para la tabla Proyectos
INSERT INTO Proyectos (Nombre, Descripcion, FechaInicio,  FK_IdEstado)
VALUES
('TPP', NULL, NULL, 1),
('TOM', NULL, NULL, 1),
('Let´s Advertise', NULL, NULL, 1),
('Easy Go', NULL, NULL, 1),
('GDG', NULL, NULL, 1),
('Anemona', NULL, NULL, 1),
('SIETE', NULL, NULL, 1),
('SCC', NULL, NULL, 1),
('RRHH', NULL, NULL, 1),
('Prometheus', NULL, NULL, 1);
GO

-- Inserciones de prueba para la tabla Permisos
-- INSERT INTO Permisos (NombrePermiso, Descripcion, FK_IdEstado)
-- VALUES ('Crear Empleado', 'Permite crear nuevos empleados', 1);
-- GO

-- Inserciones de prueba para la tabla Rol Permisos
-- INSERT INTO RolPermisos (FK_IdRol, FK_IdPermiso, FK_IdSistema)
-- VALUES (1, 1, 1);
-- GO

 -- Inserciones de prueba para la tabla Usuarios Rol
INSERT INTO UsuariosRol (FK_IdUsuario, FK_IdRol)
VALUES( 1, 1);
GO

SELECT * FROM Estados;
SELECT * FROM Empresas;
SELECT * FROM Sistemas;
SELECT * FROM Modulos;
SELECT * FROM ModulosSistema;
SELECT * FROM Departamentos;
SELECT * FROM DepartamentosEmpresa;
SELECT * FROM Usuarios;
SELECT * FROM Empleados;
SELECT * FROM EmpleadosEmpresa;
SELECT * FROM EmpleadosDepartamento;
SELECT * FROM SolicitudEncabezado;
SELECT * FROM SolicitudDetalle;
-- SELECT * FROM Permisos;
SELECT * FROM Roles;
-- SELECT * FROM RolPermisos;
SELECT * FROM UsuariosRol;
SELECT * FROM Logs;
SELECT * FROM Bitacora;

SELECT m.*
FROM Modulos m
JOIN ModulosSistema  sm ON m.IdModulo = sm.FK_IdModulo
WHERE sm.FK_IdSistema = 1
  AND m.FK_IdEstado = 1 -- Solo activos
GO

/*ErickDev*/
/*-------------*/
-- tabla de relación entre empleados y sus autorizadores
CREATE TABLE EmpleadosAutorizadores (
    IdEmpleadoAutorizador INT IDENTITY(1,1) PRIMARY KEY,
    FK_IdEmpleado INT NOT NULL,
    FK_IdAutorizador INT NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    FK_IdEstado INT DEFAULT 1,
    
    CONSTRAINT FK_EmpleadosAutorizadores_Empleado 
        FOREIGN KEY (FK_IdEmpleado) REFERENCES Empleados(IdEmpleado),
    CONSTRAINT FK_EmpleadosAutorizadores_Autorizador 
        FOREIGN KEY (FK_IdAutorizador) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_EmpleadosAutorizadores_Estado 
        FOREIGN KEY (FK_IdEstado) REFERENCES Estados(IdEstado)
);
GO
/*-----*/
/*End ErickDev*/

 ------------------- Sección de Triggers ------------------------------
 -- Empleados
 -- Trigger para actualizar días de vacaciones al insertar o actualizar empleado
-- Solo actualiza si el valor actual es 0 (respeta valores manuales)
CREATE TRIGGER tr_ActualizarDiasVacaciones
ON Empleados
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Solo actualizar días de vacaciones si el valor actual es 0 (valor por defecto)
    -- Esto permite mantener valores manuales para empleados migrados
    UPDATE e
    SET DiasVacacionesAcumulados = dbo.fn_CalcularDiasVacacionesAcumulados(e.FechaIngreso)
    FROM Empleados e
    INNER JOIN inserted i ON e.IdEmpleado = i.IdEmpleado
    WHERE e.DiasVacacionesAcumulados = 0.00; -- Solo si no ha sido establecido manualmente
END;
GO

-- Actualizar días de vacaciones para todos los empleados existentes
EXEC sp_ActualizarDiasVacacionesEmpleados;
GO

EXEC sp_ListarSolicitudEncabezado_Autorizador_Admin;
GO

--Notificaciones para recursos humanos
---ErickDev----
Create PROCEDURE sp_ObtenerAlertasEmpleadosVacaciones_Completo
AS
BEGIN
    SET NOCOUNT ON;

    WITH CalculoVacacionesCompleto AS (
        SELECT 
            e.IdEmpleado,
            e.NombresEmpleado,
            e.ApellidosEmpleado,
            e.Codigo,
            e.FechaIngreso,
            e.FK_IdEstado,
            
            -- 1. CALCULAR años trabajados (con decimales para precisión)
            CAST(DATEDIFF(DAY, e.FechaIngreso, GETDATE()) AS DECIMAL(10,2)) / 365.25 AS AniosTrabajados,
            
            -- 2. CALCULAR días acumulados total (años * 15 días por año)
            (CAST(DATEDIFF(DAY, e.FechaIngreso, GETDATE()) AS DECIMAL(10,2)) / 365.25) * 15 AS DiasAcumuladosTotal,
            
            -- 3. CALCULAR días ya tomados (suma de solicitudes aprobadas/vigentes/finalizadas)
            ISNULL((
                SELECT SUM(se.DiasSolicitadosTotal)
                FROM SolicitudEncabezado se
                WHERE se.FK_IdEmpleado = e.IdEmpleado
                AND se.FK_IdEstadoSolicitud IN (2, 3, 5) -- 2=Autorizada, 3=Vigente, 5=Finalizada
            ), 0) AS DiasYaTomados
            
        FROM Empleados e
        WHERE e.FK_IdEstado = 1 -- Solo empleados activos
        AND DATEDIFF(DAY, e.FechaIngreso, GETDATE()) > 0 -- Que tengan al menos 1 día trabajado
    ),
    
    CalculoFinal AS (
        SELECT 
            *,
            -- 4. CALCULAR días disponibles (acumulados - tomados)
            DiasAcumuladosTotal - DiasYaTomados AS DiasDisponibles
        FROM CalculoVacacionesCompleto
    )
    
    -- RESULTADO: Empleados activos con más de 14 días disponibles
    SELECT 
        IdEmpleado,
        NombresEmpleado,
        ApellidosEmpleado,
        Codigo,
        FechaIngreso,
        CAST(AniosTrabajados AS DECIMAL(10,1)) AS AniosTrabajados,
        CAST(DiasAcumuladosTotal AS DECIMAL(10,1)) AS DiasAcumuladosTotal,
        CAST(DiasYaTomados AS DECIMAL(10,1)) AS DiasYaTomados,
        CAST(DiasDisponibles AS DECIMAL(10,1)) AS DiasDisponibles,
        'Empleado con más de 14 días de vacaciones disponibles' AS TipoNotificacion
    FROM CalculoFinal
    WHERE DiasDisponibles > 14.0 -- ALERTA cuando tenga más de 14 días disponibles
    
    UNION ALL
    
    -- ADICIONAL: Empleados próximos a salir que tomaron vacaciones
    SELECT 
        e.IdEmpleado,
        e.NombresEmpleado,
        e.ApellidosEmpleado,
        e.Codigo,
        e.FechaIngreso,
        0 as AniosTrabajados,
        0 as DiasAcumuladosTotal,
        0 as DiasYaTomados,
        0 as DiasDisponibles,
        'Empleado próximo a salir (con vacaciones tomadas)' AS TipoNotificacion
    FROM Empleados e
    WHERE e.FK_IdEstado <> 1 -- No activos
    AND EXISTS (
        SELECT 1 FROM SolicitudEncabezado se 
        WHERE se.FK_IdEmpleado = e.IdEmpleado 
        AND se.FK_IdEstadoSolicitud IN (2,3,5)
        AND se.DiasSolicitadosTotal > 0
    )
    
    ORDER BY DiasDisponibles DESC;
END;
GO
