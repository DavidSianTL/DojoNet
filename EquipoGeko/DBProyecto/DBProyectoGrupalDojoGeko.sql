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


---------------------------------------------------------------------------
-- Tabla de Usuarios
CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1),
	Username VARCHAR(50) NOT NULL,
	Contrasenia VARCHAR(255) NOT NULL,
	FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	Estado BIT DEFAULT 1,
	PRIMARY KEY (IdUsuario)
);
GO

-- SP para insertar Usuarios
CREATE PROCEDURE sp_InsertarUsuario
    @Username VARCHAR(50),
    @Contrasenia VARCHAR(255)
AS
BEGIN
    INSERT INTO Usuarios (Username, Contrasenia)
    VALUES (@Username, @Contrasenia);
END;
GO

-- SP para listar los Usuarios
CREATE PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;
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
    @Estado BIT
AS
BEGIN
    UPDATE Usuarios
    SET Username = @Username,
        Contrasenia = @Contrasenia,
        Estado = @Estado
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- SP para "eliminar" un Usuario
-- Solo le cambiamos el estado para decir que se ha "eliminado"
CREATE PROCEDURE sp_EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = 0
    WHERE IdUsuario = @IdUsuario;
END;
GO


---------------------------------------------------------------------------
-- Tabla de B�tacora
CREATE TABLE Bitacora(
	IdBitacora INT IDENTITY(1,1),
	FechaEntrada DATETIME DEFAULT CURRENT_TIMESTAMP,
	FK_IdUsuario INT, 
	FK_IdSistema INT, -- As� se deben de crear las foraneas empezando con el "FK_"
	Accion NVARCHAR(75),
	Descripcion NVARCHAR(255),
	PRIMARY KEY(IdBitacora)
);
GO

-- SP para insertar B�tacora
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

-- SP para listar las B�tacoras
CREATE PROCEDURE sp_ListarBitacoras
AS
BEGIN
    SELECT * FROM Bitacora;
END;
GO

-- SP para listar por B�tacora
CREATE PROCEDURE sp_ListarBitacoraId
	@IdBitacora INT
AS
BEGIN
    SELECT * FROM Bitacora B WHERE B.IdBitacora = @IdBitacora;
END;
GO

-- SP para actualizar una B�tacora
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

-- SP para "eliminar" una B�tacora
CREATE PROCEDURE sp_EliminarBitacora
    @IdBitacora INT
AS
BEGIN
    DELETE FROM Bitacora WHERE IdBitacora = @IdBitacora;
END;
GO


---------------------------------------------------------------------------
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

-- SP para insertar Log
CREATE PROCEDURE sp_InsertarLog
    @Accion NVARCHAR(75),
    @Descripcion NVARCHAR(255),
    @Estado BIT
AS
BEGIN
    INSERT INTO Logs (Accion, Descripcion, Estado)
    VALUES (@Accion, @Descripcion, @Estado);
END;
GO

-- SP para listar los Logs
CREATE PROCEDURE sp_ListarLogs
AS
BEGIN
    SELECT * FROM Logs;
END;
GO

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

--------------------- Tabla de Empresas
CREATE TABLE Empresas (
    IdEmpresa INT IDENTITY(1,1),       
    Nombre NVARCHAR(100) NOT NULL,     
    Descripcion NVARCHAR(255),         
    Codigo NVARCHAR(50) NOT NULL,      
    Estado BIT DEFAULT 1,              
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP, 
    PRIMARY KEY (IdEmpresa)            
);
GO


---Insertar Empresa
CREATE PROCEDURE sp_InsertarEmpresa
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50)
AS
BEGIN
    INSERT INTO Empresas (Nombre, Descripcion, Codigo)
    VALUES (@Nombre, @Descripcion, @Codigo);
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
    @Estado BIT
AS
BEGIN
    UPDATE Empresas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        Estado = @Estado
    WHERE IdEmpresa = @IdEmpresa;
END;
GO



--Eliminar una Empresa 
CREATE PROCEDURE sp_EliminarEmpresa
    @IdEmpresa INT
AS
BEGIN
    UPDATE Empresas
    SET Estado = 0
    WHERE IdEmpresa = @IdEmpresa;
END;
GO





-----------Tabla Sistemas 
CREATE TABLE Sistemas (
    IdSistema INT IDENTITY(1,1),               
    Nombre NVARCHAR(100) NOT NULL,             
    Descripcion NVARCHAR(255),                 
    Codigo NVARCHAR(50) NOT NULL,              
    Estado BIT DEFAULT 1,                      
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
    @FK_IdEmpresa INT
AS
BEGIN
    INSERT INTO Sistemas (Nombre, Descripcion, Codigo, FK_IdEmpresa)
    VALUES (@Nombre, @Descripcion, @Codigo, @FK_IdEmpresa);
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
    @Estado BIT,
    @FK_IdEmpresa INT
AS
BEGIN
    UPDATE Sistemas
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        Estado = @Estado,
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
    SET Estado = 0
    WHERE IdSistema = @IdSistema;
END;
GO


------------------Tabla Departamentos
CREATE TABLE Departamentos (
    IdDepartamento INT IDENTITY(1,1),              
    Nombre NVARCHAR(100) NOT NULL,                 
    Descripcion NVARCHAR(255),                    
    Codigo NVARCHAR(50) NOT NULL,                  
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP, 
    Estado BIT DEFAULT 1,                          
    PRIMARY KEY (IdDepartamento)
);
GO


--Insertar Departamento 
CREATE PROCEDURE sp_InsertarDepartamento
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Codigo NVARCHAR(50)
AS
BEGIN
    INSERT INTO Departamentos (Nombre, Descripcion, Codigo)
    VALUES (@Nombre, @Descripcion, @Codigo);
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
    @Estado BIT
AS
BEGIN
    UPDATE Departamentos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Codigo = @Codigo,
        Estado = @Estado
    WHERE IdDepartamento = @IdDepartamento;
END;
GO



--Elimar Departamento 
CREATE PROCEDURE sp_EliminarDepartamento
    @IdDepartamento INT
AS
BEGIN
    UPDATE Departamentos
    SET Estado = 0
    WHERE IdDepartamento = @IdDepartamento;
END;
GO

---------------------------------------------------------------------------
-- Tabla de Usuarios
CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1),
	Username VARCHAR(50) NOT NULL,
	Contrasenia VARCHAR(255) NOT NULL,
	FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	Estado BIT DEFAULT 1,
	PRIMARY KEY (IdUsuario)
);
GO



----------------------------------------------- PARTE DANIEL ------
-- Creamos la DB
CREATE DATABASE DBProyectoGrupalDojoGeko;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1),
	Username VARCHAR(50) NOT NULL,
	Contrasenia VARCHAR(255) NOT NULL,
	FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	Estado BIT DEFAULT 1,
	PRIMARY KEY (IdUsuario)
);
GO

-- SP para insertar Usuarios
CREATE PROCEDURE sp_InsertarUsuario
    @Username VARCHAR(50),
    @Contrasenia VARCHAR(255)
AS
BEGIN
    INSERT INTO Usuarios (Username, Contrasenia)
    VALUES (@Username, @Contrasenia);
END;
GO

-- SP para listar los Usuarios
CREATE PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;
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
    @Estado BIT
AS
BEGIN
    UPDATE Usuarios
    SET Username = @Username,
        Contrasenia = @Contrasenia,
        Estado = @Estado
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- SP para "eliminar" un Usuario
-- Solo le cambiamos el estado para decir que se ha "eliminado"
CREATE PROCEDURE sp_EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    UPDATE Usuarios
    SET Estado = 0
    WHERE IdUsuario = @IdUsuario;
END;
GO


---Creacion tabla Roles-----
CREATE TABLE Roles (
	IdRol INT IDENTITY (1,1) PRIMARY KEY,
	NombreRol NVARCHAR (50),
	Estado BIT
);

--PROCEDIMIENTOS ALMACENADOS ROLES--
--SP Insertar Rol
CREATE PROCEDURE InsertarRol
    @NombreRol NVARCHAR(50),
    @Estado BIT
AS
BEGIN
    INSERT INTO Roles (NombreRol, Estado)
    VALUES (@NombreRol, @Estado);
END
GO

--SP Listar Roles
CREATE PROCEDURE ListarRoles
AS
BEGIN
    SELECT * FROM Roles;
END
GO

--SP Actualizar Rol
CREATE PROCEDURE ActualizarRol
    @IdRol INT,
    @NombreRol NVARCHAR(50),
    @Estado BIT
AS
BEGIN
    UPDATE Roles
    SET NombreRol = @NombreRol,
        Estado = @Estado
    WHERE IdRol = @IdRol;
END
GO

--SP Eliminar Rol
CREATE PROCEDURE EliminarRol
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
);
----PROCEDIMIENTO ALMACENADO PERMISO--
--SP Insertar Permiso
CREATE PROCEDURE InsertarPermiso
    @NombrePermiso NVARCHAR(50),
    @Descripcion NVARCHAR(50)
AS
BEGIN
    INSERT INTO Permisos (NombrePermiso, Descripcion)
    VALUES (@NombrePermiso, @Descripcion);
END
GO

--SP Listar Permisos
CREATE PROCEDURE ListarPermisos
AS
BEGIN
    SELECT * FROM Permisos;
END
GO

--SP Actualizar Permiso
CREATE PROCEDURE ActualizarPermiso
    @IdPermiso INT,
    @NombrePermiso NVARCHAR(50),
    @Descripcion NVARCHAR(50)
AS
BEGIN
    UPDATE Permisos
    SET NombrePermiso = @NombrePermiso,
        Descripcion = @Descripcion
    WHERE IdPermiso = @IdPermiso;
END
GO

-- sp Eliminar Permiso
CREATE PROCEDURE EliminarPermiso
    @IdPermiso INT
AS
BEGIN
    DELETE FROM Permisos WHERE IdPermiso = @IdPermiso;
END
GO


---Creacion tabla Empleados-----
CREATE TABLE Empleados (
	IdEmpleado INT IDENTITY (1,1) PRIMARY KEY,
	NombreEmpleado NVARCHAR (50),
	IdDepartamento INt,
	Correo NVARCHAR (50),
	FechaIngreso DATETIME DEFAULT CURRENT_TIMESTAMP,
	FechaNacimiento DATETIME DEFAULT CURRENT_TIMESTAMP,
	Telefono INT,
	Genero NVARCHAR (10),
	Salario DECIMAL(10, 2),
	Estado BIT

	---esperar la llave foranea de IdDepartamento.
);

-----PROCEDIMIENTO EMPLEADOS--
--INSETAR EMPLEADO--
CREATE PROCEDURE sp_InsertarEmpleado
    @NombreEmpleado NVARCHAR(50),
    @IdDepartamento INT,
    @Correo NVARCHAR(50),
    @FechaNacimiento DATETIME,
    @Telefono INT,
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2),
    @Estado BIT
AS
BEGIN
    INSERT INTO Empleados (
        NombreEmpleado,
        IdDepartamento,
        Correo,
        FechaNacimiento,
        Telefono,
        Genero,
        Salario,
        Estado
    )
    VALUES (
        @NombreEmpleado,
        @IdDepartamento,
        @Correo,
        @FechaNacimiento,
        @Telefono,
        @Genero,
        @Salario,
        @Estado
    );
END;


---SP LISTAR EMPLEADO--
CREATE PROCEDURE sp_ListarEmpleados
AS
BEGIN
    SELECT 
        e.IdEmpleado,
        e.NombreEmpleado,
        d.NombreDepartamento,
        e.Correo,
        e.FechaIngreso,
        e.FechaNacimiento,
        e.Telefono,
        e.Genero,
        e.Salario,
        e.Estado
    FROM Empleados e
    INNER JOIN Departamentos d ON e.IdDepartamento = d.IdDepartamento;
END;


--SP ACTUALIZAR EMPLEADO
CREATE PROCEDURE sp_ActualizarEmpleado
    @IdEmpleado INT,
    @NombreEmpleado NVARCHAR(50),
    @IdDepartamento INT,
    @Correo NVARCHAR(50),
    @FechaNacimiento DATETIME,
    @Telefono INT,
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2),
    @Estado BIT
AS
BEGIN
    UPDATE Empleados
    SET 
        NombreEmpleado = @NombreEmpleado,
        IdDepartamento = @IdDepartamento,
        Correo = @Correo,
        FechaNacimiento = @FechaNacimiento,
        Telefono = @Telefono,
        Genero = @Genero,
        Salario = @Salario,
        Estado = @Estado
    WHERE IdEmpleado = @IdEmpleado;
END;



--SP ELIMINAR EMPLEADO
CREATE PROCEDURE sp_EliminarEmpleado
    @IdEmpleado INT
AS
BEGIN
    DELETE FROM Empleados
    WHERE IdEmpleado = @IdEmpleado;
END;




-------------@Junior------------------------------- RELACIONES N:N -----------------------------------------------------------------------------------------------

GO 
USE DBProyectoGrupalDojoGeko;

CREATE TABLE EmpleadosEmpresa(
    IdEmpleadosEmpresa INT PRIMARY KEY IDENTITY(1,1),
    FK_IdEmpleado INT NOT NULL,
    FK_IdEmpresa INT NOT NULL,
    
    FOREIGN KEY (FK_IdEmpleado) REFERENCES Empleados(IdEmpleado),
    FOREIGN KEY (FK_IdEmpresa) REFERENCES Empresas(IdEmpresa)
);


    ---STORAGE PROCEDURES---

    -----------------------------------------------CREATE
    CREATE PROCEDURE sp_InsertarEmpleadosEmpresa
        @FK_IdEmpleado INT,
        @FK_IdEmpresa INT

    AS 
    BEGIN 

        IF NOT EXISTS(
            SELECT 1 FROM EmpleadosEmpresa
            WHERE FK_IdEmpleado = @FK_IdEmpleado AND FK_IdEmpresa = @FK_IdEmpresa
        )
        BEGIN 
            INSERT INTO EmpleadosEmpresa (FK_IdEmpleado, FK_IdEmpresa)
            VALUES (@FK_IdEmpleado, @FK_IdEmpresa);
        END
        
        ELSE
        BEGIN
            PRINT "LA RELACION YA EXISTE. ";
        END
    END;
    GO 

    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarEmpleadosEmpresa
    AS
    BEGIN 
        SELECT * FROM EmpleadosEmpresa;
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


    -----------------------------------------------DELETE

    CREATE PROCEDURE sp_EliminarRelacion
        @FK_IdEmpleado INT,
        @FK_IdEmpresa INT

    AS 
    BEGIN 
        DELETE FROM EmpleadosEmpresa
        WHERE FK_IdEmpleado = @FK_IdEmpleado AND FK_IdEmpresa = @FK_IdEmpresa;
    END;
    GO



CREATE TABLE UsuariosRol(
    IdUsuariosRol INT PRIMARY KEY IDENTITY(1,1),
    FK_IdUsuario INT NOT NULL,
    FK_IdRol INT NOT NULL,
    
    FOREIGN KEY (FK_IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (FK_IdRol) REFERENCES Roles(IdRol)
);

    ------------------STORAGE PROCEDURES---------------------------

    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarUsuariosRol
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol;
    END;
    GO


    -----------------------------------------------SELECT for IdUsuario

    CREATE PROCEDURE sp_ListarUsuariosRolPorUsuario
        @FK_IdUsuario INT
    AS 
    BEGIN 
        SELECT * FROM UsuariosRol
        WHERE FK_IdUsuario = @FK_IdUsuario;
    END;
    GO


    -----------------------------------------------SELECT for Rol

    CREATE PROCEDURE sp_ListarUsuariosRolPorRol
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
            PRINT "LA RELACION YA EXISTE.";
        END
    END;
    GO

    
    -----------------------------------------------DELETE 
    CREATE PROCEDURE sp_EliminarUsuariosRol
        @FK_IdUsuario INT,
        @FK_IdRol INT
    AS 
    BEGIN 
        DELETE FROM UsuariosRol
        WHERE FK_IdUsuario = @FK_IdUsuario AND FK_IdRol = @FK_IdRol;
    END;
    GO




CREATE TABLE RolPermisos(
    IdRolPermiso INT PRIMARY KEY IDENTITY(1,1),
    FK_IdRol INT NOT NULL, 
    FK_IdPermiso INT NOT NULL,

    FOREIGN KEY (FK_IdRol) REFERENCES  Roles(IdRol),
    FOREIGN KEY (FK_IdPermiso) REFERENCES  Permisos(IdPermiso)
);

    --------------STORAGE PROCEDURES------------------------------------
    -----------------------------------------------SELECT
    CREATE PROCEDURE sp_ListarRolPermiso
    AS 
    BEGIN 
        SELECT * FROM RolPermisos; 
    END;
    GO

    -----------------------------------------------SELECT for IdRol
    CREATE PROCEDURE sp_ListarRolPermisoPorRol
        @FK_IdRol INT
    AS 
    BEGIN 
        SELECT * FROM RolPermisos
        WHERE FK_IdRol = @FK_IdRol;
    END;
    GO


    -----------------------------------------------SELECT for IdPermiso
    CREATE PROCEDURE sp_ListarRolPermisoPorPermiso
        @FK_IdPermiso INT
    AS 
    BEGIN 
        SELECT * FROM RolPermisos
        WHERE FK_IdPermiso = @FK_IdPermiso;
    END;
    GO

    -----------------------------------------------INSERT
    CREATE PROCEDURE sp_InsertarRolPermiso
        @FK_IdRol INT,
        @FK_IdPermiso INT
    AS 
    BEGIN 
        IF NOT EXISTS(
            SELECT 1 FROM RolPermisos
            WHERE FK_IdRol = @FK_IdRol AND FK_IdPermiso = @FK_IdPermiso
        )
        BEGIN
            INSERT INTO RolPermisos (FK_IdRol, FK_IdPermiso)
            VALUES (@FK_IdRol, @FK_IdPermiso);
        END

        ELSE 
        BEGIN 
            PRINT "LA RELACION YA EXISTE. ";
        END
    END;
    GO

    -----------------------------------------------DELETE
    CREATE PROCEDURE sp_EliminarRolPermiso
        @FK_IdRol INT, 
        @FK_IdPermiso INT 
    AS 
    BEGIN 
        DELETE FROM RolPermisos 
        WHERE FK_IdRol = @FK_IdRol AND FK_IdPermiso = @FK_IdPermiso;
    END;
    GO

