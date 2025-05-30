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

---------------------@Carlos-----------------------------------
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
	Telefono INT,
	NIT VARCHAR(15),
	Genero NVARCHAR (10),
	Salario DECIMAL(10, 2),
	Estado BIT DEFAULT 1
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
    @Telefono INT,
    @NIT VARCHAR(15),
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2)
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
        Salario
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
        @Salario
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
EXEC sp_InsertarEmpleado 11111111111111, 'AdminPrueba', 'AdminPrueba', 'adminprueba@gmail.com', 'adminprueba@geko.com','2000/05/05', '12121212', '111111111', 'Masculino', 3500.00;
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
    @Telefono INT,
    @NIT VARCHAR(15),
    @Genero NVARCHAR(10),
    @Salario DECIMAL(10, 2),
    @Estado BIT
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
        Estado = @Estado
    WHERE IdEmpleado = @IdEmpleado;
END;
GO



--SP ELIMINAR EMPLEADO
CREATE PROCEDURE sp_EliminarEmpleado
    @IdEmpleado INT
AS
BEGIN
    DELETE FROM Empleados
    WHERE IdEmpleado = @IdEmpleado;
END;
GO

-----------------------@José----------------------------------------------------
-- Tabla de Usuarios
CREATE TABLE Usuarios(
	IdUsuario INT IDENTITY(1,1),
	Username VARCHAR(50) NOT NULL,
	Contrasenia VARCHAR(255) NOT NULL,
	FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	Estado BIT DEFAULT 1,
	FK_IdEmpleado INT NOT NULL,
	PRIMARY KEY (IdUsuario),
	CONSTRAINT FK_Usuarios_Empleados
		FOREIGN KEY (FK_IdEmpleado)
			REFERENCES Empleados(IdEmpleado)
);
GO

-- SP para insertar Usuarios
CREATE PROCEDURE sp_InsertarUsuario
    @Username VARCHAR(50),
    @Contrasenia VARCHAR(255),
	@FK_IdEmpleado INT
AS
BEGIN
    INSERT INTO Usuarios (Username, Contrasenia, FK_IdEmpleado)
    VALUES (@Username, @Contrasenia, @FK_IdEmpleado);
END;
GO

-- SP para listar los Usuarios
CREATE PROCEDURE sp_ListarUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;
GO

-- Datos de prueba
EXEC sp_InsertarUsuario AdminDev, 12345678, 1;
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
    @Estado BIT,
	@FK_IdEmpleado INT
AS
BEGIN
    UPDATE Usuarios
    SET Username = @Username,
        Contrasenia = @Contrasenia,
        Estado = @Estado,
		FK_IdEmpleado = @FK_IdEmpleado
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


-- Tabla de Token por Usuario
CREATE TABLE TokenUsuario(
	IdTokenUsuario INT IDENTITY(1,1),
	FechaCreacion DATETIME NOT NULL,
	Token VARCHAR(255) NOT NULL,
	TiempoExpira DATETIME NOT NULL,
	FK_IdUsuario INT NOT NULL,
	PRIMARY KEY(IdTokenUsuario),
	CONSTRAINT FK_TokenUsuario_Usuario
		FOREIGN KEY(FK_IdUsuario)
			REFERENCES Usuarios(IdUsuario)
);
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

-- Sección de pruebas
SELECT IdUsuario, Username, contrasenia, Estado, FK_IdEmpleado
FROM Usuarios
WHERE Username = 'AdminDev' AND Estado = 1;
GO
EXEC sp_ListarLogs
GO
-- EXEC sp_ListarBitacoras
GO	
SELECT * FROM TokenUsuario;
GO
SELECT * FROM Usuarios;
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

-----------------------@Carlos----------------------------------------------------
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



---------------------@Daniel-----------------------------------
---Creacion tabla Roles-----
CREATE TABLE Roles (
	IdRol INT IDENTITY (1,1) PRIMARY KEY,
	NombreRol NVARCHAR (50),
	Estado BIT
);
GO

--PROCEDIMIENTOS ALMACENADOS ROLES--
--SP Insertar Rol
CREATE PROCEDURE sp_InsertarRol
    @NombreRol NVARCHAR(50),
    @Estado BIT
AS
BEGIN
    INSERT INTO Roles (NombreRol, Estado)
    VALUES (@NombreRol, @Estado);
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
);
GO

----PROCEDIMIENTO ALMACENADO PERMISO--
--SP Insertar Permiso
CREATE PROCEDURE sp_InsertarPermiso
    @NombrePermiso NVARCHAR(50),
    @Descripcion NVARCHAR(50)
AS
BEGIN
    INSERT INTO Permisos (NombrePermiso, Descripcion)
    VALUES (@NombrePermiso, @Descripcion);
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
	@IdPermiso int
AS
BEGIN
    SELECT * FROM Permisos P WHERE P.IdPermiso = @IdPermiso;
END
GO

--SP Actualizar Permiso
CREATE PROCEDURE sp_ActualizarPermiso
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
CREATE PROCEDURE sp_EliminarPermiso
    @IdPermiso INT
AS
BEGIN
    DELETE FROM Permisos WHERE IdPermiso = @IdPermiso;
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



-------------@Junior------------------------------- RELACIONES N:N -----------------------------------------------------------------------------------------------
-- Relación de Empleados con Departamento
CREATE TABLE EmpleadosDepartamento(
	IdEmpleadosDepartamento INT PRIMARY KEY IDENTITY(1,1),
	FK_IdDepartamento INT NOT NULL,
	FK_IdEmpleado INT NOT NULL,
	CONSTRAINT FK_EmpleadosDepartamento_Departamento
		FOREIGN KEY (FK_IdDepartamento)
			REFERENCES Departamentos (IdDepartamento),
	CONSTRAINT FK_EmpleadosDepartamento_Empleado
		FOREIGN KEY (FK_IdEmpleado)
			REFERENCES Empleados (IdEmpleado)
);
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

-- SP para insertar la relación
CREATE PROCEDURE sp_InsertarSistemasEmpresa
	@FK_IdEmpresa INT,
	@FK_IdSistema INT
AS
BEGIN
	INSERT INTO SistemasEmpresa (FK_IdEmpresa, FK_IdSistema)
		VALUES (@FK_IdEmpresa, @FK_IdSistema)
END;
GO


/*CREATE TABLE EmpleadosEmpresa(
    IdEmpleadosEmpresa INT PRIMARY KEY IDENTITY(1,1),
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
*/

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
            PRINT 'LA RELACION YA EXISTE.';
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
    -----------------------------------------------SELECT for Sistema
    CREATE PROCEDURE sp_ListarRolPermisoPorEmpresa
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

    -----------------------------------------------DELETE
    CREATE PROCEDURE sp_EliminarRolPermiso
        @FK_IdRol INT, 
        @FK_IdPermiso INT,
        @FK_IdSistema INT
    AS 
    BEGIN 
        DELETE FROM RolPermisos 
        WHERE FK_IdRol = @FK_IdRol AND FK_IdPermiso = @FK_IdPermiso AND FK_IdSistema = @FK_IdSistema;
    END;
    GO

