

CREATE DATABASE SistemaSeguridad;
GO

USE SistemaSeguridad;
GO

-- CREACIÓN DE TABLAS

CREATE TABLE Estado_Usuario(
    id_estado INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL UNIQUE,
    fecha_creacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1, 1),
    usuario VARCHAR(50) NOT NULL UNIQUE,
    nom_usuario VARCHAR(100) NOT NULL,
    contrasenia VARCHAR(255) NOT NULL,
    fk_id_estado INT NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (fk_id_estado) REFERENCES Estado_Usuario(id_estado)
);

CREATE TABLE Sistemas(
    id_sistema INT PRIMARY KEY IDENTITY(1, 1),
    nombre_sistema VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500)
);

CREATE TABLE Permisos(
    id_permiso INT PRIMARY KEY IDENTITY(1, 1),
    fk_id_usuario INT NOT NULL,
    fk_id_sistema INT NOT NULL,
    descripcion VARCHAR(150) NOT NULL,
    FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (fk_id_sistema) REFERENCES Sistemas(id_sistema)
);

CREATE TABLE Bitacora (
    id_bitacora INT PRIMARY KEY IDENTITY (1, 1),
    fk_id_usuario  INT NOT NULL,
    accion VARCHAR(255) NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario)
);
GO


-- Alta de Usuario
CREATE PROCEDURE sp_AltaUsuario
	@usuario VARCHAR(50),
	@nom_usuario VARCHAR(100),
	@contrasenia VARCHAR(255),
	@fk_id_estado INT
AS
BEGIN
	SET NOCOUNT ON;

	
	INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
	VALUES (
		@usuario,
		@nom_usuario,
		HASHBYTES('SHA2_256', @contrasenia),
		@fk_id_estado
	);
	-- Insertar en bitácora
	INSERT INTO Bitacora (fk_id_usuario, accion)
	VALUES (
		SCOPE_IDENTITY(),
		'Se dio de alta al usuario ' + @usuario
	);
END;

----Baja de usuario
CREATE PROCEDURE sp_BajaUsuario
	@id_usuario INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Usuarios
	SET fk_id_estado = 0 
	WHERE id_usuario = @id_usuario;


	INSERT INTO Bitacora (fk_id_usuario, accion)
	VALUES (
		@id_usuario,
		'Se dio de baja al usuario con ID ' + CAST(@id_usuario AS VARCHAR)
	);
END;

---- cambio de usuario
CREATE PROCEDURE sp_ModificarUsuario
	@id_usuario INT,
	@usuario VARCHAR(50),
	@nom_usuario VARCHAR(100),
	@nueva_contrasenia VARCHAR(255),
	@fk_id_estado INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Usuarios
	SET 
		usuario = @usuario,
		nom_usuario = @nom_usuario,
		contrasenia = HASHBYTES('SHA2_256', @nueva_contrasenia),
		fk_id_estado = @fk_id_estado
	WHERE id_usuario = @id_usuario;


	INSERT INTO Bitacora (fk_id_usuario, accion)
	VALUES (
		@id_usuario,
		'Se modificó al usuario ' + @usuario
	);
END;



----- funciones Bitacora
CREATE PROCEDURE sp_InsertarBitacora
	@fk_id_usuario INT,
	@accion VARCHAR(255)
AS
BEGIN
	INSERT INTO Bitacora (fk_id_usuario, accion)
	VALUES (@fk_id_usuario, @accion);
END;

----- Encriptacion contraseña
CREATE PROCEDURE sp_EncriptarContrasenia
    @contrasenia_plana VARCHAR(255),
    @contrasenia_encriptada VARBINARY(256) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @contrasenia_encriptada = HASHBYTES('SHA2_256', @contrasenia_plana);
END;
GO
