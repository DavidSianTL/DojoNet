-- Crear la base de datos
CREATE DATABASE SistemaSeguridad;
GO

USE SistemaSeguridad;
GO

-- Tabla Estado_Usuario
CREATE TABLE Estado_Usuario(
	id_estado INT PRIMARY KEY IDENTITY(1, 1),
	descripcion VARCHAR(50) NOT NULL UNIQUE,
	fecha_creacion DATETIME DEFAULT GETDATE()
);

-- Tabla Usuarios
CREATE TABLE Usuarios(
	id_usuario INT PRIMARY KEY IDENTITY(1, 1),
	usuario VARCHAR(50) NOT NULL UNIQUE,
	nom_usuario VARCHAR (50) NOT NULL,
	contrasenia VARCHAR (255) NOT NULL,
	fk_id_estado INT NOT NULL,
	fecha_creacion DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (fk_id_estado) REFERENCES Estado_Usuario(id_estado)
);

-- Tabla Sistemas
CREATE TABLE Sistemas(
	id_sistema INT PRIMARY KEY IDENTITY(1, 1),
	nombre_sistema VARCHAR(150) NOT NULL,
	descripcion VARCHAR(500)
);

-- Tabla Permisos
CREATE TABLE Permisos(
	id_permiso INT PRIMARY KEY IDENTITY(1, 1),
	fk_id_usuario INT NOT NULL,
	fk_id_sistema INT NOT NULL,
	descripcion VARCHAR(150) NOT NULL,
	FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario),
	FOREIGN KEY (fk_id_sistema) REFERENCES Sistemas(id_sistema)
);

-- Tabla Bitacora
CREATE TABLE Bitacora(
	id_bitacora INT PRIMARY KEY IDENTITY (1, 1),
	fk_id_usuario INT NOT NULL,
	accion VARCHAR(255) NOT NULL,
	fecha DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario)
);
GO
-- Funcion para encriptar contraseñas
CREATE FUNCTION EncriptarContrasenia (@contrasenia VARCHAR(255))
RETURNS VARBINARY(64)
AS
BEGIN
    RETURN HASHBYTES('SHA2_256', CONVERT(VARBINARY(255), @contrasenia));
END;
GO

-- SP para insertar en bitacora
CREATE PROCEDURE InsertarBitacora
    @id_usuario INT,
    @accion VARCHAR(255)
AS
BEGIN
    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, @accion);
END;
GO

-- Alta de usuario
CREATE PROCEDURE AltaUsuario
    @usuario VARCHAR(50),
    @nom_usuario VARCHAR(50),
    @contrasenia VARCHAR(255),
    @fk_id_estado INT
AS
BEGIN
    DECLARE @hashed VARBINARY(64) = dbo.EncriptarContrasenia(@contrasenia);

    INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
    VALUES (@usuario, @nom_usuario, CONVERT(VARCHAR(255), @hashed), @fk_id_estado);

    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (SCOPE_IDENTITY(), 'Alta de usuario');
END;
GO

-- Baja de usuario
CREATE PROCEDURE BajaUsuario
    @id_usuario INT,
    @nuevo_estado INT 
AS
BEGIN
    UPDATE Usuarios
    SET fk_id_estado = @nuevo_estado
    WHERE id_usuario = @id_usuario;

    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, 'Baja del usuario');
END;
GO

-- Modificacion de usuario
CREATE PROCEDURE ModificarUsuario
    @id_usuario INT,
    @nuevo_nombre VARCHAR(50),
    @nuevo_estado INT
AS
BEGIN
    UPDATE Usuarios
    SET nom_usuario = @nuevo_nombre,
        fk_id_estado = @nuevo_estado
    WHERE id_usuario = @id_usuario;

    INSERT INTO Bitacora (fk_id_usuario, accion)
    VALUES (@id_usuario, 'Modificacion de usuario');
END;
GO
