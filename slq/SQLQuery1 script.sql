create database SistemaSeguridad 
go
use SistemaSeguridad
go


CREATE TABLE Estado_Usuario (
    id_estado INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL UNIQUE,
    fecha_creacion DATETIME DEFAULT GETDATE()
);


CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    usuario VARCHAR(50) NOT NULL UNIQUE,
    nom_usuario VARCHAR(100) NOT NULL,
    contrasenia VARBINARY(64) NOT NULL,
    fk_id_estado INT NOT NULL,
    fecha_creacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (fk_id_estado) REFERENCES Estado_Usuario(id_estado)
);



create table Sistemas(
id_sistema int primary key identity(1,1),
nombre_sitema varchar(150) not null,
descripcion varchar(500)
);

create table Permisos(
id_permisos int primary key identity(1,1),
fk_id_usuario int not null,
fk_id_sistema int not null,
decripcion varchar(150) not null,
foreign key(fk_id_usuario) references Usuario(id_usuario),
foreign key(fk_id_sistema) references Sistemas(id_sistema)
);


create table Bitacora(
id_bitacora int primary key identity(1,1),
fk_id_usuario int not null,
accion varchar (255) not null,
fecha datetime default getdate(),
foreign key(fk_id_usuario) references Usuario(id_usuario)
);

GO