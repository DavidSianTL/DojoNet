CREATE TABLE Empresas (
    id_empresa INT PRIMARY KEY IDENTITY(1,1),
    nombre_empresa VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(255),
    fecha_creacion DATETIME DEFAULT GETDATE()
);

ALTER TABLE Usuarios
ADD fk_id_empresa INT;

ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_Empresas FOREIGN KEY (fk_id_empresa)
REFERENCES Empresas(id_empresa);


INSERT INTO Empresas (nombre_empresa, descripcion)
VALUES ('Empresa A', 'Empresa principal'),
       ('Empresa B', 'Sucursal regional');



	   INSERT INTO Estado_Usuario(descripcion)
VALUES ('Pendiente');


select * from Estado_Usuario