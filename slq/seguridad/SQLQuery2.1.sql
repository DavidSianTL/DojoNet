CREATE TABLE Roles (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre_rol VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(255)
);



CREATE TABLE Usuario_Rol (
    id_usuario INT NOT NULL,
    id_rol INT NOT NULL,
    id_empresa INT NOT NULL,
    id_sistema INT NOT NULL,
    fecha_asignacion DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (id_usuario, id_rol, id_empresa, id_sistema),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol),
    FOREIGN KEY (id_empresa) REFERENCES Empresas(id_empresa),
    FOREIGN KEY (id_sistema) REFERENCES Sistemas(id_sistema)
);



CREATE TABLE Acciones (
    id_accion INT PRIMARY KEY IDENTITY(1,1),
    nombre_accion VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(255)
);



CREATE TABLE Rol_Accion (
    id_rol INT NOT NULL,
    id_accion INT NOT NULL,
    PRIMARY KEY (id_rol, id_accion),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol),
    FOREIGN KEY (id_accion) REFERENCES Acciones(id_accion)
);

-- Acción
INSERT INTO Acciones (nombre_accion, descripcion) VALUES ('EditarUsuario', 'Puede editar usuarios');

-- Rol
INSERT INTO Roles (nombre_rol, descripcion) VALUES ('Administrador', 'Rol con permisos totales');

-- Asocia el rol a la acción
INSERT INTO Rol_Accion (id_rol, id_accion)
SELECT r.id_rol, a.id_accion
FROM Roles r, Acciones a
WHERE r.nombre_rol = 'Administrador' AND a.nombre_accion = 'EditarUsuario';

-- Asocia el usuario al rol en una empresa y sistema
INSERT INTO Usuario_Rol (id_usuario, id_rol, id_empresa, id_sistema)
SELECT 5, r.id_rol, 1, 2
FROM Roles r
WHERE r.nombre_rol = 'Administrador';



SELECT 1
FROM Usuario_Rol ur
JOIN Rol_Accion ra ON ur.id_rol = ra.id_rol
JOIN Acciones a ON ra.id_accion = a.id_accion
WHERE 
    ur.id_usuario = 5 AND
    ur.id_empresa = 1 AND
    ur.id_sistema = 2 AND
    a.nombre_accion = 'EditarUsuario';




	SELECT * FROM Usuarios WHERE id_usuario = 5;


	SELECT * FROM Usuario_Rol
WHERE id_usuario = 5 AND id_empresa = 1 AND id_sistema = 2;



SELECT ra.*
FROM Rol_Accion ra
JOIN Acciones a ON ra.id_accion = a.id_accion
WHERE a.nombre_accion = 'EditarUsuario';



SELECT ur.id_usuario, ur.id_rol, ur.id_empresa, ur.id_sistema, a.nombre_accion
FROM Usuario_Rol ur
JOIN Rol_Accion ra ON ur.id_rol = ra.id_rol
JOIN Acciones a ON ra.id_accion = a.id_accion
WHERE ur.id_usuario = 5;
