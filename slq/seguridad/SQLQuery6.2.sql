CREATE TABLE Roles (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre_rol VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(255),
    fecha_creacion DATETIME DEFAULT GETDATE()
);




CREATE TABLE Usuario_Rol (
    id_usuario INT,
    id_rol INT,
    fecha_asignacion DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (id_usuario, id_rol),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol)
);




CREATE TABLE Autorizaciones (
    id_autorizacion INT PRIMARY KEY IDENTITY(1,1),
    nombre_autorizacion VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(255)
);





CREATE TABLE Rol_Autorizacion (
    id_rol INT,
    id_autorizacion INT,
    id_sistema INT,
    fecha_asignacion DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (id_rol, id_autorizacion, id_sistema),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol),
    FOREIGN KEY (id_autorizacion) REFERENCES Autorizaciones(id_autorizacion),
    FOREIGN KEY (id_sistema) REFERENCES Sistemas(id_sistema)
);






-- Insertar roles
INSERT INTO Roles (nombre_rol, descripcion) VALUES ('Administrador', 'Acceso total');
INSERT INTO Roles (nombre_rol, descripcion) VALUES ('Usuario', 'Acceso limitado');

-- Insertar autorizaciones
INSERT INTO Autorizaciones (nombre_autorizacion, descripcion) VALUES ('Ver Reportes', 'Puede ver reportes');
INSERT INTO Autorizaciones (nombre_autorizacion, descripcion) VALUES ('Editar Usuarios', 'Puede editar usuarios');

-- Insertar sistemas
INSERT INTO Sistemas (nombre_sistema, descripcion) VALUES ('Sistema Contable', 'Gestiona contabilidad');

-- Asignar autorizaciones al rol en un sistema
INSERT INTO Rol_Autorizacion (id_rol, id_autorizacion, id_sistema)
VALUES (1, 1, 1), -- Administrador puede ver reportes en Sistema Contable
       (1, 2, 1), -- Administrador puede editar usuarios en Sistema Contable
       (2, 1, 1); -- Usuario solo puede ver reportes

-- Crear usuario y asignar rol
INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
VALUES ('jdoe', 'Juan Doe', '123456', 1);

-- Asignar rol "Usuario" al nuevo usuario
INSERT INTO Usuario_Rol (id_usuario, id_rol)
VALUES (1, 2);







-- Verificar si el usuario tiene autorización específica en un sistema
SELECT a.nombre_autorizacion
FROM Usuarios u
JOIN Usuario_Rol ur ON u.id_usuario = ur.id_usuario
JOIN Rol_Autorizacion ra ON ur.id_rol = ra.id_rol
JOIN Autorizaciones a ON ra.id_autorizacion = a.id_autorizacion
JOIN Sistemas s ON ra.id_sistema = s.id_sistema
WHERE u.usuario = 'jdoe'
  AND s.nombre_sistema = 'Sistema Contable'
  AND a.nombre_autorizacion = 'Editar Usuarios';
