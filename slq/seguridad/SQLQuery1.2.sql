select * from Empresas


ALTER TABLE Usuarios
ADD fk_id_empresa INT NOT NULL;

ALTER TABLE Usuarios
ADD CONSTRAINT FK_Usuarios_Empresas FOREIGN KEY (fk_id_empresa) REFERENCES Empresas(id_empresa);




CREATE TABLE Usuario_Empresa (
    id_usuario INT NOT NULL,
    id_empresa INT NOT NULL,
    fecha_asignacion DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (id_usuario, id_empresa),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_empresa) REFERENCES Empresas(id_empresa)
);



INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado, fk_id_empresa)
VALUES ('user1', 'Usuario Uno', 'pass123', 1, 1); -- Empresa A



-- Crea usuario
INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
VALUES ('multiuser', 'Usuario Multi', 'pass456', 1);

-- Asócialo a 2 empresas
INSERT INTO Usuario_Empresa (id_usuario, id_empresa) VALUES (2, 1), (2, 2);



SELECT u.usuario, e.nombre_empresa
FROM Usuarios u
JOIN Empresas e ON u.fk_id_empresa = e.id_empresa;



SELECT u.usuario, e.nombre_empresa
FROM Usuario_Empresa ue
JOIN Usuarios u ON ue.id_usuario = u.id_usuario
JOIN Empresas e ON ue.id_empresa = e.id_empresa;




DECLARE @usuario VARCHAR(50) = 'user1';
DECLARE @contrasenia VARCHAR(255) = 'pass123';
DECLARE @empresa INT = 1; -- ID de Empresa A

SELECT u.id_usuario, u.nom_usuario, e.nombre_empresa
FROM Usuarios u
JOIN Empresas e ON u.fk_id_empresa = e.id_empresa
WHERE u.usuario = @usuario AND u.contrasenia = @contrasenia AND e.id_empresa = @empresa;



SELECT 
    u.id_usuario,
    u.usuario,
    u.nom_usuario,
    e.id_empresa,
    e.nombre_empresa
FROM Usuario_Empresa ue
JOIN Usuarios u ON ue.id_usuario = u.id_usuario
JOIN Empresas e ON ue.id_empresa = e.id_empresa
ORDER BY u.id_usuario, e.id_empresa;
