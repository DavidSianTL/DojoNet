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
	FechaCreacion DATE DEFAULT CURRENT_TIMESTAMP,
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
-- Tabla de Bítacora
CREATE TABLE Bitacora(
	IdBitacora INT IDENTITY(1,1),
	FechaEntrada DATE DEFAULT CURRENT_TIMESTAMP,
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

-- SP para listar las Bítacoras
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


---------------------------------------------------------------------------
-- Tabla de Logs
CREATE TABLE Logs(
	IdLog INT IDENTITY(1,1),
	FechaEntrada DATE DEFAULT CURRENT_TIMESTAMP,
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