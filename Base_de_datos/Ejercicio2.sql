CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    correo NVARCHAR(255) UNIQUE NOT NULL,
    contraseña_hash NVARCHAR(255) NOT NULL,
    estado VARCHAR(20) CHECK (estado IN ('activo', 'bloqueado', 'pendiente')) DEFAULT 'pendiente',
    fecha_registro DATETIME DEFAULT GETDATE()
);

CREATE TABLE roles (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) UNIQUE NOT NULL,
    descripcion NVARCHAR(MAX)
);

CREATE TABLE user_roles (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    rol_id INT NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES users(id),
    FOREIGN KEY (rol_id) REFERENCES roles(id)
);

CREATE TABLE bitacora (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    accion NVARCHAR(255) NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    ip_address NVARCHAR(45),
    FOREIGN KEY (usuario_id) REFERENCES users(id)
);

CREATE TABLE login_attempts (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    fecha_intento DATETIME DEFAULT GETDATE(),
    exitoso BIT,
    ip_address NVARCHAR(45),
    FOREIGN KEY (usuario_id) REFERENCES users(id)
);

CREATE TABLE user_devices (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    dispositivo NVARCHAR(255),
    sistema_operativo NVARCHAR(100),
    navegador NVARCHAR(100),
    ip_address NVARCHAR(45),
    fecha_registro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (usuario_id) REFERENCES users(id)
);

CREATE PROCEDURE InsertarBitacora
    @usuario_id INT,
    @accion NVARCHAR(255),
    @ip_address NVARCHAR(45)
AS
BEGIN
    INSERT INTO bitacora (usuario_id, accion, ip_address) 
    VALUES (@usuario_id, @accion, @ip_address);
END
GO
