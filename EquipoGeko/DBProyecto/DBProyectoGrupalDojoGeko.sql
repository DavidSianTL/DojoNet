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
	@FK_IdEstado INT,
    @FK_IdEmpresa INT
AS
BEGIN
    INSERT INTO Sistemas (Nombre, Descripcion, Codigo, FK_IdEstado, FK_IdEmpresa)
    VALUES (@Nombre, @Descripcion, @Codigo, @FK_IdEstado, @FK_IdEmpresa);
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
    @FK_IdEstado INT,
    @FK_IdEmpresa INT
AS
BEGIN
    UPDATE Sistemas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        FK_IdEstado = @FK_IdEstado,
        FK_IdEmpresa = @FK_IdEmpresa
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
	DPI VARCHAR(15),
	NombreEmpleado NVARCHAR (50),
	ApellidoEmpleado NVARCHAR (50),
	CorreoPersonal NVARCHAR (50),
	CorreoInstitucional NVARCHAR (50),
	FechaIngreso DATETIME DEFAULT CURRENT_TIMESTAMP,
	FechaNacimiento DATE,
	Telefono VARCHAR(20),
	NIT VARCHAR(15),
	Genero NVARCHAR (10),
	Salario DECIMAL(10, 2),
	FK_IdEstado INT,
	CONSTRAINT FK_Empleados_Estados
		FOREIGN KEY (FK_IdEstado)
			REFERENCES Estados (IdEstado)
);
GO


-----PROCEDIMIENTO EMPLEADOS--
--INSETAR EMPLEADO--
CREATE PROCEDURE sp_InsertarEmpleado
    @DPI VARCHAR(15),
    @NombreEmpleado NVARCHAR(50),
    @ApellidoEmpleado NVARCHAR(50),
    @CorreoPersonal NVARCHAR(50),
    @CorreoInstitucional NVARCHAR(50),
    @FechaNacimiento DATE,
    @Telefono VARCHAR(20),
    @NIT VARCHAR(15),
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2),
	@FK_IdEstado INT
AS
BEGIN
    INSERT INTO Empleados (
        DPI,
        NombreEmpleado,
        ApellidoEmpleado,
        CorreoPersonal,
        CorreoInstitucional,
        FechaNacimiento,
        Telefono,
        NIT,
        Genero,
        Salario,
		FK_IdEstado
    )
    VALUES (
        @DPI,
        @NombreEmpleado,
        @ApellidoEmpleado,
        @CorreoPersonal,
        @CorreoInstitucional,
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
EXEC sp_InsertarEmpleado 123456789101112, 'AdminPrueba', 'AdminPrueba', 'adminprueba@gmail.com', 'adminprueba@geko.com','2000/05/05', '12121212', '1234567891011', 'Masculino', 3500.00, null;
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
    @DPI VARCHAR(15),
    @NombreEmpleado NVARCHAR(50),
    @ApellidoEmpleado NVARCHAR(50),
    @CorreoPersonal NVARCHAR(50),
    @CorreoInstitucional NVARCHAR(50),
    @FechaNacimiento DATE,
    @Telefono VARCHAR(20),
    @NIT VARCHAR(15),
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2),
    @FK_IdEstado INT
AS
BEGIN
    UPDATE Empleados
    SET 
        DPI = @DPI,
        NombreEmpleado = @NombreEmpleado,
        ApellidoEmpleado = @ApellidoEmpleado,
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
CREATE PROCEDURE sp_BuscarEmpleadosDepartamentoPorId
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
    FK_IdEmpleado INT NOT NULL,
    FK_IdEmpresa INT NOT NULL
);
GO

    ---STORAGE PROCEDURES---

    -----------------------------------------------INSERT
    CREATE PROCEDURE sp_InsertarEmpleadosEmpresa
        @FK_IdEmpleado INT,
        @FK_IdEmpresa INT
    AS 
    BEGIN 

        INSERT INTO EmpleadosEmpresa (FK_IdEmpleado, FK_IdEmpresa)
        VALUES (@FK_IdEmpleado, @FK_IdEmpresa);
        
    END;
    GO 

    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarEmpleadosEmpresa
    AS
    BEGIN 
        SELECT * FROM EmpleadosEmpresa;
    END;
    GO

	-----------------------------------------------SELECT for IdEmpleadoEmpresa
	CREATE PROCEDURE sp_ListarEmpleadoEmpresaPorId
        @IdEmpleadoEmpresa INT
    AS
    BEGIN
        SELECT * FROM EmpleadosEmpresa WHERE IdEmpleadoEmpresa = @IdEmpleadoEmpresa;
    END;
    GO

    -----------------------------------------------SELECT for IdEmpelado

    CREATE PROCEDURE sp_ListarEmpleadoEmpresaPorEmpleado
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
VALUES ('Sistema de Gestiones', 'Sistema integral para diversas gestiones', 'ERP001', 1, 1);
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
INSERT INTO Empleados (DPI, NombreEmpleado, ApellidoEmpleado, CorreoPersonal, CorreoInstitucional, FechaNacimiento, Telefono, NIT, Genero, Salario, FK_IdEstado)
VALUES ('1234567890123', 'Juan', 'Pérez', 'juanperez@gmail.com', 'juan.perez@empresa.com', '1990-01-15', 5551234, '1234567-8', 'Masculino', 4500.00, 1);
GO

UPDATE Empleados SET FK_IdEstado = 1 WHERE IdEmpleado = 1;
GO

UPDATE Usuarios SET FK_IdEstado = 1 WHERE IdUsuario = 1;
GO

-- Inserciones de prueba para la tabla Roles
INSERT INTO Roles (NombreRol, FK_IdEstado)
VALUES ('SuperAdministrador', 1);
GO

-- Inserciones de prueba para la tabla Permisos
INSERT INTO Permisos (NombrePermiso, Descripcion, FK_IdEstado)
VALUES ('Crear Empleado', 'Permite crear nuevos empleados', 1);
GO

-- Inserciones de prueba para la tabla Rol Permisos
INSERT INTO RolPermisos (FK_IdRol, FK_IdPermiso, FK_IdSistema)
 VALUES (1, 1, 1);
GO

 -- Inserciones de prueba para la tabla Usuarios Rol
INSERT INTO UsuariosRol (FK_IdUsuario, FK_IdRol)
VALUES( 1, 1);
GO

SELECT * FROM Estados;
SELECT * FROM Empresas;
SELECT * FROM Sistemas;
SELECT * FROM Departamentos;
SELECT * FROM DepartamentosEmpresa;
SELECT * FROM Usuarios;
SELECT * FROM Empleados;
SELECT * FROM Permisos;
SELECT * FROM Roles;
SELECT * FROM RolPermisos;
SELECT * FROM UsuariosRol;
SELECT * FROM Logs;
SELECT * FROM Bitacora;
