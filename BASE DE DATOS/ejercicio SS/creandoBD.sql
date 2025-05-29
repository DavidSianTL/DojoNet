-- Creación de la base de datos
CREATE DATABASE SistemaSeguridad;
GO
USE SistemaSeguridad;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    estado VARCHAR(20) DEFAULT 'Activo',
    fecha_creacion DATETIME DEFAULT GETDATE()
);
GO

-- Tabla de Sistemas
CREATE TABLE Sistemas (
    id_sistema INT PRIMARY KEY IDENTITY(1,1),
    nombre_sistema VARCHAR(100) NOT NULL,
    descripcion VARCHAR(MAX)
);
GO

-- Tabla de Permisos
CREATE TABLE Permisos (
    id_permiso INT PRIMARY KEY IDENTITY(1,1),
    id_usuario INT NOT NULL,
    id_sistema INT NOT NULL,
    permiso VARCHAR(50),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_sistema) REFERENCES Sistemas(id_sistema)
);
GO

-- Tabla de Bitácora
CREATE TABLE Bitacora (
    id_bitacora INT PRIMARY KEY IDENTITY(1,1),
    id_usuario INT NOT NULL,
    accion VARCHAR(255),
    fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
);
GO

