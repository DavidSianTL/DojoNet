CREATE DATABASE SistemaSeguridad;
Go

USE	SistemaSeguridad
go
CREATE TABLE Estado_Usuario(
	id_estado INT PRIMARY KEY IDENTITY(1,1),
	descripcion VARCHAR(50) NOT NULL UNIQUE,
	fecha_creacion DATETIME DEFAULT GETDATE()
	);

CREATE TABLE Usuarios(
	id_usuario INT Primary Key IDENTITY(1,1),
	usuario		VARCHAR(50) NOT NULL UNIQUE,
	nom_usuario VARCHAR(50) NOT NULL UNIQUE,
	contrasenia VARCHAR(255) NOT NULL,
	fk_id_estado INT NOT NULL,
	fecha_creacion DATETIME DEFAULT GETDATE()
	FOREIGN KEY (fk_id_estado) REFERENCES Estado_Usuario(id_estado)
);



CREATE TABLE Sistemas(
	id_sistema INT PRIMARY KEY IDENTITY(1,1),
	nombre_sistema VARCHAR (150) NOT NULL,
	descripcion VARCHAR (500), 
);

CREATE TABLE Permisos(
	id_permiso INT PRIMARY KEY IDENTITY(1,1),
	fk_id_usuario INT NOT NULL,
	fk_id_sistema INT NOT NULL,
	descripcion VARCHAR(150) NOT NULL,
	FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario),
	FOREIGN KEY (fk_id_sistema) REFERENCES Sistemas(id_sistema)
);

CREATE TABLE Bitacora(
	id_bitacora INT PRIMARY KEY IDENTITY (1,1),
	fk_id_usuario INT NOT NULL,
	accion VARCHAR (255) NOT NULL,
	fecha DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (fk_id_usuario)	REFERENCES Usuarios(id_usuario)
);

GO 