use master;
go
drop database if exists DBSistemaDeSeguridad;
go
Create database DBSistemaDeSeguridad;
GO
USE DBSistemaDeSeguridad
GO

CREATE TABLE Estado_Usuario(
    id_estado INT PRIMARY KEY IDENTITY(1, 1),
    descripcion VARCHAR(50) NOT NULL UNIQUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Usuarios(
    id_usuario INT PRIMARY KEY IDENTITY(1, 1),
    usuario VARCHAR(50) NOT NULL UNIQUE,
    nom_usuario VARCHAR (50) NOT NULL,
	contrasenia VARBINARY(64) NOT NULL,
    fk_id_estado INT NOT NULL,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP
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

CREATE TABLE Bitacora(
    id_bitacora INT PRIMARY KEY IDENTITY (1, 1),
    fk_id_usuario INT NOT NULL,
    accion VARCHAR(255) NOT NULL,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (fk_id_usuario) REFERENCES Usuarios(id_usuario)
);

go

INSERT INTO Estado_Usuario (descripcion)
VALUES 
('Activo'),
('Inactivo'),
('Pendiente');

INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
VALUES 
('jlopez', 'Juan López', HASHBYTES('SHA2_256', 'pass123'), 1),
('mrojas', 'María Rojas', HASHBYTES('SHA2_256', 'secure456'), 2),
('cgarcia', 'Carlos García', HASHBYTES('SHA2_256', 'clave789'), 3);

INSERT INTO Sistemas (nombre_sistema, descripcion)
VALUES 
('Inventario', 'Gestión de productos y existencias'),
('Ventas', 'Control de ventas y facturación'),
('RRHH', 'Sistema de Recursos Humanos');

INSERT INTO Permisos (fk_id_usuario, fk_id_sistema, descripcion)
VALUES 
(1, 1, 'Acceso completo a inventario'),
(2, 2, 'Solo lectura en ventas'),
(3, 3, 'Permiso para ver reportes de RRHH');

INSERT INTO Bitacora (fk_id_usuario, accion)
VALUES 
(1, 'Inicio de sesión'),
(2, 'Modificó un registro de ventas'),
(3, 'Consultó reporte de asistencia');

go

select * from Estado_Usuario;
select * from Usuarios;
select usuario, nom_usuario, convert(varchar(64), contrasenia, 2) as contrasenia_hash
from Usuarios;
select * from Sistemas;
select * from Permisos;
select * from Bitacora;

go

-- SP Agregar Usuario
create procedure sp_AgregarUsuario
    @usuario_admin int,
    @usuario varchar(50),
    @nombre varchar(255),
    @contrasenia_plaintext varchar(255),
    @id_estado int
as
begin
    set nocount on;

    declare @nuevo_id int;

    insert into Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado)
    values (@usuario, @nombre, HASHBYTES('SHA2_256', @contrasenia_plaintext), @id_estado);

    set @nuevo_id = SCOPE_IDENTITY();

    -- Mensaje de confirmación
    select 'Usuario registrado correctamente' as Mensaje, @nuevo_id as NuevoID;

    -- Registro en bitácora con el ID del usuario que realiza la acción
    insert into Bitacora (fk_id_usuario, accion)
    values (@usuario_admin, CONCAT('Registró al nuevo usuario: ', @usuario));
END;

go

exec sp_AgregarUsuario 1, 'usuario123', 'nuevo usuario', 'usuario123', 1;
select * from Usuarios;

go

-- SP Agregar Bitácora
create procedure sp_AgregarBitacora
    @id_usuario int,
    @accion nvarchar(255)
as
begin
    set nocount on;

	declare @id_bitacora int;

    insert into Bitacora (fk_id_usuario, accion, fecha)
    values (@id_usuario, @accion, current_timestamp);
    
    set @id_bitacora = SCOPE_IDENTITY();
    
    select 'Bitácora registrada correctamente' as Mensaje, @id_bitacora as NumeroBitacora;
end;

go

exec sp_AgregarBitacora 1, 'Soy una prueba de bitácora';
select * from Bitacora;

go

-- SP Editar Bitácora
create procedure sp_EditarBitacora
	@id_bitacora INT,
    @id_usuario int,
    @accion nvarchar(255)
as
begin
    set nocount on;

	update Bitacora
    set fk_id_usuario = fk_id_usuario, accion = @accion, fecha = current_timestamp
    where id_bitacora = @id_bitacora;
    
    set @id_bitacora = SCOPE_IDENTITY();
    
    select 'Bitácora actualizada correctamente' as Mensaje, @id_bitacora as NumeroBitacora;
end;

go

exec sp_EditarBitacora 4, 1, 'Soy una prueba de bitácora 2.0';
select * from Bitacora;

go

-- SP Eliminar Bitácora
create procedure sp_EliminarBitacora
	@id_bitacora INT
as
begin
    set nocount on;

	delete from Bitacora where id_bitacora = @id_bitacora;
    
    set @id_bitacora = SCOPE_IDENTITY();
    
    select 'Bitácora eliminada correctamente' as Mensaje, @id_bitacora as NumeroBitacora;
end;

go

exec sp_EliminarBitacora 4;
select * from Bitacora;