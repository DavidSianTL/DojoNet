CREATE TABLE Roles (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre_rol VARCHAR(100) NOT NULL UNIQUE
);


CREATE TABLE Usuario_Rol (
    id_usuario INT,
    id_rol INT,
    PRIMARY KEY (id_usuario, id_rol),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol)
);


CREATE TABLE Rol_Permiso (
    id_rol INT,
    id_sistema INT,
    descripcion_permiso VARCHAR(150),
    PRIMARY KEY (id_rol, id_sistema),
    FOREIGN KEY (id_rol) REFERENCES Roles(id_rol),
    FOREIGN KEY (id_sistema) REFERENCES Sistemas(id_sistema)
);
