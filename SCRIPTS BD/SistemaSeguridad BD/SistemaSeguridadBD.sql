CREATE DATABASE SistemaSeguridad;
GO

USE SistemaSeguridad
GO

--SISTEMA DE SEGURIDAD PARA GEKO

CREATE TABLE Estado_Usuario(
	Id_Estado INT PRIMARY KEY IDENTITY(1,1),
	Descripcion VARCHAR(50) NOT NULL UNIQUE,
	Fecha_Creacion DATETIME DEFAULT GETDATE()
);

CREATE TABLE Usuarios(
	Id_Usuario INT PRIMARY KEY IDENTITY(1,1),
	Usuario VARCHAR(50) NOT NULL UNIQUE,
	Nom_Usuario VARCHAR(100) NOT NULL,
	Contrasenia VARBINARY(255) NOT NULL,
	FK_Id_Estado INT NOT NULL,
	Fecha_Creacion DATETIME DEFAULT GETDATE()
	FOREIGN KEY (FK_Id_Estado) REFERENCES Estado_Usuario(Id_Estado)
);

CREATE TABLE Sistemas(
	Id_Sistema INT PRIMARY KEY IDENTITY(1,1),
	Nombre_Sistema VARCHAR(150) NOT NULL,
	Descripcion VARCHAR(500)
);

CREATE TABLE Permisos(
	Id_Permiso INT PRIMARY KEY IDENTITY(1,1),
	FK_Id_Usuario INT NOT NULL,
	FK_Id_Sistema INT NOT NULL,
	Descripcion VARCHAR (150) NOT NULL,
	FOREIGN KEY (FK_Id_Usuario) REFERENCES Usuarios(Id_Usuario),
	FOREIGN KEY (FK_Id_Sistema) REFERENCES Sistemas(Id_Sistema)
);

CREATE TABLE Bitacora(
	Id_Bitacora INT PRIMARY KEY IDENTITY(1,1),
	FK_Id_Usuario INT NOT NULL,
	Accion VARCHAR(255) NOT NULL,
	Fecha DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (FK_Id_Usuario) REFERENCES Usuarios(Id_Usuario)
);
GO

-- INSERT CON CONTRASEÑA ENCRIPTADA
INSERT INTO Usuarios (Usuario, Nom_Usuario, Contrasenia, FK_Id_Estado) VALUES 
('jlopez', 'Juan López', ENCRYPTBYPASSPHRASE('password','Jlopez123'), 1),
('mgarcia', 'María García', ENCRYPTBYPASSPHRASE('password', 'MariaGarcia567'), 2);
